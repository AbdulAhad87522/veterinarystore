using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fertilizesop.BL.Models;

namespace fertilizesop.Interfaces.DLinterfaces
{
    internal interface ItransactionDL
    {
        bool InsertTransaction(Transaction t);
        decimal GetLatestBalance();
        DataTable GetAllTransactions();
        DataTable SearchTransactions(string keyword);
    }
}
