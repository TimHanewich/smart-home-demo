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
    public static class summary
    {
        [FunctionName("summary")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        {

            //Get parameter 'hours'. Default is 24.
            int _hours = 24;
            string hours = req.Query["hours"];            
            if (hours != null)
            {
                _hours = Convert.ToInt32(hours);
            }

            int[] locations = await SmartHomeToolkit.DistinctLocationsAsync(60 * _hours);
            JArray ToReturn = new JArray();
            foreach (int location in locations)
            {
                //Get temperature avg
                float temperature = await SmartHomeToolkit.GetAverageAsync(location, 0, DateTime.UtcNow.AddHours(_hours * -1));
                float humidity = await SmartHomeToolkit.GetAverageAsync(location, 1, DateTime.UtcNow.AddHours(_hours * -1));

                //Add data to response payload
                JObject ToAdd = new JObject();
                ToAdd.Add("location", location);
                ToAdd.Add("temperature", temperature);
                ToAdd.Add("humidity", humidity);
                ToReturn.Add(ToAdd);
            }

            //Return
            HttpResponseMessage resp = new HttpResponseMessage();
            resp.StatusCode = HttpStatusCode.OK;
            resp.Content = new StringContent(ToReturn.ToString(), Encoding.UTF8, "application/json");
            return resp;
        }
    }
}