using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;

namespace YoavShop.Models
{
    public class UserInfo
    {
        public int Id { get; set; }
        [Index(IsUnique = true)]
        public string UserName { get; set; }
        public int CreditCardId { get; set; }
        public virtual CreditCard CreditCard { get; set; }
    }
}