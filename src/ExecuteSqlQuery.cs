using System.Data;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace SqlQuery.WebApi;
public class ExecuteSqlQuery
{
    private readonly ILogger _logger;
    private readonly IDbConnection _dbConnection;

    public ExecuteSqlQuery(ILoggerFactory loggerFactory, IDbConnection dbConnection)
    {
        _logger = loggerFactory.CreateLogger<ParseSqlQuery>();
        _dbConnection = dbConnection;
    }

    [Function("ExecuteSqlQuery")]
    public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        string? query = req.ReadAsString(System.Text.Encoding.UTF8);
        if (string.IsNullOrEmpty(query))
        {
            return req.ReturnOk();
        }

        try
        {
            _dbConnection.Open();
            var csv = QueryAsCsv(query);
            return req.ReturnOk(csv);
        }
        catch (SqlException e)
        {
            return req.ReturnBad($"Execution error. Details: {e.Message}");
        }
    }

    // TODO: make pretty table with indentations etc.
    private string QueryAsCsv(string query)
    {
        using var command = _dbConnection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandText = query;
        var reader = command.ExecuteReader();

        return reader.ToCsv();
    }
}
