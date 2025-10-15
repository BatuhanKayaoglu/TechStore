using AutoMapper;
using AutoMapper.QueryableExtensions;
using EksiSozluk.Api.Application.Repositories;
using EksiSozluk.Common.ViewModels.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EksiSozluk.Api.Application.Features.Queries.GetEntries
{
    public class GetEntriesQueryHandler : IRequestHandler<GetEntriesQuery, List<GetEntriesViewModel>>
    {
        private readonly IEntryRepository entryRepository;
        private readonly IMapper mapper;

        public GetEntriesQueryHandler(IEntryRepository entryRepository, IMapper mapper)
        {
            this.entryRepository = entryRepository;
            this.mapper = mapper;
        }

        public async Task<List<GetEntriesViewModel>> Handle(GetEntriesQuery request, CancellationToken cancellationToken)
        {
            var query = entryRepository.AsQueryable();

            if (request.TodaysEntries)
            {
                query = query.Where(i => i.CreateDate >= DateTime.Now.Date) // sadece bugünden(00.00) itibaren olan kayıtlar
                .Where(i => i.CreateDate >= DateTime.Now.AddDays(1).Date);
            }

            query.Include(i => i.EntryComments)
                .OrderBy(i => Guid.NewGuid()) // rastgele sıralasın diye
                .Take(request.Count);

            return await query.ProjectTo<GetEntriesViewModel>(mapper.ConfigurationProvider).ToListAsync(cancellationToken); // gidicek burdaki modele bakıcak ve içersinde hangi alanlar varsa db'de sorgu calıstırırken sadee  o alanı yazıcak ve performans kazancaz.

        }
    }
}
