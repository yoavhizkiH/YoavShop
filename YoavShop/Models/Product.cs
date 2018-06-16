using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace YoavShop.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Range(1, 999.99)]
        public int Price { get; set; }
        [Range(1, 900)]
        public int Amount { get; set; }
        public ProductColor Color { get; set; }
        public bool IsActive { get; set; } = true;
        public int ProductCategorieId { get; set; }
        [ForeignKey("Supplier")]
        public int SupplierId { get; set; }
        public virtual ProductCategorie ProductCategorie { get; set; }
        public virtual Supplier Supplier { get; set; }
    }

    public enum ProductColor
    {
        White,
        Black,
        Gold,
        Blue,
        Red,
        Green,
        Silver
    }
}