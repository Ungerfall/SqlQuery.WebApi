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
                    var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Northwind"];
                    s.AddTransient<IDbConnection>(_ => new SqlConnection(connectionString.ConnectionString));
                })
                .Build();

            host.Run();
        }
    }
}