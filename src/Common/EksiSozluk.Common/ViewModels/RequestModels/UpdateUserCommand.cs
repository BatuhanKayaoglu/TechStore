using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Common.ViewModels.RequestModels
{
    public class UpdateUserCommand:IRequest<Guid>
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Username { get; set; }
        //public string Password { get; set; } // password'ün değiştirilmesi işlemini ayrı bir endpoint'de yapacagız.
    }
}
