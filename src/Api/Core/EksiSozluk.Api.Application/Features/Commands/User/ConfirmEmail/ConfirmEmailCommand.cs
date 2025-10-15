using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Api.Application.Features.Commands.User.ConfirmEmail
{
    public class ConfirmEmailCommand:IRequest<bool>
    {
        public Guid ConfirmationId { get; set; }
    }
}
