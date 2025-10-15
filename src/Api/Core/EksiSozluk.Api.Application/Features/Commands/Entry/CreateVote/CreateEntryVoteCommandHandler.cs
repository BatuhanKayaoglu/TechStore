using EksiSozluk.Common.Events.Entry;
using EksiSozluk.Common.Infrastructure;
using EksiSozluk.Common;
using EksiSozluk.Common.ViewModels.RequestModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Api.Application.Features.Commands.Entry.CreateVote
{
    public class CreateEntryVoteCommandHandler : IRequestHandler<CreateEntryVoteCommand, bool>
    {
        public Task<bool> Handle(CreateEntryVoteCommand request, CancellationToken cancellationToken)
        {
            QueueFactory.SendMessageToExchange(exchangeName: SozlukConstants.VoteExchangeName,
                exchangeType: SozlukConstants.DefaultExchangeType,
                queueName: SozlukConstants.CreateEntryVoteQueueName,
                obj: new CreateEntryVoteEvent()
                {
                    EntryId = request.EntryId,
                    CreatedBy = request.CreatedBy,
                    VoteType = request.VoteType
                });

            return Task.FromResult(true);
        }
    }
}
