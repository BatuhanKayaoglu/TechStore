using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace EksİSozluk.Domain.Models
{
    public class Entry:BaseEntity
    {
        public string Subject { get; set; }
        public string Content { get; set; }
        public Guid CreatedById { get; set; }

        public virtual User CreatedBy { get; set; } 

        public virtual ICollection<EntryComment> EntryComments { get; set; }  // o entry'e ait commentler olacak.

        public virtual ICollection<EntryVote> EntryVotes { get; set; }  

        public virtual ICollection<EntryFavorite> EntryFavorites { get; set; }
    }
}
