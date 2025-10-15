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
    public class EntryCommentEntityConfiguration : BaseEntityConfiguration<EntryComment>
    {
        public override void Configure(EntityTypeBuilder<EntryComment> builder)
        {
            base.Configure(builder);

            builder.ToTable("entrycomment", EksiSozlukContext.DEFAULT_SCHEMA);

            builder.HasOne(e => e.CreatedBy).WithMany(e => e.EntryComments).HasForeignKey(e => e.CreatedById).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.Entry).WithMany(e => e.EntryComments).HasForeignKey(e => e.EntryId);


        }
    }
}
