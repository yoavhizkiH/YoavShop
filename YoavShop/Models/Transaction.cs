using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoavShop.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public int MoneyPaid { get; set; }
        public DateTime TimeStamp { get; set; }
        public virtual Product Product { get; set; }
        public virtual Customer Customer { get; set; }
    }
}