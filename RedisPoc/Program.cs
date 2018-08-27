using Newtonsoft.Json;
using RedisPoc.Business;
using RedisPoc.poco;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MessagePack;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Threading.Tasks;

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

            var u = new List<string>();
            //Check speed of geting hash data
            for (int i = 0; i < 10; i++)
            {
                var junk = dataGen.RandomHashData(2000);
                logic.SetHashData("junk", junk);

                timer.Restart();
                u = logic.GetHashValues("junk");
                timer.Stop();

                Console.WriteLine($"Getting {u.Count} hash values took {timer.ElapsedMilliseconds}ms.");
            }

            timer.Restart();
            var q = logic.GetHashKeys("junk");
            timer.Stop();

            Console.WriteLine($"Getting {q.Count} hash keys took {timer.ElapsedMilliseconds}ms.");

            timer.Restart();
            var e = logic.GetSingleHashValue("junk", q.First());
            timer.Stop();
            Console.WriteLine($"Getting specific hash value took {timer.ElapsedMilliseconds}ms.");

            saveData.Clear();

            //Testing pipelinging speed
            for (int i = 0; i < 1; i++)
            {
                data = dataGen.GetRandomData(2000);
                saveData.AddRange((from d in data
                                   select JsonConvert.SerializeObject(d)).ToList());
            }

            timer.Restart();
            logic.SetListData("pipe", saveData.GetRange(0, 3));
            timer.Stop();

            Console.WriteLine($"Adding {saveData.Count} rows took {timer.ElapsedMilliseconds}ms.");
            logic.SetListPipeline("reg", saveData);
            logic.SetListPipelineAsParallel("para", saveData);

            var reg = new List<long>();
            var asParallels = new List<long>();
            for (int i = 0; i < 10; i++)
            {
                timer.Restart();
                logic.SetListPipeline("reg", saveData);
                timer.Stop();
                reg.Add(timer.ElapsedMilliseconds);

                timer.Restart();
                logic.SetListPipelineAsParallel("para", saveData);
                timer.Stop();
                asParallels.Add(timer.ElapsedMilliseconds);
            }

            Console.WriteLine($"Adding {saveData.Count} rows regular took {reg.Average()}ms.");
            Console.WriteLine($"Adding {saveData.Count} rows asParallel took {asParallels.Average()}ms.");

            timer.Restart();
            var w = logic.GetListData("pipe");
            timer.Stop();

            Console.WriteLine($"Getting {w.Count} rows pipelined took {timer.ElapsedMilliseconds}ms.");

            var seraldata = new List<PersonData>();
            Parallel.For(0,100, i => {
                seraldata.AddRange(dataGen.RandomPersonData(2000));
            });

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            byte[] Array;
            bf.Serialize(ms, seraldata);
            Array = ms.ToArray();
            int startSize = Array.Length;

            var l4Time = new List<long>();
            var l4Size = new List<long>();
            var msgTime = new List<long>();
            var msgSize = new List<long>();

            for (int i = 0; i < 100; i++)
            {
                timer.Restart();
                var lv4 = LZ4MessagePackSerializer.Serialize(seraldata);
                timer.Stop();
                l4Time.Add(timer.ElapsedMilliseconds);
                l4Size.Add(lv4.LongLength);

                timer.Restart();
                var megPak = MessagePackSerializer.Serialize(seraldata);
                timer.Stop();
                msgTime.Add(timer.ElapsedMilliseconds);
                msgSize.Add(megPak.LongLength);
            }

            Console.WriteLine($"Lz4 serialization took {l4Time.Average()}ms and a size of {(l4Size.Average() / 1024) / 1024} mb");
            Console.WriteLine($"Messagepack serialization took {msgTime.Average()}ms and a size of {(msgSize.Average() / 1024) / 1024} mb");
            Console.WriteLine($"lz4 took { msgTime.Average() - l4Time.Average()} less time.");
            Console.WriteLine($"Lz4 is {((msgSize.Average() - l4Size.Average()) / 1024) / 1024} smaller.");
            Console.WriteLine($"Lz4 shunk in size by {(startSize / l4Size.Average()) * 100} %");
            Console.WriteLine($"MsgPack shunk in size by {(startSize / msgSize.Average()) * 100} %");

            timer.Restart();
            //cleanup
            //logic.DeleteData("basicCall");
            //logic.DeleteData("people");
            //logic.DeleteData("junk");
            //logic.DeleteData("pipe");
            //logic.DeleteData("para");
            //logic.DeleteData("reg");
            timer.Stop();
            Console.WriteLine($"Clean up took {timer.ElapsedMilliseconds}ms.");
            Console.WriteLine($"Press enter to exit.");
            var t = Console.ReadLine();
        }
    }
}

