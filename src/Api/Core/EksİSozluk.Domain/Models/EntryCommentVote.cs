using EksiSozluk.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksİSozluk.Domain.Models
{
    public class EntryCommentVote:BaseEntity
    {
        public Guid EntryCommentId { get; set; }   
        public VoteType VoteType { get; set; }  
        public Guid CreatedById { get; set; }   

        public virtual EntryComment EntryComment { get; set; }    
    }
}
