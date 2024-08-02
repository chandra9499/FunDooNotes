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
        private IDatabase _cacheDb;
        public CacheDL()
        {
            var radis = ConnectionMultiplexer.Connect("localhost:6379");
            _cacheDb = radis.GetDatabase();
        }
        public T GetData<T>(string key)
        {
            var value = _cacheDb.StringGet(key);
            if (value.IsNullOrEmpty!)
            {
                return JsonSerializer.Deserialize<T>(value);
            }
            return default;//a nullable type of the object
        }

        public object RemoveData(string key)
        {
            var exist = _cacheDb.KeyExists(key);

            if (exist) 
            {
                return _cacheDb.KeyDelete(key);
            }
            return false;
        }

        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            var expieryTime = expirationTime - DateTimeOffset.UtcNow ;
            var isSet = _cacheDb.StringSet(key, JsonSerializer.Serialize(value),expieryTime);
            return isSet;
        }
    }
}
