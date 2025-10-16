using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Common.Events.EntryComment
{
    public class CreateEntryCommentFavEvent
    {
        public Guid EntryCommentId { get; set; }
        public Guid CreatedBy { get; set; } // iki tarafta da kullanılacagı için anlaşılır olması adına burada UserId yerine CreatedBy dedik.
    }
}
