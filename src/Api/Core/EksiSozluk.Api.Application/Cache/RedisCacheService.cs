using EksiSozluk.Api.Application.Repositories;
using EksiSozluk.Common.ViewModels.Queries;
using EksİSozluk.Domain.Models;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Pipelines.Sockets.Unofficial;
using System.Text.Json.Serialization;

namespace EksiSozluk.Api.Application.Cache
{
    public class RedisCacheService : IRedisCacheService
    {
        readonly IConfiguration configuration;
        private readonly IUserRepository userRepository;
        private readonly IDistributedCache distributedCache;
        private readonly Lazy<ConnectionMultiplexer> redisConn;
        // ConnectionMultiplexer sınıfı, Redis sunucusuna bağlanmak için kullanılır. IDistributedCache methodlarıyla yapamadıgımız bazı işlemler için bu sınıf üzerinden işlemlerimizi yapıyoruz.    
        // Lazy<T> sınıfı, bir nesnenin oluşturulması gerektiğinde oluşturulmasını sağlar. Bu nesne oluşturulana kadar bellekte yer kaplamaz.   

        public RedisCacheService(IConfiguration configuration, IUserRepository userRepository, IDistributedCache distributedCache)
        {
            this.configuration = configuration;
            this.userRepository = userRepository;
            this.distributedCache = distributedCache;

            var redisConnectionString = configuration.GetSection("Redis").Value;
            redisConn = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(redisConnectionString));
        }
        private ConnectionMultiplexer Connection => redisConn.Value;

        private static RedisKey UserKeyPrefix = Encoding.UTF8.GetBytes("user:");
        private static RedisKey GetUserKey(string userId) => UserKeyPrefix.Append(userId);

        public async Task<User> GetByIdAsync(Guid key, CancellationToken cancellationToken)
        {
            var user = await distributedCache.GetStringAsync(GetUserKey(key.ToString()));
            //var keyData = $"user:{key}";
            //var user = await distributedCache.GetStringAsync(keyData);

            // Get the data from Redis  
            if (user is not null)
            {
                User? userData = JsonSerializer.Deserialize(user, UserSerializationContext.Default.User);
                return userData;
            }
            return null;
        }

        public async Task DeleteAsync(Guid key, CancellationToken cancellationToken)
        {
            bool control = await UserExistsAsync(key, cancellationToken);
            if (!control)
                throw new Exception("User not found in cache!");

            var keyData = $"{key.GetType().ToString()}:{key}";
            distributedCache.Remove(keyData);
            await Task.CompletedTask;
        }

        public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken)
        {
            // IDistributedCache interface'i içersinde Key bilgisine göre tüm verileri getirme işlemi yapılamadığı için alttaki db bağlantısı ile tüm verileri getirme işlemi yapılmıştır.      
            IDatabase db = Connection.GetDatabase();
            var server = Connection.GetServer(Connection.GetEndPoints().First());

            RedisKey[] keys = server.Keys(pattern: "user:*").ToArray(); // if have "user" key, you can use pattern: "user:*"  
            List<User> users = new List<User>();

            foreach (RedisKey keyData in keys)
            {
                HashEntry[] hashEntries = await db.HashGetAllAsync(keyData);

                RedisValue userData = hashEntries[1].Value;
                var serializedUser = JsonSerializer.Deserialize<User>(userData);

                if (hashEntries.Length > 0)
                    users.Add(serializedUser);
            }

            return users;
        }


        public async Task SetAsync(User user, CancellationToken cancellationToken)
        {
            bool control = await UserExistsAsync(user.Id, cancellationToken);
            if (control)
                throw new Exception("User found in cache!");

            //await GetAllAsync(cancellationToken);

            var key = $"user:{user.Id}";
            await distributedCache.SetStringAsync(key, JsonSerializer.Serialize(user));
        }

        public async Task UpdatedAsync(User user, CancellationToken cancellationToken)
        {
            var control = await UserExistsAsync(user.Id, cancellationToken);
            if (control)
                throw new Exception("User not found in cache!");

            var key = GetUserKey(user.Id.ToString());
            distributedCache.Remove(key);

            distributedCache.SetString(key, JsonSerializer.Serialize(user));
            await Task.CompletedTask;
        }

        public async Task<bool> UserExistsAsync(Guid key, CancellationToken cancellationToken)
        {
            var user = await distributedCache.GetStringAsync(GetUserKey(key.ToString()));
            if (user == null)
                return false;
            return true;
        }
    }
}

[JsonSerializable(typeof(User))]
[JsonSourceGenerationOptions(PropertyNameCaseInsensitive = true)]
public partial class UserSerializationContext : JsonSerializerContext
{
}
