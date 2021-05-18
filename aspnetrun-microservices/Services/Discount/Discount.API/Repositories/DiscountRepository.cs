using Dapper;
using Discount.API.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Threading.Tasks;

namespace Discount.API.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration configuration;

        public DiscountRepository(IConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            using var connection = new NpgsqlConnection
                (configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var query = "SELECT * FROM Coupons WHERE ProductName = @ProductName";

            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>(query, new { ProductName = productName });

            if(coupon == null)
            {
                return new Coupon { ProductName = "No discount", Amount = 0, Description = "No discount" };
            }

            return coupon;
        }

        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection
                (configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var query = "INSERT INTO Coupons (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)";

            var affected = await connection.ExecuteAsync(query, new
            {
                coupon.ProductName,
                coupon.Description,
                coupon.Amount
            });
            return affected != 0;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            using var connection = new NpgsqlConnection
                (configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var query = "DELETE FROM Coupons WHERE ProductName = @ProductName";
            var affected = await connection.ExecuteAsync(query, new { ProductName = productName });
            return affected != 0;
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection
                (configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var query = @"UPDATE Coupons SET 
                           ProductName = @ProductName,
                           Description = @Description,
                           Amount = @Amount
                          WHERE Id = @Id";

            var affected = await connection.ExecuteAsync(query, new
            {
                coupon.Id,
                coupon.ProductName,
                coupon.Description,
                coupon.Amount
            });
            return affected != 0;
        }
    }

    public interface IDiscountRepository
    {
        Task<Coupon> GetDiscount(string productName);
        Task<bool> CreateDiscount(Coupon coupon);
        Task<bool> UpdateDiscount(Coupon coupon);
        Task<bool> DeleteDiscount(string productName);
    }
}
