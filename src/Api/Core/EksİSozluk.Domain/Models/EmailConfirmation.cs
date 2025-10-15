using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksİSozluk.Domain.Models
{
    public class EmailConfirmation:BaseEntity // bir kullanıcı olustururken veya halihazırdaki kullanıcı emailini değiştireceği zaman kullanacagız.
    {
        public string OldEmailAdress { get; set; }
        public string NewEmailAdress { get; set; }
    }
}
