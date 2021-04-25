using System.Collections.Generic;
using System.Linq;

namespace Basket.API.Models
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {

        }
        public ShoppingCart(string username)
        {
            UserName = username;
        }

        public string UserName { get; set; }
        public IEnumerable<ShoppingCartItem> Items { get; set; }
        public decimal TotalPrice { get { return Items.Sum(x => x.Price * x.Quantity); } }
    }
}
