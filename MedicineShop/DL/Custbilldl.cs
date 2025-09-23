using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicineShop.BL.Models;
using MedicineShop.Interfaces.DLInterfaces;
using MySql.Data.MySqlClient;
using static System.Net.Mime.MediaTypeNames;

namespace MedicineShop.DL
{
    internal class Custbilldl : Icustomerbilldl
    {
        public List<custbill> AddCustomerPayment(string text)
        {
            List<custbill> companyBills = new List<custbill>();

            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT b.customer_id, c.full_name, SUM(b.total_amount) AS total_price, SUM(b.paid_amount) AS paid, (SUM(b.total_amount) - SUM(b.paid_amount)) AS remaining
                                     FROM sales b
                                     JOIN customers c ON b.customer_id = c.customer_id
                                     WHERE c.full_name LIKE @search OR b.customer_id LIKE @search
                                     GROUP BY b.customer_id, c.full_name";
                    using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@search", $"%{text}%");
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                custbill bill = new custbill
                                {
                                    customer_id = reader.GetInt32("company_id"),
                                    full_name = reader.GetString("company_name"),
                                    total_amount = reader.GetDecimal("total_price"),
                                    paid = reader.GetDecimal("paid"),
                                    remaining = reader.GetDecimal("remaining")
                                };
                                companyBills.Add(bill);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching company bills", ex);
            }
            return companyBills;

        }

        public bool AddCustomerPayment(int companyId, decimal paymentAmount)
        {
            throw new NotImplementedException();
        }

        public List<custbill> GetCustomerBills(int companyid)
        {
            List<custbill> companyBills = new List<custbill>();

            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT b.customer_id, c.full_name, SUM(b.total_amount) AS total_price, SUM(b.paid_amount) AS paid, (SUM(b.total_amount) - SUM(b.paid_amount)) AS remaining
                                     FROM sales b
                                     JOIN customers c ON b.customer_id = c.customer_id
                                     WHERE c.full_name LIKE @search OR b.customer_id LIKE @search
                                     GROUP BY b.customer_id, c.full_name";
                    using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@search", $"%{companyid}%");
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                custbill bill = new custbill
                                {
                                    customer_id = reader.GetInt32("company_id"),
                                    full_name = reader.GetString("company_name"),
                                    total_amount = reader.GetDecimal("total_price"),
                                    paid = reader.GetDecimal("paid"),
                                    remaining = reader.GetDecimal("remaining")
                                };
                                companyBills.Add(bill);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching company bills", ex);
            }
            return companyBills;

        }
        public bool GetCustomerBills(int customerid, decimal paymentAmount)
        {
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    using (var tran = conn.BeginTransaction())
                    {
                        // 1. Insert into payment_records
                        string insertPayment = @"INSERT INTO customerpricerecord (customer_id, date, payment) 
                                         VALUES (@customerid, @date, @amount)";
                        using (var cmdInsert = new MySql.Data.MySqlClient.MySqlCommand(insertPayment, conn, tran))
                        {
                            cmdInsert.Parameters.AddWithValue("@customerid", customerid);
                            cmdInsert.Parameters.AddWithValue("@date", DateTime.Now);
                            cmdInsert.Parameters.AddWithValue("@amount", paymentAmount);
                            cmdInsert.ExecuteNonQuery();
                        }

                        // 2. Fetch unpaid batches in order
                        string selectQuery = @"SELECT sale_id, total_amount, paid_amount
                                       FROM sales
                                       WHERE customer_id = @customerid 
                                       AND (total_amount - paid_amount) > 0
                                       ORDER BY sale_date ASC, sale_id ASC";
                        using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(selectQuery, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@customerid", customerid);
                            using (var reader = cmd.ExecuteReader())
                            {
                                var sales = new List<(int id, decimal total, decimal paid)>();
                                while (reader.Read())
                                {
                                    sales.Add((
                                        reader.GetInt32("sale_id"),
                                        reader.GetDecimal("total_amount"),
                                        reader.GetDecimal("paid_amount")
                                    ));
                                }
                                reader.Close();

                                // 3. Distribute payment across batches
                                foreach (var sale in sales)
                                {
                                    if (paymentAmount <= 0) break;

                                    decimal remaining = sale.total - sale.paid;
                                    decimal toPay = Math.Min(paymentAmount, remaining);

                                    string updateQuery = @"UPDATE sales 
                                                   SET paid_amount = paid_amount + @toPay 
                                                   WHERE sale_id = @sale_id";
                                    using (var updateCmd = new MySql.Data.MySqlClient.MySqlCommand(updateQuery, conn, tran))
                                    {
                                        updateCmd.Parameters.AddWithValue("@toPay", toPay);
                                        updateCmd.Parameters.AddWithValue("@sale_id", sale.id);
                                        updateCmd.ExecuteNonQuery();
                                    }

                                    paymentAmount -= toPay;
                                }
                            }
                        }

                        tran.Commit();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding company payment", ex);
            }
        }

        public List<CompanyBill> GetCustomerBills(string text)
        {
            throw new NotImplementedException();
        }

        public List<custPaymentRecord> GetCustomerPaymentRecords(int companyId)
        {
            var records = new List<custPaymentRecord>();

            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    string query = @"
                SELECT 
                    p.record_id,
                    p.customer_id,
                    p.date,
                    p.payment,
                    c.full_name
                FROM customerpricerecord p
                JOIN customers c ON p.customer_id = c.customer_id
                WHERE p.customer_id = @companyId
                ORDER BY p.date DESC;
            ";

                    using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@companyId", companyId);
                        conn.Open();

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var record = new custPaymentRecord
                                {
                                    PaymentId = reader.GetInt32("payment_id"),
                                    customerId = reader.GetInt32("company_id"),
                                    Date = reader.GetDateTime("payment_date"),
                                    Amount = reader.GetDecimal("amount"),
                                    CustomerName = reader.GetString("company_name")
                                };
                                records.Add(record);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching payment records", ex);
            }

            return records;
        }


        public List<custPaymentRecord> GetcustPaymentRecords(int companyId)
        {
            var records = new List<custPaymentRecord>();
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = @"
                    SELECT 
                        pr.record_id,
                        pr.customer_id,
                        pr.date,
                        pr.payment,
                        pb.total_amount,
                        pb.paid_amount,
                        (pb.total_amount - pb.paid_amount) AS remaining_balance
                    FROM customerpricerecord pr
                    JOIN sales pb ON pr.customer_id = pb.customer_id
                    WHERE pr.customer_id = @CompanyId
                    ORDER BY pr.date DESC;
                ";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CompanyId", companyId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var record = new custPaymentRecord
                                {
                                    PaymentId = reader.GetInt32("payment_id"),
                                    customerId = reader.GetInt32("company_id"),
                                    Date = reader.GetDateTime("payment_date"),
                                    Amount = reader.GetDecimal("amount"),
                                    Status = reader.GetString("status"),
                                    TotalPrice = reader.GetDecimal("total_price"),
                                    Paid = reader.GetDecimal("paid"),
                                    RemainingBalance = reader.GetDecimal("remaining_balance")
                                };
                                records.Add(record);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching payment records: " + ex.Message);
            }

            return records;
        }
    }
}
