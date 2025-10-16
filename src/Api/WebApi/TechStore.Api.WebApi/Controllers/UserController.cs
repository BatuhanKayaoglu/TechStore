using TechStore.Api.Application.Cache;
using TechStore.Api.Application.Features.Commands.User.ConfirmEmail;
using TechStore.Api.Application.Features.Queries.GetAllUser;
using TechStore.Api.Application.Features.Queries.GetUserDetail;
using TechStore.Common.Events.User;
using TechStore.Common.ViewModels.Queries;
using TechStore.Common.ViewModels.RequestModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace TechStore.Api.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {

        private readonly IMediator mediator;
        private readonly IRedisCacheService redisCacheService;
        private readonly IDistributedCache distributedCache;

        public UserController(IMediator mediator, IRedisCacheService redisCacheService, IDistributedCache distributedCache)
        {
            this.mediator = mediator;
            this.redisCacheService = redisCacheService;
            this.distributedCache = distributedCache;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            List<GetAllUserViewModel> users = await mediator.Send(new GetAllUserQuery());
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var user = await mediator.Send(new GetUserDetailQuery(id));
            return Ok(user);
        }

        [HttpGet]
        [Route("UserName/{userName}")]
        public async Task<IActionResult> GetByUserName(string userName)
        {
            var user = await mediator.Send(new GetUserDetailQuery(Guid.Empty, userName));
            return Ok(user);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var res = await mediator.Send(command);
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
        {
            var res = await mediator.Send(command);
            return Ok(res);
        }


        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand command)
        {
            var res = await mediator.Send(command);
            return Ok(res);
        }

        [HttpPost]
        [Route("ConfirmEmail")]
        public async Task<IActionResult> Confirm(Guid id)
        {
            var res = await mediator.Send(new ConfirmEmailCommand() { ConfirmationId = id });
            return Ok(res);
        }

        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangeUserPasswordCommand command)
        {
            if (command?.UserId is null) // eğer ki dışarıdan bu id'yi göndermedikleri bir senaryo olursa jwtToken'dan gelen UserId bilgisi CreatedById bilgisine atansın.
                command.UserId = UserId;
            var res = await mediator.Send(command);

            return Ok(res);
        }

    }
}
