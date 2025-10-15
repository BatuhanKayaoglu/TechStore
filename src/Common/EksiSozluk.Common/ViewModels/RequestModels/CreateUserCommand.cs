using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Common.ViewModels.RequestModels
{
    // Dışarıdan alacagım parametreler için. (Bu parametreleri hem application hem de webApi tarafında ortak kullanacagım için Common'a ekledim.)
    public class CreateUserCommand:IRequest<Guid> // Dışarıya Guid döneceğim.
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
