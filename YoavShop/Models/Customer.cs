using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YoavShop.Models
{
    public partial class Customer : UserInfo
    {
        [Column(TypeName = "VARCHAR")]
        [StringLength(10)]
        [Required]
        public override string UserName { get; set; }
        public virtual ICollection<Transaction> Buyings{ get; set; }
    }

    [MetadataType(typeof(CustomerMetaData))]
    public partial class Customer
    {
    }

    public class CustomerMetaData
    {
        [Remote("IsUserExists", "Customer", ErrorMessage = "User Name already in use")]
        public string UserName { get; set; }
    }
}