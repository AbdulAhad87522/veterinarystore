using fertilizesop.BL.Models;
using System.Collections.Generic;

namespace fertilizesop.BL.Bl
{
    public interface IBankBl
    {
        bool AddBank(string bankName, decimal initialDeposit);
        List<string> GetAllBankNames();
        int GetBankId(string bankName);
        decimal GetRemainingBalance(string bankName);
        bool UpdateBalance(int bankId, decimal newBalance);
        List<Bank> GetAllBanks();
    }
}