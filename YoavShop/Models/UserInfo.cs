using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;

namespace YoavShop.Models
{
    public class UserInfo 
    {
        [Column(TypeName = "VARCHAR")]
        [StringLength(10)]
        [Index(IsUnique = true)]
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public long CardNumber { get; set; }
        [Range(1,2100, ErrorMessage = "Invalid Year")]
        public int ExiprationYear { get; set; }
        [Range(1, 12, ErrorMessage = "Invalid Mounth")]
        public int ExiprationMounth { get; set; }
    }
}