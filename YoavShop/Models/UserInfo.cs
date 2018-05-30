using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;

namespace YoavShop.Models
{
    public class UserInfo
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(10)]
        [Index]
        public string UserName { get; set; }
        public string Password { get; set; }
        public int CreditCardId { get; set; }
        public virtual CreditCard CreditCard { get; set; }
    }
}