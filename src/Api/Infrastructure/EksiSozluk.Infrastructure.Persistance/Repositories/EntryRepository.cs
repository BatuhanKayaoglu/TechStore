using EksiSozluk.Api.Application.Repositories;
using EksiSozluk.Infrastructure.Persistance.Context;
using EksİSozluk.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Infrastructure.Persistance.Repositories
{
    public class EntryRepository : GenericRepository<Entry>, IEntryRepository
    {
        public EntryRepository(EksiSozlukContext dbContext) : base(dbContext)
        {
        }
    }
}
