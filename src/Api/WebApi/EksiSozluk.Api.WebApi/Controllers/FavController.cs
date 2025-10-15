using EksiSozluk.Api.Application.Features.Commands.Entry.CreateFav;
using EksiSozluk.Api.Application.Features.Commands.Entry.DeleteFav;
using EksiSozluk.Api.Application.Features.Commands.EntryComment.CreateFav;
using EksiSozluk.Api.Application.Features.Commands.EntryComment.DeleteFav;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EksiSozluk.Api.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavController : BaseController
    {
        private readonly IMediator mediator;

        public FavController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [Route("Entry/{entryId}")]
        public async Task<IActionResult> CreateEntryFav(Guid entryId)
        {
            var result = await mediator.Send(new CreateEntryFavCommand(entryId, UserId));
            return Ok(result);
        }

        [HttpPost]
        [Route("EntryComment/{entryCommentId}")]
        public async Task<IActionResult> CreateEntryCommentFav(Guid entryCommentId)
        {
            var result = await mediator.Send(new CreateEntryCommentFavCommand(entryCommentId, UserId));
            return Ok(result);
        }


        [HttpDelete]
        [Route("DeleteEntryFav/{enmtryId}")]
        public async Task<IActionResult> DeleteEntryFav(Guid entryId)
        {
            var result = await mediator.Send(new DeleteEntryFavCommand(entryId, UserId));
            return Ok(result);
        }

        [HttpDelete]
        [Route("DeleteEntryCommentFav/{entryCommentId}")]
        public async Task<IActionResult> DeleteEntryCommentFav(Guid entryCommentId)
        {
            var result = await mediator.Send(new DeleteEntryCommentFavCommand(entryCommentId, UserId));
            return Ok(result);
        }
    }
}
