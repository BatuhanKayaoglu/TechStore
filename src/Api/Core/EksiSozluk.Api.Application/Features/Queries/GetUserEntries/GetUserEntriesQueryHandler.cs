using EksiSozluk.Api.Application.Repositories;
using EksiSozluk.Common.Infrastructure.Extensions;
using EksiSozluk.Common.Models.Page;
using EksiSozluk.Common.ViewModels;
using EksiSozluk.Common.ViewModels.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Api.Application.Features.Queries.GetUserEntries
{
    public class GetUserEntriesQueryHandler : IRequestHandler<GetUserEntriesQuery, PagedViewModel<GetUserEntriesViewModel>>
    {
        private readonly IEntryRepository entryRepository;

        public GetUserEntriesQueryHandler(IEntryRepository entryRepository)
        {
            this.entryRepository = entryRepository;
        }

        public async Task<PagedViewModel<GetUserEntriesViewModel>> Handle(GetUserEntriesQuery request, CancellationToken cancellationToken)
        {
            var query = entryRepository.AsQueryable();

            if (request.UserId != null && request.UserId.HasValue && request.UserId != Guid.Empty)
                query = query.Where(i => i.CreatedById == request.UserId);

            else if (!string.IsNullOrEmpty(request.UserName))
                query = query.Where(i => i.CreatedBy.Username == request.UserName);

            else return null;

            query = query.Include(i => i.EntryFavorites)
                .Include(i => i.CreatedBy);


            var list = query.Select(i => new GetUserEntriesViewModel()
            {
                Id = i.Id,
                Content = i.Content,
                IsFavorited = request.UserId.HasValue && i.EntryFavorites.Any(j => j.CreatedById == request.UserId),
                FavoritedCount = i.EntryFavorites.Count,
                CreatedDate = i.CreateDate,
                CreatedByUserName = i.CreatedBy.Username,
            });

            var entries = await list.GetPaged(request.Page, request.PageSize);
            return entries;
        }
    }
}
