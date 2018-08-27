using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace RedisPoc.Business
{
    public class RedisLogic
    {
        private RedisManagerPool manager { get; set; }

        public RedisLogic()
        {
            manager = new RedisManagerPool("ec2-18-191-90-218.us-east-2.compute.amazonaws.com");
        }

        public T GetData<T>(string key)
        {
            using (var client = manager.GetClient())
            {
                var data = client.Get<T>(key);

                return data;
            }
        }

        public bool SetListData(string key, List<string> data)
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

        public bool SetHashData(string key, List<KeyValuePair<string, string>> data)
        {
            using (var client = manager.GetClient())
            {
                client.SetRangeInHash(key, data);

                return true;
            }
        }

        public List<string> GetHashKeys(string key)
        {
            using (var client = manager.GetClient())
            {
                return client.GetHashKeys(key);
            }
        }

        public List<string> GetHashValues(string key)
        {
            using (var client = manager.GetClient())
            {
                return client.GetHashValues(key);
            }
        }

        public string GetSingleHashValue(string key, string id)
        {
            using (var client = manager.GetClient())
            {
                return client.GetValueFromHash(key, id);
            }
        }

        public bool SetHashEntity<T>(string key, T data)
        {
            using (var client = manager.GetClient())
            {
                client.StoreAsHash<T>(data);

                return true;
            }
        }

        public bool SetListPipeline(string key, List<string> datas)
        {

            using (var client = manager.GetClient())
            {
                using (var pipe = client.CreatePipeline())
                {
                    foreach (var data in datas)
                    {
                        pipe.QueueCommand(x => x.AddItemToList(key, data));
                    }

                    pipe.Flush();
                }

            }

            return true;
        }

        public bool SetListPipelineAsParallel(string key, List<string> datas)
        {

            using (var client = manager.GetClient())
            {
                using (var pipe = client.CreatePipeline())
                {
                    foreach (var data in datas.AsParallel())
                    {
                        pipe.QueueCommand(x => x.AddItemToList(key, data));
                    }

                    pipe.Flush();
                }

            }

            return true;
        }
    }
}
