using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoavShop.Models
{
    public class CreditCard
    {
        public int Id { get; set; }
        public long CardNumber { get; set; }
        public int ExiprationYear  { get; set; }
        public int ExiprationMounth { get; set; }
    }
}