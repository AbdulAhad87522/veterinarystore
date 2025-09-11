using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using fertilizesop.BL.Models;
using KIMS;

namespace fertilizesop.DL
{
    public class BankDAL : IBankDAL
    {
        private MySqlConnection conn => DatabaseHelper.Instance.GetConnection();

        public bool InsertBank(Bank bank)
        {
            using (var connection = conn)
            {
                string query = @"INSERT INTO new_table (bank_name, remaining_balance) 
                                 VALUES (@bank_name, @remaining_balance)";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@bank_name", bank.BankName);
                cmd.Parameters.AddWithValue("@remaining_balance", bank.RemainingBalance);
                connection.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public decimal GetRemainingBalance(string bankName)
        {
            using (var connection = conn)
            {
                string query = "SELECT remaining_balance FROM new_table WHERE bank_name = @bank_name";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@bank_name", bankName);
                connection.Open();
                var result = cmd.ExecuteScalar();
                return result != null ? Convert.ToDecimal(result) : 0;
            }
        }

        public bool UpdateRemainingBalance(int bankId, decimal newBalance)
        {
            using (var connection = conn)
            {
                string query = "UPDATE new_table SET remaining_balance = @balance WHERE bank_id = @bank_id";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@balance", newBalance);
                cmd.Parameters.AddWithValue("@bank_id", bankId);
                connection.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public List<string> GetBankNames()
        {
            var list = new List<string>();
            using (var connection = conn)
            {
                string query = "SELECT bank_name FROM new_table";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                connection.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(reader["bank_name"].ToString());
                }
            }
            return list;
        }
        public static List<Bank> SearchBanks(string keyword)
        {
            var banks = new List<Bank>();
            using (var connection = DatabaseHelper.Instance.GetConnection())
            {
                string query = @"
            SELECT * FROM new_table
            WHERE bank_name LIKE @keyword
               OR remaining_balance LIKE @keyword";

                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");
                connection.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        banks.Add(new Bank
                        {
                            BankId = Convert.ToInt32(reader["bank_id"]),
                            BankName = reader["bank_name"].ToString(),
                            RemainingBalance = Convert.ToDecimal(reader["remaining_balance"])
                        });
                    }
                }
            }
            return banks;
        }

        public int GetBankIdByName(string bankName)
        {
            using (var connection = conn)
            {
                string query = "SELECT bank_id FROM new_table WHERE bank_name = @bank_name";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@bank_name", bankName);
                connection.Open();
                var result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }
        public List<Bank> GetAllBanks()
        {
            var banks = new List<Bank>();
            using (var connection = conn)
            {
                string query = "SELECT * FROM new_table";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                connection.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var bank = new Bank
                    {
                        BankId = Convert.ToInt32(reader["bank_id"]),
                        BankName = reader["bank_name"].ToString(),
                        RemainingBalance = Convert.ToDecimal(reader["remaining_balance"])
                    };
                    banks.Add(bank);
                }
            }
            return banks;
        }
    }
}