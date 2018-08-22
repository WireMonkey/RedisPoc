using ServiceStack.Redis;
using System;
using System.Collections.Generic;

namespace RedisPoc.Business
{
    public class RedisLogic
    {
        private RedisManagerPool manager { get; set; }

        public RedisLogic()
        {
            manager = new RedisManagerPool("");
        }

        public T GetData<T>(string key)
        {
            using (var client = manager.GetClient())
            {
                var data = client.Get<T>(key);

                return data;
            }
        }

        public bool SetListData(string key,List<string> data)
        {
            var x = new List<string>();
            using (var client = manager.GetClient())
            {
                client.AddRangeToList(key, data);

                return true;
            }
        }
    }
}
