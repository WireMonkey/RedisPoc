using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace RedisPoc.Business
{
    public class RedisLogic
    {
        private RedisManagerPool manager { get; set; }

        public RedisLogic()
        {
            manager = new RedisManagerPool("ec2-18-222-58-156.us-east-2.compute.amazonaws.com");
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

        public List<string> GetListData(string key)
        {
            using (var client = manager.GetClient())
            {
                return client.GetAllItemsFromList(key);
            }
        }

        public bool SetStringData(string key, string data)
        {
            using (var client = manager.GetClient())
            {
                client.Add<string>(key, data);

                return true;
            }
        }

        public bool DeleteData(string key)
        {
            using (var clinet = manager.GetClient())
            {
                clinet.Remove(key);

                return true;
            }
        }

        public List<string> GetAllKeys()
        {
            using (var client = manager.GetClient())
            {
                return client.GetAllKeys();
            }
        }
    }
}
