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
    public class EntryCommentFavoriteEntityConfiguration : BaseEntityConfiguration<EntryCommentFavorite>
    {
        public override void Configure(EntityTypeBuilder<EntryCommentFavorite> builder)
        {
            base.Configure(builder);

            builder.ToTable("entryCommentFavorite", EksiSozlukContext.DEFAULT_SCHEMA);

            builder.HasOne(e => e.EntryComment).WithMany(e=>e.EntryCommentFavorites).HasForeignKey(e=>e.EntryCommentId);
            builder.HasOne(e => e.CreatedUser).WithMany(e=>e.EntryCommentFavorites).HasForeignKey(e=>e.CreatedById).OnDelete(DeleteBehavior.Restrict);
          
        }      
    }
}
