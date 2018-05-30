using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace YoavShop.Models
{
    public class Customer : UserInfo
    {
        public int Id { get; set; }
        public virtual ICollection<Transaction> Buyings{ get; set; }
    }
}