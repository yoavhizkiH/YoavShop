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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        [CreditCard]
        public string CardNumber { get; set; }
        [Range(2018, 2028)]
        public int ExiprationYear { get; set; }
        [Range(1,12)]
        public int ExiprationMounth { get; set; }
    }
}