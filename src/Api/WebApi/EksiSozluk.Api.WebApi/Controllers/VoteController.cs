using EksiSozluk.Api.Application.Features.Commands.Entry.DeleteVote;
using EksiSozluk.Api.Application.Features.Queries.GetUserDetail;
using EksiSozluk.Common.ViewModels;
using EksiSozluk.Common.ViewModels.RequestModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EksiSozluk.Api.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoteController : BaseController
    {
        private readonly IMediator mediator;
        public VoteController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [Route("Entry/{entryId}")]
        public async Task<IActionResult> CreateEntryVote(Guid entryId,VoteType voteType=VoteType.UpVote)
        {
            var result = await mediator.Send(new CreateEntryVoteCommand(entryId,voteType,UserId));

            return Ok(result);
        }


        [HttpPost]
        [Route("EntryComment/{entryCommentId}")]
        public async Task<IActionResult> CreateEntryCommentVote(Guid entryCommentId, VoteType voteType = VoteType.UpVote)
        {
            var result = await mediator.Send(new CreateEntryCommentVoteCommand(entryCommentId, voteType, UserId));

            return Ok(result);
        }


        [HttpDelete]
        [Route("DeleteEntryVote/{entryId}")]
        public async Task<IActionResult> DeleteEntryVote(Guid entryId)
        {
            await mediator.Send(new DeleteEntryVoteCommand(entryId, UserId));

            return Ok();
        }
    }
}
