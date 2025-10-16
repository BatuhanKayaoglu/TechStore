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
    public class EntryVoteEntityConfiguration : BaseEntityConfiguration<EntryVote>
    {
        public override void Configure(EntityTypeBuilder<EntryVote> builder)
        {
            base.Configure(builder);

            builder.ToTable("entryvote", TechStoreContext.DEFAULT_SCHEMA);

            builder.HasOne(e => e.Entry).WithMany(e => e.EntryVotes).HasForeignKey(e => e.EntryId);
        }
    }
}
