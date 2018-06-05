using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoavShop.Models;

namespace YoavShop.ViewModels
{
    public class ProductViewModel
    {
        public IEnumerable<ProductCategorie> ProductCategories { get; set; }
        public Product Product { get; set; }
    }
}