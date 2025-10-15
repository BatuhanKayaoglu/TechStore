using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Common
{
    public class SozlukConstants
    {
        public const string RabbitMQHost = "amqps://xvnbchgs:Xqd-yU1RoCvt3stDRk0cPyXzXIiqxJJT@moose.rmq.cloudamqp.com/xvnbchgs";
        //public const string RabbitMQHost = "amqp://guest:guest@localhost:5672/";
        public const string RabbitMQHostUri = "localhost";
        public const string DefaultExchangeType = "direct";


        public const string UserExchangeName = "UserExchange";
        public const string UserEmailChangedQueueName = "UserEmailChangedQueue";
        public const string UserPasswordChangedQueueName = "UserPasswordChangedQueue";


        public const string FavExchangeName = "FavExchange";
        public const string CreateEntryFavQueueName = "CreateEntryFavFavQueue";
        public const string CreateEntryCommentFavQueueName = "CreateEntryCommentFavQueue";
        public const string CreateEntryCommentVoteQueueName = "CreateEntryCommentVoteQueue";
        public const string DeleteEntryCommentFavQueueName = "DeleteEntryCommentFavQueue";
        public const string DeleteEntryFavQueueName = "DeleteEntryFavQueue";

        public const string VoteExchangeName= "VoteExchange";
        public const string CreateEntryVoteQueueName = "CreateEntryVoteQueue";
        public const string DeleteEntryVoteQueueName = "DeleteEntryVoteQueue";
        public const string DeleteEntryCommentVoteQueueName = "DeleteEntryCommentVoteQueue";

    }
}
