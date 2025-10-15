using EksiSozluk.Common.Models.Page;
using EksiSozluk.Common.ViewModels.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Api.Application.Features.Queries.GetEntryComments
{
    public class GetEntryCommentsQuery : BasePagedQuery,IRequest<PagedViewModel<GetEntryCommentsViewModel>>
    {
        public GetEntryCommentsQuery(int page, int pageSize, Guid entryId, Guid? userId) : base(page, pageSize)
        {
            EntryId = entryId;
            UserId = userId;
        }

        // dışardan ne alacagımızı yazıyoruz.
        public Guid EntryId { get; set; }   
        public Guid? UserId { get; set; }    
    }
}
