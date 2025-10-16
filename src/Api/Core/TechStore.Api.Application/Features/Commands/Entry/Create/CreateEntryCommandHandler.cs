using AutoMapper;
using TechStore.Api.Application.Cache;
using TechStore.Api.Application.Repositories;
using TechStore.Common.ViewModels.RequestModels;
using TechStore.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Api.Application.Features.Commands.Entry.Create
{
    public class CreateEntryCommandHandler : IRequestHandler<CreateEntryCommand, Guid>
    {
        private readonly IEntryRepository entryRepository;
        private readonly IMapper mapper;
        private readonly IGenericRedisService<TechStore.Domain.Models.Entry> genericRedisService;

        public CreateEntryCommandHandler(IEntryRepository entryRepository, IMapper mapper, IGenericRedisService<TechStore.Domain.Models.Entry> genericRedisService)
        {
            this.entryRepository = entryRepository;
            this.mapper = mapper;
            this.genericRedisService = genericRedisService;
        }

        public async Task<Guid> Handle(CreateEntryCommand request, CancellationToken cancellationToken)
        {
            var dbEntry = mapper.Map<TechStore.Domain.Models.Entry>(request);

            await entryRepository.AddAsync(dbEntry);

            // for redisCacheService    
            var mappedData = mapper.Map<TechStore.Domain.Models.Entry>(dbEntry);
            await genericRedisService.SetAsync(mappedData, dbEntry.Id, cancellationToken);

            return dbEntry.Id;

        }
    }
}
