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

namespace SmartHomeApi
{
    public static class snapshot
    {
        [FunctionName("snapshot")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req, ILogger log)
        {


            int[] locations = await DistinctLocationsAsync(60); //queries which locations have data within the last 60 minutes
            JArray ToReturn = new JArray();
            foreach (int location in locations)
            {
                SingleValueReading svr_temp = await GetLatestSingleValueReadingAsync(location, 0); //read temperature
                SingleValueReading svr_humidity = await GetLatestSingleValueReadingAsync(location, 1); //read humidity
                JObject jo = new JObject();
                jo.Add("location", location);
                if (svr_temp != null)
                {
                    jo.Add("temperature", svr_temp.Value);
                    jo.Add("temperatureCollected", svr_temp.CollectedAtUtc);
                }
                else
                {
                    jo.Add("temperature", null);
                    jo.Add("temperatureCollected", null);
                }
                if (svr_humidity != null)
                {
                    jo.Add("humidity", svr_humidity.Value);
                    jo.Add("humidityCollected", svr_humidity.CollectedAtUtc);
                }
                else
                {
                    jo.Add("humidity", null);
                    jo.Add("humidityCollected", null);
                }
                ToReturn.Add(jo);
            }
            

            HttpResponseMessage resp = new HttpResponseMessage();
            resp.StatusCode = HttpStatusCode.OK;
            resp.Content = new StringContent(ToReturn.ToString(), Encoding.UTF8, "application/json");
            return resp;
        }  




        public static async Task<int[]> DistinctLocationsAsync(int within_minutes)
        {
            string cmd = "select distinct Location from SingleValueReading where datediff(minute, CollectedAtUtc, getdate()) < " + within_minutes.ToString();
            JArray q = await ExecuteQueryAsync(cmd);
            List<int> ToReturn = new List<int>();
            foreach (JObject jo in q)
            {
                JProperty prop = jo.Property("Location");
                if (prop != null)
                {
                    int loc = Convert.ToInt32(prop.Value.ToString());
                    ToReturn.Add(loc);
                }
            }
            return ToReturn.ToArray();
        }

        public static async Task<SingleValueReading> GetLatestSingleValueReadingAsync(int location, int reading_type)
        {
            string cmd = "select top 1 * from SingleValueReading where Location = " + location.ToString() + " and ReadingType = " + reading_type.ToString() + "order by CollectedAtUtc desc";
            JArray q = await ExecuteQueryAsync(cmd);
            foreach (JObject jo in q)
            {
                try
                {
                    SingleValueReading ToReturn = SingleValueReading.Deserialize(jo);
                    return ToReturn;
                }
                catch
                {

                }
            }
            return null;
        }

        public static async Task<JArray> ExecuteQueryAsync(string query)
        {
            SqlConnection sqlcon = new SqlConnection("(SQL connection string here)");
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand(query, sqlcon);
            SqlDataReader dr = await sqlcmd.ExecuteReaderAsync();
            string json = SqlToolkit.ReadSqlToJson(dr);
            sqlcon.Close();
            JArray ja = JArray.Parse(json);
            return ja;
        }


    }
}
