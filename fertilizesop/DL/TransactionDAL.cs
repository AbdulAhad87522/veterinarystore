using fertilizesop.BL.Models;
using KIMS;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;

namespace fertilizesop.DL
{
    public class TransactionDAL : ITransactionDAL
    {
        private MySqlConnection conn => DatabaseHelper.Instance.GetConnection();

        public bool InsertTransaction(Transaction t)
        {
            using (var connection = conn)
            {
                string query = @"INSERT INTO transaction_history 
                                (transaction_type, bank_id, date, amount) 
                                 VALUES (@type, @bank_id, @date, @amount)";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@type", t.TransactionType);
                cmd.Parameters.AddWithValue("@bank_id", t.BankId);
                cmd.Parameters.AddWithValue("@date", t.TransactionDate);
                cmd.Parameters.AddWithValue("@amount", t.Amount);
                connection.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        public static List<Transaction> SearchTransactions(string keyword, int? bankId = null)
        {
            var transactions = new List<Transaction>();

            using (var connection = DatabaseHelper.Instance.GetConnection())
            {
                // Base query
                string query = @"
            SELECT t.record_id, t.transaction_type, t.bank_id, t.amount, t.date, b.bank_name
            FROM transaction_history t
            INNER JOIN new_table b ON t.bank_id = b.bank_id
            WHERE (b.bank_name LIKE @keyword
               OR t.transaction_type LIKE @keyword
               OR t.amount LIKE @keyword
               OR DATE_FORMAT(t.date, '%Y-%m-%d') LIKE @keyword)";

                // Filter by bankId if provided
                if (bankId.HasValue)
                {
                    query += " AND t.bank_id = @bankId";
                }

                query += " ORDER BY t.date DESC";

                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");

                    if (bankId.HasValue)
                        cmd.Parameters.AddWithValue("@bankId", bankId.Value);

                    connection.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            transactions.Add(new Transaction
                            {
                                TransactionId = Convert.ToInt32(reader["transaction_id"]),
                                BankId = Convert.ToInt32(reader["bank_id"]),
                                BankName = reader["bank_name"].ToString(),
                                TransactionType = reader["transaction_type"].ToString(),
                                Amount = Convert.ToDecimal(reader["amount"]),
                                TransactionDate = Convert.ToDateTime(reader["date"])
                            });
                        }
                    }
                }
            }

            return transactions;
        }


        public List<Transaction> GetTransactionsByBankId(int bankId)
        {
            var list = new List<Transaction>();

            using (var connection = conn)
            {
                string query = @"SELECT t.record_id, t.transaction_type, 
                                        t.amount, t.date, b.bank_name
                                 FROM transaction_history t
                                 INNER JOIN new_table b ON t.bank_id = b.bank_id
                                 WHERE t.bank_id = @bank_id
                                 ORDER BY t.date DESC";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@bank_id", bankId);
                connection.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Transaction
                        {
                            TransactionId = Convert.ToInt32(reader["record_id"]),
                            BankId = bankId,
                            BankName = reader["bank_name"].ToString(),
                            TransactionType = reader["transaction_type"].ToString(),
                            Amount = Convert.ToDecimal(reader["amount"]),
                            TransactionDate = Convert.ToDateTime(reader["date"])
                        });
                    }
                }
            }

            return list;
        }
    }
}
