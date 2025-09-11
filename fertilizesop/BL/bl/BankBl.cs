using fertilizesop.BL.Models;
using fertilizesop.DL;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fertilizesop.BL.Bl
{
    public class BankBl : IBankBl
    {
        private readonly IBankDAL idl;
        public BankBl(IBankDAL bankDal)
        {
            this.idl = bankDal;
        }
        public bool AddBank(string bankName, decimal initialDeposit)
        {
            Bank bank = new Bank
            {
                BankName = bankName,
                RemainingBalance = initialDeposit
            };
            bool result = idl.InsertBank(bank);
            if (result)
            {
                // ✅ Automatically backup to .dat
                MySqlBackupHelper.CreateBackup();
            }

            return result;
        }

        public decimal GetRemainingBalance(string bankName)
        {
            return idl.GetRemainingBalance(bankName);
        }

        public List<string> GetAllBankNames()
        {
            return idl.GetBankNames();
        }

        public int GetBankId(string bankName)
        {
            return idl.GetBankIdByName(bankName);
        }

        public bool UpdateBalance(int bankId, decimal newBalance)
        {
            return idl.UpdateRemainingBalance(bankId, newBalance);
        }
        public List<Bank> GetAllBanks()
        {
            return idl.GetAllBanks();
        }
    }

}
