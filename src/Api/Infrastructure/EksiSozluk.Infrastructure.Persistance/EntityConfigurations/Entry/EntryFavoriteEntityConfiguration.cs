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
    public class EntryFavoriteEntityConfiguration:BaseEntityConfiguration<EntryFavorite>
    {
        public override void Configure(EntityTypeBuilder<EntryFavorite> builder)
        {
            base.Configure(builder);

            builder.ToTable("entryfavorite", EksiSozlukContext.DEFAULT_SCHEMA);

            builder.HasOne(e => e.Entry).WithMany(e=>e.EntryFavorites).HasForeignKey(e=>e.EntryId);
            builder.HasOne(e => e.CreatedUser).WithMany(e=>e.EntryFavorites).HasForeignKey(e=>e.CreatedById).OnDelete(DeleteBehavior.Restrict);
        }      
    }
}
