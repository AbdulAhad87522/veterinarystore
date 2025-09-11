using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fertilizesop.BL.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public int BankId { get; set; }
        public string BankName { get; set; }
        public string TransactionType { get; set; } // "Deposit" or "Withdraw"
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }

        public Transaction ()
        {

        }
public Transaction(int transactionId, int bankId, string bankName, string transactionType, decimal amount, DateTime transactionDate)
        {
            TransactionId = transactionId;
            BankId = bankId;
            BankName = bankName;
            TransactionType = transactionType;
            Amount = amount;
            TransactionDate = transactionDate;
        }
    }
}
