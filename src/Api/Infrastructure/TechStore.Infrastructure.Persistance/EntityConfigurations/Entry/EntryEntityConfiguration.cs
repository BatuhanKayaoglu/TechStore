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
    public class EntryEntityConfiguration : BaseEntityConfiguration<TechStore.Domain.Models.Entry>
    {
        public override void Configure(EntityTypeBuilder<TechStore.Domain.Models.Entry> builder)
        {
            base.Configure(builder);

            builder.ToTable("entry", TechStoreContext.DEFAULT_SCHEMA);

            builder.HasOne(e => e.CreatedBy).WithMany(e => e.Entries).HasForeignKey(e => e.CreatedById);
        }
    }
}
