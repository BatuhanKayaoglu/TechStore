using AutoMapper;
using EksiSozluk.Api.Application.Repositories;
using EksiSozluk.Common.Events.User;
using EksiSozluk.Common.Infrastructure;
using EksiSozluk.Common;
using EksiSozluk.Common.Infrastructure.Exceptions;
using EksiSozluk.Common.ViewModels.RequestModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Api.Application.Features.Commands.User.Update
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Guid>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public UpdateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }
        public async Task<Guid> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var dbUser = await userRepository.GetByIdAsync(request.Id);

            if (dbUser == null)
                throw new DatabaseValidationException("User not found!");


            var dbEmaillAdress = dbUser.EmailAddress;
            var emailChanged = string.CompareOrdinal(dbEmaillAdress, request.EmailAddress) != 0; // bu bana true false dönecek db'deki email ile bana yeni gelen email arasında fark varsa


            //dbUser = mapper.Map<EksİSozluk.Domain.Models.User>(request);
            mapper.Map(dbUser, request); // üstteki gibi yaparsak yeni bir User olusturup request datalarını onun içine atmıs olacak fakat bu sekilde yaparsak request içindekileri 'dbUser' icersine atmıs olacak.

            var rows = await userRepository.UpdateAsync(dbUser);

            // Check if email changed (email değiştiyse sadece bir bilgi mesajı göndermek istiyoruz.)

            if (rows > 0 && emailChanged)
            {
                var @event = new UserEmailChangedEvent()
                {
                    OldEmailAdress = null,
                    NewEmailAdress = dbUser.EmailAddress
                };

                QueueFactory.SendMessageToExchange(exchangeName: SozlukConstants.UserExchangeName, exchangeType: SozlukConstants.DefaultExchangeType, queueName: SozlukConstants.UserEmailChangedQueueName, obj: @event);

                dbUser.EmailConfirmed = false;
                await userRepository.UpdateAsync(dbUser);   
            }

            return dbUser.Id;  

        }
    }
}
