using fertilizesop.BL.Models;
using System.Collections.Generic;

namespace fertilizesop.DL
{
    public interface IBankDAL
    {
        List<Bank> GetAllBanks();
        int GetBankIdByName(string bankName);
        List<string> GetBankNames();
        decimal GetRemainingBalance(string bankName);
        bool InsertBank(Bank bank);
        bool UpdateRemainingBalance(int bankId, decimal newBalance);
    }
}