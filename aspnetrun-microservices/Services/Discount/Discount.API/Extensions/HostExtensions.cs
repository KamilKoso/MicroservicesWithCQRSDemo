using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Data.SqlClient;
using System.Threading;

namespace Discount.API.Extensions
{
    public static class HostExtensions
    {

        public static IHost MigrateDatabase<TContext>(this IHost host, int? retry = 0)
        {
            using (var scope = host.Services.CreateScope())
            {
                var servicesProvider = scope.ServiceProvider;
                var configuration = servicesProvider.GetRequiredService<IConfiguration>();
                var logger = servicesProvider.GetRequiredService<ILogger<TContext>>();

                try
                {
                    using var connection = new NpgsqlConnection
                          (configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
                    connection.Open();
                    var couponsTableExists = connection
                        .QueryFirstOrDefault<int>(@"select case when exists(
                                            (select * from information_schema.tables where table_name = 'coupons')
                                           ) then 1 else 0 end") == 1;

                    if(couponsTableExists)
                    {
                        logger.LogInformation("Coupons table exists. Migration not needed");
                        return host;
                    }

                    logger.LogInformation("Migrating postgresql database");
                    string migrationSql = @"CREATE TABLE Coupons(Id SERIAL PRIMARY KEY,
                                                                 ProductName VARCHAR(24) NOT NULL,
                                                                 Description TEXT,
                                                                 Amount INT);";
                    migrationSql += "INSERT INTO Coupons(ProductName, Description, Amount) VALUES('IPhone X', 'IPhone Discount', 150);";
                    migrationSql += "INSERT INTO Coupons(ProductName, Description, Amount) VALUES('Samsung 10', 'Samsung Discount', 100);";
                    connection.Execute(migrationSql);
                    logger.LogInformation("Migrated postgresql database");
                }
                catch (SqlException ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the postgresql database");
                    if (retry < 50)
                    {
                        Thread.Sleep(2000);
                        MigrateDatabase<TContext>(host, ++retry);
                    }
                }
            }
           return host;
        }
    }
}
