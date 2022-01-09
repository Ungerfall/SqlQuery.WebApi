using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace src
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(s =>
                {
                    s.AddTransient<IDbConnection>(DbConnectionFactoryMethod);
                })
                .Build();

            host.Run();
        }

        public static IDbConnection DbConnectionFactoryMethod(IServiceProvider _)
        {
            string? connectionString = GetSqlAzureConnectionString("Default");
            return new SqlConnection(connectionString);
        }

        public static string? GetSqlAzureConnectionString(string name)
        {
            string? conStr = Environment.GetEnvironmentVariable($"ConnectionStrings:{name}", EnvironmentVariableTarget.Process);
            if (string.IsNullOrEmpty(conStr)) // Azure Functions App Service naming convention
                conStr = Environment.GetEnvironmentVariable($"SQLAZURECONNSTR_{name}", EnvironmentVariableTarget.Process);
            return conStr;
        }
    }
}