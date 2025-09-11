using fertilizesop.BL.Models;
using System.Collections.Generic;

namespace fertilizesop.DL
{
    public interface ITransactionDAL
    {
        List<Transaction> GetTransactionsByBankId(int bankId);
        bool InsertTransaction(Transaction t);
    }
}