using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoavShop.BL;

namespace YoavShop.ViewModels
{
    public class PagedSearchProductsViewModel
    {
        public PagedList.IPagedList<YoavShop.Models.Product> PagedList { get; set; }
        public ProductSearchModel ProductSearchModel { get; set; }
    }
}