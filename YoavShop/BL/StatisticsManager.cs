using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoavShop.DAL;
using YoavShop.Models;

namespace YoavShop.BL
{
    public class StatisticsManager
    {
        public SupplierWithTransactionsModel[] GetSuppliersByMoney(List<Supplier> suppliers, List<Transaction> transactions)
        {
            SupplierWithTransactionsModel[] swtArray = new SupplierWithTransactionsModel[suppliers.Count];
            for (var i = 0; i < suppliers.Count; i++)
            {
                swtArray[i] = new SupplierWithTransactionsModel
                {
                    UserName = suppliers[i].UserName,
                    Count = transactions.Where(t => t.Product.SupplierId == suppliers[i].Id).Sum(t => t.MoneyPaid)
                };
            }

            return swtArray; 
        }

        public int[] TransactionsByTime(List<Transaction> transactions)
        {
            var g = transactions.GroupBy(t => t.TimeStamp.Date).OrderBy(t => t.Key).ToArray();
            var s = new int[g.Count()];
            for (var i = 0; i < s.Length; i++)
            {
                s[i] = g[i].Sum(t => t.MoneyPaid);
            }

            return s;
        }
    }

    public class SupplierWithTransactionsModel
    {
        public string UserName { get; set; }
        public int Count { get; set; }
    }
}