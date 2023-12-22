using Order.Core.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Core.Entities
{
    public class Order : EntityBase
    {
        public string? UserName { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalPrice { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? EmailAddress { get; set; }
        public string? AddressLine { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public string? CardName { get; set; }
        public string? CardNumber { get; set; }
        public string? Expiration { get; set; }
        public string? Cvv { get; set; }
        public int? PaymentMethod { get; set; }
    }
}
