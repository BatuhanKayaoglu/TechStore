using EksiSozluk.Common.ViewModels.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Api.Application.Features.Queries.GetAllUser
{
    public class GetAllUserQuery : IRequest<List<GetAllUserViewModel>>
    {
    }
}
