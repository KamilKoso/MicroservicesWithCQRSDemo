using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
        {
            if(!orderContext.Orders.Any())
            {
                orderContext.AddRange(GetDummyOrders());
                await orderContext.SaveChangesAsync();
                logger.LogInformation($"Seeding database associated with context {typeof(OrderContext)} finished.");
            }
        }

        private static IEnumerable<Order> GetDummyOrders()
        {
            return new List<Order>
            {
                new Order { UserName = "Dummy user", FirstName = "Dummy", LastName="User", EmailAddress="dummy@adres.com", AddressLine="DummyCity", Country="Dummylandia", TotalPrice=350 }
            };
        }
    }
}
