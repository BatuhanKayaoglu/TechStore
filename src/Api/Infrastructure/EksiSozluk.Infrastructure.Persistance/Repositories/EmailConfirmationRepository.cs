using EksiSozluk.Api.Application.Repositories;
using EksiSozluk.Infrastructure.Persistance.Context;
using EksİSozluk.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Infrastructure.Persistance.Repositories
{
    public class EmailConfirmationRepository : GenericRepository<EmailConfirmation>, IEmailConfirmationRepository
    {
        public EmailConfirmationRepository(EksiSozlukContext dbContext) : base(dbContext)
        {

        }
    }
}
