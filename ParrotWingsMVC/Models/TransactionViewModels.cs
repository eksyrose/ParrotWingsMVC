using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ParrotWingsMVC.Models
{
    public class TransactionViewModel
    {
        [Required]
    //    [Remote("CheckTransactionSum", "TransactionsController", ErrorMessage = "На вашем счету недостаточно средств для совершения транзакции.")]
        [Range(1, 1000000, ErrorMessage = "Сумма перевода не может быть меньше 1 и больше 1000000.")]
        public string TransactionAmount { get; set; }
    }
}