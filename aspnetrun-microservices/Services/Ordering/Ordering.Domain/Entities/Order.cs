using Ordering.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Ordering.Domain.Entities
{
    public class Order : EntityBase
    {
        [StringLength(1024)]
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }

        // BillingAddress
        [StringLength(1024)]
        public string FirstName { get; set; }
        [StringLength(1024)]
        public string LastName { get; set; }
        [StringLength(254)]
        public string EmailAddress { get; set; }
        [StringLength(1024)]
        public string AddressLine { get; set; }
        [StringLength(1024)]
        public string Country { get; set; }
        [StringLength(1024)]
        public string State { get; set; }
        [StringLength(32)]
        public string ZipCode { get; set; }

        // Payment
        [StringLength(1024)]
        public string CardName { get; set; }
        [StringLength(16)]
        public string CardNumber { get; set; }
        [StringLength(5)]
        public string Expiration { get; set; }
        [StringLength(3)]
        public string CVV { get; set; }
        public int PaymentMethod { get; set; }
    }
}
