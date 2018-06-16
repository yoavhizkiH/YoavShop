using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YoavShop.Models
{
    public partial class Supplier : UserInfo
    {
        [Column(TypeName = "VARCHAR")]
        [StringLength(10)]
        [Required]
        public override string UserName { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Transaction> Sellings { get; set; }
    }

    [MetadataType(typeof(UserMetaData))]
    public partial class Supplier
    {
    }
    class UserMetaData
    {
        [Remote("IsUserExists", "Supplier", ErrorMessage = "User Name already in use")]
        public string UserName { get; set; }
    }
}