using RedisPoc.Business;
using System;

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
        }
    }
}

