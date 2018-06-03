using System;
using System.Collections.Generic;
using System.Linq;
using YoavShop.Models;
using YoavShop.DAL;

namespace YoavShop.BL
{
    public class ProductSearch
    {
        private YoavShopContext YoavShopContext { get; set; }

        public ProductSearch()
        {
            YoavShopContext = new YoavShopContext();
        }

        public IQueryable<Product> GetProducts(ProductSearchModel searchModel)
        {
            var result = YoavShopContext.Products.AsQueryable();
            if (searchModel == null) return result;
            if (!string.IsNullOrEmpty(searchModel.Name))
                result = result.Where(x => x.Name.Contains(searchModel.Name));
            if (!string.IsNullOrEmpty(searchModel.Description))
                result = result.Where(x => x.Description.Contains(searchModel.Description));
            if (searchModel.MinPrice.HasValue)
                result = result.Where(x => x.Price >= searchModel.MinPrice);
            if (searchModel.MaxPrice.HasValue)
                result = result.Where(x => x.Price <= searchModel.MaxPrice);
            if (searchModel.MinAmount.HasValue)
                result = result.Where(x => x.Amount >= searchModel.MinAmount);
            if (searchModel.MaxAmount.HasValue)
                result = result.Where(x => x.Price <= searchModel.MaxAmount);
            if (searchModel.Color.HasValue)
                result = result.Where(x => x.Color == searchModel.Color);
            if (searchModel.SupplierId.HasValue)
                result = result.Where(x => x.SupplierId == searchModel.SupplierId);
            if (searchModel.ProductCategorieId.HasValue)
                result = result.Where(x => x.ProductCategorieId == searchModel.ProductCategorieId);
            return result;
        }
    }
}