using AutoMapper;
using TechStore.Api.Application.Repositories;
using TechStore.Common.Events.Entry;
using TechStore.Common;
using TechStore.Common.Events.User;
using TechStore.Common.Infrastructure;
using TechStore.Common.Infrastructure.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Api.Application.Features.Commands.User.ChangePassword
{
    public class ChangeUserPasswordCommandHandler : IRequestHandler<ChangeUserPasswordCommand, bool>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public ChangeUserPasswordCommandHandler(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }
        public async Task<bool> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var dbUser = await userRepository.GetByIdAsync(request.UserId);

            if (dbUser is null)
                throw new DatabaseValidationException("User not found.");


            //if (dbUser.Password!=PasswordEncrypter.Encrypt(request.OldPassword))
            //    throw new DatabaseValidationException("User password is not matched.");

            //dbUser.Password = PasswordEncrypter.Encrypt(request.NewPassword);

            QueueFactory.SendMessageToExchange(exchangeName: SozlukConstants.UserExchangeName,
               exchangeType: SozlukConstants.DefaultExchangeType,
               queueName: SozlukConstants.UserPasswordChangedQueueName,
               obj: new UserPasswordChangedEvent()
               {
                   Id = request.UserId,
                   OldPassword = request.OldPassword,
                   NewPassword = request.NewPassword
               });

            return await Task.FromResult(true);

            //dbUser.Password = request.NewPassword;
            //await userRepository.UpdateAsync(dbUser);

            //return true;
        }
    }
}
