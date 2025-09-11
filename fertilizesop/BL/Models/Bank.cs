using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fertilizesop.BL.Models
{

    public class Bank
    {
        public int BankId { get; set; }
        public string BankName { get; set; }
        public decimal RemainingBalance { get; set; }
        public Bank(int bankId, string bankName, decimal remainingBalance)
        {
            BankId = bankId;
            BankName = bankName;
            RemainingBalance = remainingBalance;
        }
        public Bank()
        {

        }
    }
}
