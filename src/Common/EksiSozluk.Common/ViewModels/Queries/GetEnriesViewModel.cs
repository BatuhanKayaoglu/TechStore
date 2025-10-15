using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Common.ViewModels.Queries
{
    public class GetEntriesViewModel // birisi api'ye istek yaptıgında ne dönceğimizi burda belirliyoruz.
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public int CommentCount { get; set; }
    }
}
