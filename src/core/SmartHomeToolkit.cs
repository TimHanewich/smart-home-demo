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

        public static float RandomFloat(float min, float max)
        {
            Random r = new Random();
            float rValue = r.NextSingle();
            float ToReturn = min + ((max - min) * rValue);
            return ToReturn;
        }

        //Uploads (inserts) into SQL
        public static async Task UploadSingleValueReading(SingleValueReading svr)
        {
            UpstreamHelper uh = new UpstreamHelper("SingleValueReading");
            uh.Add("Id", svr.Id.ToString(), true);
            uh.Add("CollectedAtUtc", SqlToolkit.ToSqlDateTimeString(svr.CollectedAtUtc), true);
            uh.Add("Location", svr.Location.ToString());
            uh.Add("Value", svr.Value.ToString());
            uh.Add("ReadingType", svr.ReadingType.ToString());
            string sql_cmd = uh.ToInsert(); //i.e.: insert into SingleValueReading (Id,CollectedAtUtc,Location,Value,ReadingType) values ('67f781ae-1190-4bf3-a225-edcc24686634','20230614 13:35:48.360',4,72.154236,0)
            SqlConnection sqlcon = new SqlConnection(ConnectionStringProvider.SqlConnectionString);
            sqlcon.Open();
            SqlCommand sqlcmd = new SqlCommand(sql_cmd, sqlcon);
            await sqlcmd.ExecuteNonQueryAsync();
            sqlcon.Close();
        }

    }
}