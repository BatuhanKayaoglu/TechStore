using TechStore.Api.Application.Repositories;
using TechStore.Infrastructure.Persistance.Context;
using TechStore.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Infrastructure.Persistance.Repositories
{
    public class EmailConfirmationRepository : GenericRepository<EmailConfirmation>, IEmailConfirmationRepository
    {
        public EmailConfirmationRepository(TechStoreContext dbContext) : base(dbContext)
        {

        }
    }
}
