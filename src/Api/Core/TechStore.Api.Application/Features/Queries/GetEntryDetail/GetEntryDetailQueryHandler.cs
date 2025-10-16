using AutoMapper;
using TechStore.Api.Application.Cache;
using TechStore.Api.Application.Features.Queries.GetEntryComments;
using TechStore.Api.Application.Repositories;
using TechStore.Common.Infrastructure.Extensions;
using TechStore.Common.Models.Page;
using TechStore.Common.ViewModels;
using TechStore.Common.ViewModels.Queries;
using TechStore.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Api.Application.Features.Queries.GetEntryDetail
{
    public class GetEntryDetailQueryHandler : IRequestHandler<GetEntryDetailQuery, GetEntryDetailViewModel>
    {
        private readonly IEntryRepository entryRepository;
        private readonly IGenericRedisService<Entry> genericRedisService;
        private readonly IMapper mapper;

        public GetEntryDetailQueryHandler(IEntryRepository entryRepository, IGenericRedisService<Entry> genericRedisService, IMapper mapper)
        {
            this.entryRepository = entryRepository;
            this.genericRedisService = genericRedisService;
            this.mapper = mapper;
        }

        public async Task<GetEntryDetailViewModel> Handle(GetEntryDetailQuery request, CancellationToken cancellationToken)
        {
            var query = entryRepository.AsQueryable();
            query = query.Include(i => i.EntryFavorites)
                .Include(i => i.CreatedBy)
                .Include(i => i.EntryVotes)
                .Where(i => i.Id == request.EntryId);

            var list = query.Select(i => new GetEntryDetailViewModel()
            {
                Id = i.Id,
                Content = i.Content,
                IsFavorited = request.UserId.HasValue && i.EntryFavorites.Any(j => j.CreatedById == request.UserId),
                FavoritedCount = i.EntryFavorites.Count,
                CreatedDate = i.CreateDate,
                CreatedByUserName = i.CreatedBy.Username,
                VoteType = request.UserId.HasValue && i.EntryVotes.Any(j => j.CreatedById == request.UserId) ? i.EntryVotes.FirstOrDefault(j => j.CreatedById == request.UserId).VoteType : VoteType.None
            });




            return await list.FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }
    }
}
