using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace RedisPoc.Business
{
    public class RandomData
    {
        public JArray GetRandomData(int rows)
        {
            var dataToGen = new
            {
                rowsToReturn = rows,
                columns = new List<object>
                {
                    new
                    {
                        columnName = "firstName",
                        function = "first",
                    },
                    new
                    {
                        columnName = "lastName",
                        function = "last",
                    },
                    new
                    {
                        columnName = "address",
                        function = "address",
                    },
                    new
                    {
                        columnName = "city",
                        function = "city",
                    },
                    new
                    {
                        columnName = "state",
                        function = "state",
                    },
                    new
                    {
                        columnName = "zip",
                        function = "zip",
                    },
                    new
                    {
                        columnName = "ssn",
                        function = "ssn",
                    },
                    new
                    {
                        columnName = "gender",
                        function = "gender",
                    },
                    new
                    {
                        columnName = "ssn",
                        function = "ssn",
                    },
                    new
                    {
                        columnName = "age",
                        function = "age"
                    },
                    new
                    {
                        columnName = "birthday",
                        function = "birthday"
                    }
                }
            };

            var client = new RestClient("https://lscx1zn8ib.execute-api.us-east-2.amazonaws.com/dev/randomData");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("undefined", JsonConvert.SerializeObject(dataToGen), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            return (JArray)JsonConvert.DeserializeObject(response.Content);
        }

        public List<KeyValuePair<string,string>> RandomHashData(int rows)
        {
            var dataToGen = new
            {
                rowsToReturn = rows,
                columns = new List<object>
                {
                    new
                    {
                    columnName = "key",
                    function = "guid"
                    },
                    new
                    {
                        columnName = "value",
                        function = "paragraph"
                    }
                }
            };

            var client = new RestClient("https://lscx1zn8ib.execute-api.us-east-2.amazonaws.com/dev/randomData");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("undefined", JsonConvert.SerializeObject(dataToGen), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            return JsonConvert.DeserializeObject<List<KeyValuePair<string, string>>>(response.Content);
        }
    }
}
