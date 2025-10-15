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

namespace EksiSozluk.Api.Application.Features.Queries.GetEntryComments
{
    public class GetEntryCommentsQueryHandler : IRequestHandler<GetEntryCommentsQuery, PagedViewModel<GetEntryCommentsViewModel>>
    {
        private readonly IEntryCommentRepository entryCommentRepository;

        public GetEntryCommentsQueryHandler(IEntryCommentRepository entryCommentRepository)
        {
            this.entryCommentRepository = entryCommentRepository;
        }

        public async Task<PagedViewModel<GetEntryCommentsViewModel>> Handle(GetEntryCommentsQuery request, CancellationToken cancellationToken)
        {
            var query = entryCommentRepository.AsQueryable();
            query = query.Include(i => i.EntryCommentFavorites)
                .Include(i => i.EntryCommentVotes)
                .Include(i => i.CreatedBy)
                .Where(i=>i.EntryId==request.EntryId);

            var list = query.Select(i => new GetEntryCommentsViewModel()
            {
                Id = i.Id,
                Content = i.Content,
                IsFavorited = request.UserId.HasValue && i.EntryCommentFavorites.Any(j => j.CreatedById == request.UserId),
                FavoritedCount = i.EntryCommentFavorites.Count,
                CreatedDate = i.CreateDate,
                CreatedByUserName = i.CreatedBy.Username,
                VoteType = request.UserId.HasValue && i.EntryCommentVotes.Any(j => j.CreatedById == request.UserId) ? i.EntryCommentVotes.FirstOrDefault(j => j.CreatedById == request.UserId).VoteType : VoteType.None
            });

            var entries = await list.GetPaged(request.Page, request.PageSize);
            return entries;
        }
    }
}
