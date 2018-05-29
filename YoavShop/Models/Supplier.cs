using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoavShop.Models
{
    public class Supplier
    {
        public int Id { get; set; }
        public int UserInfoId { get; set; }
        public virtual UserInfo UserInfo { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Transaction> Sellings { get; set; }
    }
}