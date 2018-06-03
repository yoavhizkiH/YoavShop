using System;
using System.Collections.Generic;
using System.Linq;
using YoavShop.Models;

namespace YoavShop.BL
{
    public class ProductSearchModel
    {
        private List<string> paramsList;

        public ProductSearchModel()
        {
            
        }
        public ProductSearchModel(string searchParams)
        {
            paramsList = searchParams.Split('?').ToList();
            Name = paramsList.Single(p => p.Contains("Name=")).Remove(0, 5);
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public int? MinAmount { get; set; }
        public int? MaxAmount { get; set; }
        public ProductColor? Color { get; set; }
        public int? ProductCategorieId { get; set; }
        public int? SupplierId { get; set; }

        public bool IsSearched()
        {
            if (!string.IsNullOrEmpty(Name))
            {
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return $"Name={Name}?Description={Description}?MinPrice={MinPrice}?MaxPrice={MaxPrice}?MinAmount={MinAmount}?MaxAmount={MaxAmount}?Color={Color}?";
        }
    }
}