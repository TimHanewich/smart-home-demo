using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using TimHanewich.Sql;

namespace SmartHomeCore
{
    public class SmartHomeToolkit
    {
        public static async Task<int[]> DistinctLocationsAsync(int within_minutes)
        {
            string cmd = "select distinct Location from SingleValueReading where datediff(minute, CollectedAtUtc, getdate()) < " + within_minutes.ToString();
            JArray q = await ExecuteQueryAsync(cmd);
            List<int> ToReturn = new List<int>();
            foreach (JObject jo in q)
            {
                JProperty? prop = jo.Property("Location");
                if (prop != null)
                {
                    int loc = Convert.ToInt32(prop.Value.ToString());
                    ToReturn.Add(loc);
                }
            }
            return ToReturn.ToArray();
        }

        public static async Task<SingleValueReading?> GetLatestSingleValueReadingAsync(int location, int reading_type)
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
            SqlConnection sqlcon = new SqlConnection(ConnectionStringProvider.SqlConnectionString);
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