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
    public class EntryCommentRepository : GenericRepository<EntryComment>, IEntryCommentRepository
    {
        public EntryCommentRepository(TechStoreContext dbContext) : base(dbContext)
        {
        }
    }
}
