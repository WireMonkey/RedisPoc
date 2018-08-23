﻿using Newtonsoft.Json;
using RedisPoc.Business;
using System;
using System.Collections.Generic;
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

            timer.Reset();
            timer.Start();
            var z = logic.GetListData("people");
            timer.Stop();

            Console.WriteLine($"Getting {z.Count} rows took {timer.ElapsedMilliseconds}ms.");

            //logic.DeleteData("people");

            for (int i = 0; i <= 3; i++)
            {
                data = dataGen.GetRandomData(2000);
                saveData = (from d in data
                            select JsonConvert.SerializeObject(d)).ToList();

                timer.Reset();
                timer.Start();
                logic.SetListData("people", saveData);
                timer.Stop();

                Console.WriteLine($"Adding {saveData.Count} rows took {timer.ElapsedMilliseconds}ms.");
            }

            timer.Reset();
            timer.Start();
            z = logic.GetListData("people");
            timer.Stop();

            Console.WriteLine($"Getting {z.Count} rows took {timer.ElapsedMilliseconds}ms.");


            for (int i = 0; i < 100; i++)
            {
                var junk = dataGen.RandomHashData(2000);
                logic.SetHashData("junk", junk);
                
                timer.Reset();
                timer.Start();
                var w = logic.GetHashValues("junk");
                timer.Stop();

                Console.WriteLine($"Getting {w.Count} hash values took {timer.ElapsedMilliseconds}ms.");
            }

            timer.Reset();
            timer.Start();
            var q = logic.GetHashKeys("junk");
            timer.Stop();

            Console.WriteLine($"Getting {q.Count} hash keys took {timer.ElapsedMilliseconds}ms.");

            logic.DeleteData("basicCall");
            logic.DeleteData("people");
            logic.DeleteData("junk");

            for (int i = 0; i < 100; i++)
            {
                data = dataGen.GetRandomData(2000);
                saveData = (from d in data
                            select JsonConvert.SerializeObject(d)).ToList();

                logic.SetListData("people", saveData);

                timer.Reset();
                timer.Start();
                z = logic.GetListData("people");
                timer.Stop();

                Console.WriteLine($"Getting {z.Count} rows took {timer.ElapsedMilliseconds}ms.");
            }

            logic.DeleteData("people");
        }
    }
}

