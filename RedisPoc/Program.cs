﻿using Newtonsoft.Json;
using RedisPoc.Business;
using System;
using System.Diagnostics;
using System.Linq;

namespace RedisPoc
{
    class Program
    {
        static void Main(string[] args)
        {
            var timer = new Stopwatch();
            var logic = new RedisLogic();

            logic.SetStringData("basicCall", "Hello world");

            var x = logic.GetData<string>("basicCall");

            Console.WriteLine(x);

            x = logic.GetData<string>("basicCall");

            Console.WriteLine(x);

            var y = logic.GetAllKeys();

            Console.WriteLine(string.Join(", ", y));

            var dataGen = new RandomData();

            var data = dataGen.GetRandomData(10);
            var saveData = (from d in data
                            select JsonConvert.SerializeObject(d)).ToList();

            timer.Start();
            logic.SetListData("people", saveData);
            timer.Stop();
            Console.WriteLine($"Adding {saveData.Count} rows took {timer.ElapsedMilliseconds}ms.");

            timer.Restart();
            var z = logic.GetListData("people");
            timer.Stop();

            Console.WriteLine($"Getting {z.Count} rows took {timer.ElapsedMilliseconds}ms.");

            //Check speed of geting list data
            for (int i = 0; i < 10; i++)
            {
                data = dataGen.GetRandomData(2000);
                saveData = (from d in data
                            select JsonConvert.SerializeObject(d)).ToList();

                logic.SetListData("people", saveData);

                timer.Restart();
                z = logic.GetListData("people");
                timer.Stop();

                Console.WriteLine($"Getting {z.Count} rows took {timer.ElapsedMilliseconds}ms.");
            }

            //Check speed of geting hash data
            for (int i = 0; i < 10; i++)
            {
                var junk = dataGen.RandomHashData(2000);
                logic.SetHashData("junk", junk);

                timer.Restart();
                var w = logic.GetHashValues("junk");
                timer.Stop();

                Console.WriteLine($"Getting {w.Count} hash values took {timer.ElapsedMilliseconds}ms.");
            }

            timer.Restart();
            var q = logic.GetHashKeys("junk");
            timer.Stop();

            Console.WriteLine($"Getting {q.Count} hash keys took {timer.ElapsedMilliseconds}ms.");

            timer.Restart();
            var e = logic.GetSingleHashValue("junk", q.First());
            timer.Stop();
            Console.WriteLine($"Getting specific hash value took {timer.ElapsedMilliseconds}ms.");

            timer.Restart();
            //cleanup
            logic.DeleteData("basicCall");
            logic.DeleteData("people");
            logic.DeleteData("junk");
            timer.Stop();
            Console.WriteLine($"Clean up took {timer.ElapsedMilliseconds}ms.");
            Console.WriteLine($"Press enter to exit.");
            var t = Console.ReadLine();
        }
    }
}

