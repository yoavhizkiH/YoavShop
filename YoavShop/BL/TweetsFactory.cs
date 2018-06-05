using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoavShop.ExternalFeauters;
using YoavShop.Models;

namespace YoavShop.BL
{
    public class TweetsFactory
    {
        private TwitterApi TwitterApi { get; set; }

        public TweetsFactory()
        {
            TwitterApi = new TwitterApi();
        }

        public void Edit(Product beforeChanges, Product afterChanges)
        {
            var message = "";

            if (beforeChanges.Amount == 0 && afterChanges.Amount > 0)
            {
                message += $"New supply of product {afterChanges.Name} is Here! Come and get it before its over. ";
            }
            if (beforeChanges.Color != afterChanges.Color)
            {
                message += $"New supply of product {afterChanges.Name} with diffrent color is Here! " +
                          $"The new color is {afterChanges.Color}. ";
            }

            if (!string.IsNullOrEmpty(message))
            {
                TwitterApi.Tweet(message);
            }
        }

        public void Create(Product product)
        {
            var message =
                $"New product is Here! Under the categorie {product.ProductCategorie.Name} you can find the newest product in the store - {product.Name} for only {product.Price}$. Hurry up! There are only {product.Amount} Left!";
            TwitterApi.Tweet(message);
        }
    }
}