using fertilizesop.BL.Models;
using KIMS;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace fertilizesop.DL
{
    public class Customerbilldl
    {
        private readonly DatabaseHelper _dbHelper;

        public Customerbilldl()
        {
            _dbHelper = DatabaseHelper.Instance;
        }

        public DataTable GetCustomerBillingRecords(string searchTerm = "")
        {
            DataTable dt = new DataTable();

            try
            {
                string query = @"
            SELECT 
                cb.BillID,
                CONCAT(c.first_name, ' ', c.last_name) AS CustomerName,
                c.phone AS CustomerPhone,
                DATE_FORMAT(cb.SaleDate, '%d-%m-%Y %h:%i %p') AS SaleDate,
                CAST(cb.total_price AS DECIMAL(12,2)) AS TotalAmount,
                CAST(IFNULL(cb.paid_amount, 0) AS DECIMAL(12,2)) AS PaidAmount,
                CAST((cb.total_price - IFNULL(cb.paid_amount, 0)) AS DECIMAL(12,2)) AS DueAmount,
                CASE 
                    WHEN (cb.total_price - IFNULL(cb.paid_amount, 0)) <= 0 THEN 'Paid'
                    ELSE 'Due'
                END AS payment_status
            FROM 
                customerbills cb
            JOIN 
                customers c ON cb.CustomerID = c.customer_id
            WHERE 
                cb.BillID LIKE @searchTerm OR
                CONCAT(c.first_name, ' ', c.last_name) LIKE @searchTerm OR
                c.phone LIKE @searchTerm OR
                cb.SaleDate LIKE @searchTerm
            ORDER BY 
                cb.SaleDate DESC";

                using (var conn = _dbHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@searchTerm", $"%{searchTerm}%");

                        using (var adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving billing records: " + ex.Message);
            }

            return dt;
        }

        public List<customerbill> getbill()
        {
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT    sb.BillID, sb.paid_amount, sb.CustomerID,   sb.total_price,    (sb.total_price-sb.paid_amount) as pending,   sb.SaleDate,    concat(s.first_name,' ',s.last_name) as name, sb.payment_status as Status FROM   customerbills sb      JOIN   customers s ON s.customer_id = sb.CustomerID ";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        var list = new List<customerbill>();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int billid = reader.GetInt32("BillID");
                                string customer_name = reader.GetString("name");
                                decimal totalamount = reader.IsDBNull(reader.GetOrdinal("total_price")) ? 0 : reader.GetDecimal(reader.GetOrdinal("total_price"));
                                decimal paidamount = reader.IsDBNull(reader.GetOrdinal("paid_amount")) ? 0 : reader.GetDecimal(reader.GetOrdinal("paid_amount"));
                                decimal pending = reader.IsDBNull(reader.GetOrdinal("pending")) ? 0 : reader.GetDecimal(reader.GetOrdinal("pending"));
                                string status = reader.GetString("Status");
                                DateTime date = reader.GetDateTime("SaleDate");
                                int customer_id = reader.GetInt32("CustomerID");
                                var bills = new customerbill(billid, customer_name, date, totalamount, paidamount, pending, customer_id, status);

                                list.Add(bills);



                            }
                        }
                        return list;
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception("Error retrieving batches: " + ex.Message, ex);
            }
        }

        public List<customerbill> searchbill(string text)
        {
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT    sb.BillID, sb.paid_amount, sb.CustomerID,   sb.total_price,    (sb.total_price-sb.paid_amount) as pending,   sb.SaleDate,    concat(s.first_name,' ',s.last_name) as name, sb.payment_status as Status FROM   customerbills sb      JOIN   customers s ON s.customer_id = sb.CustomerID  where concat(s.first_name,' ',s.last_name) like @text or BillID like @text or payment_status like @text";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@text", "%" + text + "%");
                        var list = new List<customerbill>();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int billid = reader.GetInt32("BillID");
                                string customer_name = reader.GetString("name");
                                decimal totalamount = reader.IsDBNull(reader.GetOrdinal("total_price")) ? 0 : reader.GetDecimal(reader.GetOrdinal("total_price"));
                                decimal paidamount = reader.IsDBNull(reader.GetOrdinal("paid_amount")) ? 0 : reader.GetDecimal(reader.GetOrdinal("paid_amount"));
                                decimal pending = reader.IsDBNull(reader.GetOrdinal("pending")) ? 0 : reader.GetDecimal(reader.GetOrdinal("pending"));
                                string status = reader.GetString("Status");
                                DateTime date = reader.GetDateTime("SaleDate");
                                int customer_id = reader.GetInt32("CustomerID");
                                var bills = new customerbill(billid, customer_name, date, totalamount, paidamount, pending, customer_id, status);

                                list.Add(bills);



                            }
                        }
                        return list;
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception("Error retrieving batches: " + ex.Message, ex);
            }
        }

        public DataTable GetBillDetails(int billId)
        {
            DataTable dt = new DataTable();

            try
            {
                string query = @"
                    SELECT 
                        p.name AS ProductName,
                        cbd.quantity,
                        p.description AS ProductDescription,
                        (i.sale_price * cbd.quantity) AS TotalPrice,
                        cbd.discount,
                        cbd.status,
                        cbd.warranty,
                        cbd.warranty_from,
                        cbd.warranty_till
                    FROM 
                        customer_bill_details cbd
                    JOIN 
                        products p ON cbd.product_id = p.product_id
                    JOIN 
                        inventory i ON p.product_id = i.product_id
                    WHERE 
                        cbd.bill_id = @billId";

                using (var conn = _dbHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@billId", billId);

                        using (var adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving bill details: " + ex.Message);
            }

            return dt;
        }

        public DataTable GetPaymentHistory(int billId)
        {
            DataTable dt = new DataTable();

            try
            {
                string query = @"
                    SELECT 
                        date AS PaymentDate,
                        payment AS Amount,
                        BillID
                    FROM 
                        customerpricerecord
                    WHERE 
                        BillID = @billId
                    ORDER BY 
                        date DESC";

                using (var conn = _dbHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@billId", billId);

                        using (var adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving payment history: " + ex.Message);
            }

            return dt;
        }
        public static bool AddRecord(Customerrecord s)
        {
            int customerId = DatabaseHelper.Instance.getcustid(s.name);

            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    {
                        // Insert into customerpricerecord
                        string insertQuery = @"
                    INSERT INTO customerpricerecord
                    (customer_id, billid, date, payment, remarks)
                    VALUES (@cust_id, @billid, @date, @payment, @remarks);";

                        using (var insertCmd = new MySqlCommand(insertQuery, conn, transaction))
                        {
                            insertCmd.Parameters.AddWithValue("@cust_id", customerId);
                            insertCmd.Parameters.AddWithValue("@billid", s.bill_id);
                            insertCmd.Parameters.AddWithValue("@date", s.date);
                            insertCmd.Parameters.AddWithValue("@payment", s.payement);
                            insertCmd.Parameters.AddWithValue("@remarks", s.remarks);

                            insertCmd.ExecuteNonQuery();
                        }

                        // Update paid_amount in customerbills
                        string updateQuery = @"
                    UPDATE customerbills
                    SET paid_amount = paid_amount + @payment
                    WHERE billid = @billid;";

                        using (var updateCmd = new MySqlCommand(updateQuery, conn, transaction))
                        {
                            updateCmd.Parameters.AddWithValue("@payment", s.payement);
                            updateCmd.Parameters.AddWithValue("@billid", s.bill_id);

                            updateCmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding payment record and updating bill: " + ex.Message, ex);
            }
        }


        public static List<Customerrecord> getrecord(int billid)
        {
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    var listt = new List<Customerrecord>();
                    conn.Open();
                    string query = "select * from customerpricerecord where BillID=@billid;";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@billid", billid);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32("record_id");
                                int billsid = reader.GetInt32("BillID");
                                int custid = reader.GetInt32("customer_id");
                                DateTime date = reader.GetDateTime("date");
                                decimal payments = reader.GetDecimal("payment");
                                string remarks = reader["remarks"] == DBNull.Value ? "" : reader.GetString("remarks");

                                var record = new Customerrecord(id, custid, payments, date, billsid, remarks);

                                listt.Add(record);
                            }
                        }
                        return listt;
                    }
                }
            }
            catch (Exception ex) { throw new Exception("error" + ex.Message, ex); }
        }
    }
}