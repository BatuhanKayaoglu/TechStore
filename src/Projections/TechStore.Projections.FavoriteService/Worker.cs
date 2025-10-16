using TechStore.Common;
using TechStore.Common.Events.Entry;
using TechStore.Common.Events.EntryComment;
using TechStore.Common.Infrastructure;

namespace TechStore.Projections.FavoriteService
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
            var favService = new Services.FavoriteService(connStr);

            QueueFactory.CreateBasicConsumer()
                .EnsureExchange(SozlukConstants.FavExchangeName)
                .EnsureQueue(SozlukConstants.CreateEntryFavQueueName, SozlukConstants.FavExchangeName)
                .Receive<CreateEntryFavEvent>(async (fav) =>
                {
                    favService.CreateEntryFav(fav).GetAwaiter().GetResult();
                    _logger.LogInformation($"Received {fav.EntryId} from {fav.CreatedBy}");
                    // Burada gelen event'e g�re i�lem yap�lacak.
                    // �rne�in; EntryId'si 5 olan entry'e favori eklenmi�se, EntryId'si 5 olan entry'i favori olarak db'ye eklemi� olacag�z.       
                })
                .StartingConsuming(SozlukConstants.CreateEntryFavQueueName);


            QueueFactory.CreateBasicConsumer()
                .EnsureExchange(SozlukConstants.FavExchangeName)
                .EnsureQueue(SozlukConstants.DeleteEntryFavQueueName, SozlukConstants.FavExchangeName)
                .Receive<DeleteEntryFavEvent>(async (fav) =>
                {
                    favService.DeleteEntryFav(fav).GetAwaiter().GetResult();
                    _logger.LogInformation($"Deleted Received {fav.EntryId} from {fav.CreatedBy}");
                })
                .StartingConsuming(SozlukConstants.DeleteEntryFavQueueName);


            QueueFactory.CreateBasicConsumer()
                .EnsureExchange(SozlukConstants.FavExchangeName)
                .EnsureQueue(SozlukConstants.CreateEntryCommentFavQueueName, SozlukConstants.FavExchangeName)
                .Receive<CreateEntryCommentFavEvent>(async (fav) =>
                {
                    favService.CreateEntryCommentFav(fav).GetAwaiter().GetResult();
                    _logger.LogInformation($"Create Entry Comment Received EntryCommentId {fav.EntryCommentId}");
                    // Burada gelen event'e g�re i�lem yap�lacak.
                    // �rne�in; EntryId'si 5 olan entry'e favori eklenmi�se, EntryId'si 5 olan entry'i favori olarak db'ye eklemi� olacag�z.       
                })
                .StartingConsuming(SozlukConstants.CreateEntryFavQueueName);


            QueueFactory.CreateBasicConsumer()
                .EnsureExchange(SozlukConstants.FavExchangeName)
                .EnsureQueue(SozlukConstants.DeleteEntryCommentFavQueueName, SozlukConstants.FavExchangeName)
                .Receive<DeleteEntryCommentFavEvent>(async (fav) =>
                {
                    favService.DeleteEntryCommentFav(fav).GetAwaiter().GetResult();
                    _logger.LogInformation($"Deleted Entry Comment  Received EntryCommentId{fav.EntryCommentId}");
                })
                .StartingConsuming(SozlukConstants.DeleteEntryCommentFavQueueName);
        }
    }
}