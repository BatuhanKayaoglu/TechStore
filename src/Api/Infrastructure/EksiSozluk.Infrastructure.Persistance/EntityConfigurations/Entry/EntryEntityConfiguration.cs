using EksiSozluk.Infrastructure.Persistance.Context;
using EksİSozluk.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Infrastructure.Persistance.EntityConfigurations.Entry
{
    public class EntryEntityConfiguration:BaseEntityConfiguration<EksİSozluk.Domain.Models.Entry>
    {
        public override void Configure(EntityTypeBuilder<EksİSozluk.Domain.Models.Entry> builder)
        {
            base.Configure(builder);

            builder.ToTable("entry", EksiSozlukContext.DEFAULT_SCHEMA);

            builder.HasOne(e => e.CreatedBy).WithMany(e=>e.Entries).HasForeignKey(e=>e.CreatedById);
        }      
    }
}
