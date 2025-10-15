using EksiSozluk.Common;
using EksiSozluk.Common.Events.EntryComment;
using EksiSozluk.Common.Events.User;
using EksiSozluk.Common.Infrastructure;
using EksİSozluk.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Api.Application.Features.Commands.EntryComment.CreateFav
{
    public class CreateEntryCommentFavCommandHandler : IRequestHandler<CreateEntryCommentFavCommand, bool>
    {

        public async Task<bool> Handle(CreateEntryCommentFavCommand request, CancellationToken cancellationToken)
        {
            QueueFactory.SendMessageToExchange(exchangeName: SozlukConstants.FavExchangeName,
                exchangeType: SozlukConstants.DefaultExchangeType,
                queueName: SozlukConstants.CreateEntryCommentFavQueueName,
                obj: new CreateEntryCommentFavEvent()
                {
                    EntryCommentId = request.EntryCommentId,    
                    CreatedBy = request.UserId
                });

            return await Task.FromResult(true);

        }
    }
}
