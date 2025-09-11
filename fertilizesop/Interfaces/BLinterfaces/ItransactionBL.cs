using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fertilizesop.BL.Models;


namespace fertilizesop.Interfaces.BLInterfaces
{
    public interface ItransactionBL
    {
        bool AddTransaction(string bankName, string type, decimal amount, DateTime date);
        List<Transaction> GetBankTransactionHistory(int bankId);
    }
}
