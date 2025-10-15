using AutoMapper;
using EksiSozluk.Api.Application.Cache;
using EksiSozluk.Api.Application.Repositories;
using EksiSozluk.Common.ViewModels.RequestModels;
using EksİSozluk.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Api.Application.Features.Commands.Entry.Create
{
    public class CreateEntryCommandHandler : IRequestHandler<CreateEntryCommand, Guid>
    {
        private readonly IEntryRepository entryRepository;
        private readonly IMapper mapper;
        private readonly IGenericRedisService<EksİSozluk.Domain.Models.Entry> genericRedisService;

        public CreateEntryCommandHandler(IEntryRepository entryRepository, IMapper mapper, IGenericRedisService<EksİSozluk.Domain.Models.Entry> genericRedisService)
        {
            this.entryRepository = entryRepository;
            this.mapper = mapper;
            this.genericRedisService = genericRedisService;
        }

        public async Task<Guid> Handle(CreateEntryCommand request, CancellationToken cancellationToken)
        {
            var dbEntry = mapper.Map<EksİSozluk.Domain.Models.Entry>(request);

            await entryRepository.AddAsync(dbEntry);

            // for redisCacheService    
            var mappedData = mapper.Map<EksİSozluk.Domain.Models.Entry>(dbEntry);
            await genericRedisService.SetAsync(mappedData, dbEntry.Id, cancellationToken);

            return dbEntry.Id;

        }
    }
}
