using System.Data;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace SqlQuery.WebApi
{
    public class ParseSqlQuery
    {
        private readonly ILogger _logger;
        private readonly IDbConnection _dbConnection;

        public ParseSqlQuery(ILoggerFactory loggerFactory, IDbConnection dbConnection)
        {
            _logger = loggerFactory.CreateLogger<ParseSqlQuery>();
            _dbConnection = dbConnection;
        }

        [Function("ParseSqlQuery")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var query = req.ReadAsString(System.Text.Encoding.UTF8);
            using var command = _dbConnection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "SET NOEXEC ON; " + query;
            command.Connection.Open();
            try
            {
                command.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                var bad = req.CreateResponse(HttpStatusCode.BadRequest);
                bad.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                bad.WriteString(e.Message);

                return bad;
            }

            var ok = req.CreateResponse(HttpStatusCode.OK);
            ok.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            ok.WriteString("Valid query.");
            return ok;
        }
    }
}
