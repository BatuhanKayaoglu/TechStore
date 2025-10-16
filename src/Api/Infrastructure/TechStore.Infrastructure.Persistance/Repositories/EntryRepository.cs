using TechStore.Api.Application.Repositories;
using TechStore.Infrastructure.Persistance.Context;
using TechStore.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Infrastructure.Persistance.Repositories
{
    public class EntryRepository : GenericRepository<Entry>, IEntryRepository
    {
        public EntryRepository(TechStoreContext dbContext) : base(dbContext)
        {
        }
    }
}
