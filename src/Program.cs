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
            string connectionString = Environment.GetEnvironmentVariable(
                $"ConnectionStrings:Northwind",
                EnvironmentVariableTarget.Process);
            return new SqlConnection(connectionString);
        }
    }
}