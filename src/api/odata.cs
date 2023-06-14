using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;
using TimHanewich.OData;
using Microsoft.AspNetCore.Mvc.WebApiCompatShim;
using System.Data.SqlClient;
using TimHanewich.Sql; 

namespace SmartHomeApi
{
    public static class odata
    {
        [FunctionName("odata")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "odata/{table}")] HttpRequest req, ILogger log, string table)
        {
            //Convert HttpRequest to HttpRequestMessage
            HttpRequestMessageFeature util = new HttpRequestMessageFeature(req.HttpContext);
            HttpRequestMessage reqmsg = util.HttpRequestMessage;

            ODataOperation op = ODataOperation.Parse(reqmsg);

            if (op.Operation == DataOperation.Create)
            {
                string sqlcmd = op.ToSql(); //Creates the "INSERT INTO ..." SQL query
                SqlConnection sqlcon = new SqlConnection(ConnectionStringProvider.SqlConnectionString);
                sqlcon.Open();
                SqlCommand cmd = new SqlCommand(sqlcmd, sqlcon);

                //Execute (upload)
                await cmd.ExecuteNonQueryAsync();

                //Close the connection
                sqlcon.Close();

                //Respond w/ 201 CREATED (success)
                HttpResponseMessage resp = new HttpResponseMessage();
                resp.StatusCode = HttpStatusCode.Created;
                return resp;
            }

            //Return bad request if it was not a create operation
            HttpResponseMessage bresp = new HttpResponseMessage();
            bresp.StatusCode = HttpStatusCode.BadRequest;
            return bresp;
        }
    }
}