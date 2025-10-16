using AutoMapper;
using TechStore.Api.Application.Repositories;
using TechStore.Common.Infrastructure.Exceptions;
using TechStore.Common.ViewModels.RequestModels;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Domain.Models;
using TechStore.Common.Infrastructure;
using TechStore.Common.Events.User;
using TechStore.Common;
using TechStore.Api.Application.Cache;

namespace TechStore.Api.Application.Features.Commands.User.Create
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly IRedisCacheService redisCacheService;

        public CreateUserCommandHandler(IUserRepository userRepository, IMapper mapper, IRedisCacheService redisCacheService)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.redisCacheService = redisCacheService;
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var existUser = await userRepository.GetSingleAsync(i => i.EmailAddress == request.EmailAddress);

            if (existUser is not null)
                throw new DatabaseValidationException("User already exist");

            var dbUser = mapper.Map<TechStore.Domain.Models.User>(request);

            var rows = await userRepository.AddAsync(dbUser);

            await redisCacheService.SetAsync(dbUser, default);  // add redisCache 

            // Email changed/created
            if (rows > 0)
            {
                var @event = new UserEmailChangedEvent()
                {
                    OldEmailAdress = null,
                    NewEmailAdress = dbUser.EmailAddress
                };

                // send message to RabbitMQ for email changed/created   
                QueueFactory.SendMessageToExchange(exchangeName: SozlukConstants.UserExchangeName, exchangeType: SozlukConstants.DefaultExchangeType, queueName: SozlukConstants.UserEmailChangedQueueName, obj: @event);
            }

            return dbUser.Id;


        }


    }
}
