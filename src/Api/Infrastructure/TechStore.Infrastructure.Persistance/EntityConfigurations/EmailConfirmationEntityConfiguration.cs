using TechStore.Infrastructure.Persistance.Context;
using TechStore.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Infrastructure.Persistance.EntityConfigurations.Entry
{
    public class EmaiLConfirmationEntityConfiguration : BaseEntityConfiguration<EmailConfirmation>
    {
        public override void Configure(EntityTypeBuilder<EmailConfirmation> builder)
        {
            base.Configure(builder);

            builder.ToTable("emailConfirmation", TechStoreContext.DEFAULT_SCHEMA);
        }
    }
}
