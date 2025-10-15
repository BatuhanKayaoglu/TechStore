using EksiSozluk.Api.Application.Repositories;
using EksiSozluk.Common.ViewModels.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Api.Application.Features.Queries.SearchBySubject
{
    public class SearchEntryQueryHandler : IRequestHandler<SearchEntryQuery, List<SearchEntryViewModel>>
    {
        private readonly IEntryRepository entryRepository;

        public SearchEntryQueryHandler(IEntryRepository entryRepository)
        {
            this.entryRepository = entryRepository;
        }

        public Task<List<SearchEntryViewModel>> Handle(SearchEntryQuery request, CancellationToken cancellationToken)
        {
            var result = entryRepository.Get(i => EF.Functions.Like(i.Subject, $"{request.SearchText}"))
                .Select(i => new SearchEntryViewModel
                {
                    Id = i.Id,
                    Subject = i.Subject
                });

            return result.ToListAsync(cancellationToken);
        }
    }
}
