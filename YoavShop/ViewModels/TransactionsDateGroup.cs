using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace YoavShop.ViewModels
{
    public class TransactionsDateGroup
    {
        [DataType(DataType.Date)]
        public DateTime? TransactionDate { get; set; }

        public int TransactionsCount { get; set; }
    }
}