using AutoMapper;
using EksiSozluk.Api.Application.Repositories;
using EksiSozluk.Common.Infrastructure.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Api.Application.Features.Commands.User.ConfirmEmail
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, bool>
    {
        private readonly IEmailConfirmationRepository  emailConfirmationRepository;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public ConfirmEmailCommandHandler(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task<bool> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var confirmation = await emailConfirmationRepository.GetByIdAsync(request.ConfirmationId);
            if (confirmation == null)
                throw new DatabaseValidationException("Confirmation not found!");

            var dbUser = await userRepository.GetSingleAsync(i => i.EmailAddress == confirmation.NewEmailAdress);

            if(dbUser.EmailConfirmed)
                throw new DatabaseValidationException("email address is already confirmed!");

            dbUser.EmailConfirmed = true;
            await userRepository.UpdateAsync(dbUser);   

            return true;


        }
    }
}
