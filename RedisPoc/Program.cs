using Newtonsoft.Json;
using RedisPoc.Business;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedisPoc
{
    class Program
    {
        static void Main(string[] args)
        {
            var logic = new RedisLogic();

            logic.SetStringData("basicCall", "Hello world");

            var x = logic.GetData<string>("basicCall");

            Console.WriteLine(x);

            logic.DeleteData("basicCall");

            x = logic.GetData<string>("basicCall");

            Console.WriteLine(x);

            var y = logic.GetAllKeys();

            Console.WriteLine(string.Join(", ",y));

            var dataGen = new RandomData();

            var data = dataGen.GetRandomData(10);
            var saveData = (from d in data
                            select JsonConvert.SerializeObject(d)).ToList();

            logic.SetListData("people", saveData);

            var z = logic.GetListData("people");

            Console.WriteLine(string.Join("/r/n", z));
        }
    }
}

