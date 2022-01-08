using System.Data;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace SqlQuery.WebApi;
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

        string? query = req.ReadAsString(System.Text.Encoding.UTF8);
        if (string.IsNullOrEmpty(query))
        {
            return req.ReturnOk();
        }

        try
        {
            _dbConnection.Open();
            SetNoExec();
            Query(query);
        }
        catch (SqlException e)
        {
            return req.ReturnBad($"Invalid Query. Details: {e.Message}");
        }

        return req.ReturnOk();
    }

    private void SetNoExec()
    {
        using var command = _dbConnection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandText = SqlStatements.SET.NOEXEC_ON;
        command.ExecuteNonQuery();
    }

    private void Query(string query)
    {
        using var command = _dbConnection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandText = query;
        command.ExecuteNonQuery();
    }
}
