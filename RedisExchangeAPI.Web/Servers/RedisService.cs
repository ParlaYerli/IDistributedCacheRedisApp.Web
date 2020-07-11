using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchangeAPI.Web.Servers
{
    public class RedisService
    {
        private readonly string _redisHost;
        private readonly string _redisPort;
        private IConnectionMultiplexer _redis; //redis server ile haberleşmeyi sağlar.
        public IDatabase db { get; set; }
        public RedisService(IConfiguration _configuration)
        {
            _redisHost = _configuration["Redis:Host"];
            _redisPort = _configuration["Redis:Port"];
        }

        // proje ayağa kalktığında redis server ile iletişime geçecek.
        public void Connect()
        {
            var configString = $"{_redisHost}:{_redisPort}";
            _redis = ConnectionMultiplexer.Connect(configString);
        }

        public IDatabase GetDB(int db) //  Redis içerisinde hazır sıralı dbler mevcut. Bu databaselerden istediğimi seçmek için bu metodu kullanacağım.
        {
            return _redis.GetDatabase(db);
        }
    }
}
