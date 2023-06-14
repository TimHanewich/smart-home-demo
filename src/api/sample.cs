using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Text.Encodings;
using System.Text;
using System.Collections.Generic;
using System.Data.SqlClient;
using TimHanewich.Sql;
using SmartHomeCore;

namespace SmartHomeApi
{
    public static class sample
    {
        [FunctionName("sample")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            //Get the response body
            StreamReader sr = new StreamReader(req.Body);
            string content = await sr.ReadToEndAsync();

            //If content is empty
            if (content == "" || content == "[]")
            {
                HttpResponseMessage failed = new HttpResponseMessage();
                failed.StatusCode = HttpStatusCode.BadRequest;
                failed.Content = new StringContent("You must provide a JSON object (not array)");
                return failed;
            }

            //Parse
            JObject jo = JObject.Parse(content);

            //Locations property provided?
            JProperty prop_locations = jo.Property("locations");
            if (prop_locations != null)
            {
                int[] locations = JsonConvert.DeserializeObject<int[]>(prop_locations.Value.ToString());
                
                //If locations were empty, insert default ones
                if (locations.Length == 0)
                {
                    locations = new int[]{4, 5, 7}; // 4 = living room, 5 = master bedroom, 7 = garage
                }

                foreach (int location in locations)
                {

                    //Temperature
                    SingleValueReading svrt = new SingleValueReading();
                    svrt.Id = Guid.NewGuid();
                    svrt.Location = location;
                    svrt.CollectedAtUtc = DateTime.UtcNow;
                    svrt.ReadingType = 0;
                    svrt.Value = SmartHomeToolkit.RandomFloat(68.0f, 76.0f);
                    
                    //Humidity
                    SingleValueReading svrh = new SingleValueReading();
                    svrh.Id = Guid.NewGuid();
                    svrh.Location = location;
                    svrh.CollectedAtUtc = DateTime.UtcNow;
                    svrh.ReadingType = 1;
                    svrh.Value = SmartHomeToolkit.RandomFloat(44.0f, 59.0f);

                    //Upload both
                    await SmartHomeToolkit.UploadSingleValueReading(svrt);
                    await SmartHomeToolkit.UploadSingleValueReading(svrh);
                }

                //Return success
                HttpResponseMessage s = new HttpResponseMessage();
                s.StatusCode = HttpStatusCode.Created;
                return s;
            }
            else
            {
                HttpResponseMessage failed = new HttpResponseMessage();
                failed.StatusCode = HttpStatusCode.BadRequest;
                failed.Content = new StringContent("You must provide property 'locations' (array of integers) in your request body.");
                return failed;
            }
        }
    }
}