using EksiSozluk.Common;
using EksiSozluk.Common.Events.Entry;
using EksiSozluk.Common.Events.EntryComment;
using EksiSozluk.Common.Infrastructure;

namespace EksiSozluk.Projections.VoteService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration configuration;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            this.configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var connStr = configuration.GetConnectionString("SqlServer");
            var voteService = new Services.VoteService(connStr);


            // CreateEntryVote      
            QueueFactory.CreateBasicConsumer()
                .EnsureExchange(SozlukConstants.VoteExchangeName)
                .EnsureQueue(SozlukConstants.CreateEntryVoteQueueName, SozlukConstants.VoteExchangeName)
                .Receive<CreateEntryVoteEvent>(async (vote) =>
                {
                    voteService.CreateEntryVote(vote).GetAwaiter().GetResult();
                    _logger.LogInformation($"Create Entry Received EntryId:  {vote.EntryId}, VoteType:  {vote.VoteType}");
                })
                .StartingConsuming(SozlukConstants.CreateEntryVoteQueueName);


            // DeleteEntryVote  
            QueueFactory.CreateBasicConsumer()
               .EnsureExchange(SozlukConstants.VoteExchangeName)
               .EnsureQueue(SozlukConstants.DeleteEntryVoteQueueName, SozlukConstants.VoteExchangeName)
               .Receive<DeleteEntryVoteEvent>(async (vote) =>
               {
                   voteService.DeleteEntryVote(vote.EntryId, vote.CreatedBy).GetAwaiter().GetResult();
                   _logger.LogInformation($"Delete EntryCommentVote Received EntryId:  {vote.EntryId}");
               })
               .StartingConsuming(SozlukConstants.DeleteEntryVoteQueueName);


            // CreateEntryCommentVote
            QueueFactory.CreateBasicConsumer()
                .EnsureExchange(SozlukConstants.VoteExchangeName)
                .EnsureQueue(SozlukConstants.CreateEntryCommentVoteQueueName, SozlukConstants.VoteExchangeName)
                .Receive<CreateEntryCommentVoteEvent>(async (vote) =>
                {
                    voteService.CreateEntryCommentVote(vote).GetAwaiter().GetResult();
                    _logger.LogInformation($"Create EntryCommentVote Received EntryId:  {vote.EntryCommentId}, VoteType:  {vote.VoteType}");
                })
                .StartingConsuming(SozlukConstants.CreateEntryCommentVoteQueueName);


            // DeleteEntryCommentVote   
            QueueFactory.CreateBasicConsumer()
              .EnsureExchange(SozlukConstants.VoteExchangeName)
              .EnsureQueue(SozlukConstants.DeleteEntryCommentVoteQueueName, SozlukConstants.VoteExchangeName)
              .Receive<DeleteEntryCommentVoteEvent>(async (vote) =>
              {
                  voteService.DeleteEntryCommentVote(vote.EntryCommentId, vote.CreatedBy).GetAwaiter().GetResult();
                  _logger.LogInformation($"Delete EntryCommentVote Received EntryId:  {vote.EntryCommentId}");
              })
              .StartingConsuming(SozlukConstants.DeleteEntryCommentVoteQueueName);


        }



    }
}