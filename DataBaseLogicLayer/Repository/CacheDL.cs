using DataBaseLayer.Interface;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataBaseLayer.Repository
{
    public class CacheDL : ICacheDL
    {
        private readonly IDatabase _cacheDb;

        public CacheDL(IConnectionMultiplexer connectionMultiplexer)
        {
            _cacheDb = connectionMultiplexer.GetDatabase();
        }
        //public CacheDL()
        //{
        //    var radis = ConnectionMultiplexer.Connect("localhost:6379");
        //    _cacheDb = radis.GetDatabase();
        //}
        public T GetData<T>(string key)
        {
            var value = _cacheDb.StringGet(key);
            if (!value.IsNullOrEmpty)
            {
                return JsonSerializer.Deserialize<T>(value);
            }
            return default;//a nullable type of the object
        }

        public object RemoveData(string key)
        {
            return _cacheDb.KeyDelete(key);
        }

        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            var expiryTime = expirationTime - DateTimeOffset.UtcNow;

            // Ensure that expiryTime is non-negative
            if (expiryTime <= TimeSpan.Zero)
            {
                expiryTime = TimeSpan.FromMinutes(1); // Set to minimum expiry time of 1 second
            }

            var serializedValue = JsonSerializer.Serialize(value);
            return _cacheDb.StringSet(key, serializedValue, expiryTime);
        }
    }
}
