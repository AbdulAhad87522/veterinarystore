using fertilizesop.BL.Models;
using KIMS;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fertilizesop.DL
{
    public class SupplierbillDl : ISupplierbillDl
    {
        public Supplierbill GetSupplierBillByBatchName(string batchName)
        {
            if (string.IsNullOrWhiteSpace(batchName))
                throw new ArgumentException("Batch name is required", nameof(batchName));

            int batchId = DatabaseHelper.Instance.getbatchid(batchName);

            string query = @"
        SELECT 
            sb.supplier_bill_id,
            s.name AS supplier_name,
            b.batch_name,
            sb.total_price,
            sb.date,
            sb.paid_amount
        FROM 
            supplierbills sb
        JOIN batches b ON sb.batch_id = b.batch_id
        JOIN suppliers s ON b.supplier_id = s.supplier_id
        WHERE sb.batch_id = @batch_id;
    ";

            using (var conn = DatabaseHelper.Instance.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@batch_id", batchId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Supplierbill
                            (
                                Convert.ToInt32(reader["supplier_bill_id"]),
                                reader["supplier_name"].ToString(),
                                reader["batch_name"].ToString(),
                                reader.IsDBNull(reader.GetOrdinal("total_price")) ? 0 : Convert.ToDecimal(reader["total_price"]),
                                reader.IsDBNull(reader.GetOrdinal("date")) ? DateTime.MinValue : Convert.ToDateTime(reader["date"]),
                                reader.IsDBNull(reader.GetOrdinal("paid_amount")) ? 0 : Convert.ToDecimal(reader["paid_amount"])
                            );
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public bool UpdateBill(Supplierbill s)
        {
            try
            {
                int batch_id = DatabaseHelper.Instance.getbatchid(s.batch_name);
                int supplier_id = DatabaseHelper.Instance.getsuppierid(s.supplier_name);

                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();

                    using (var tran = conn.BeginTransaction())
                    {
                        try
                        {
                            // 1. Update the supplier bill
                            string updateQuery = @"UPDATE supplierbills 
                                           SET total_price = @total,
                                               paid_amount = paid_amount + @paid
                                           WHERE batch_id = @batch_id AND supplier_id = @supplier_id";
                            using (var cmd = new MySqlCommand(updateQuery, conn, tran))
                            {
                                cmd.Parameters.AddWithValue("@batch_id", batch_id);
                                cmd.Parameters.AddWithValue("@supplier_id", supplier_id);
                                cmd.Parameters.AddWithValue("@total", s.total_price);
                                cmd.Parameters.AddWithValue("@paid", s.paid_price);
                                cmd.ExecuteNonQuery();
                            }

                            // 2. Get the supplier_bill_id
                            string selectBillId = @"SELECT supplier_bill_id FROM supplierbills 
                                            WHERE batch_id = @batch_id AND supplier_id = @supplier_id LIMIT 1";
                            int billId;
                            using (var cmd = new MySqlCommand(selectBillId, conn, tran))
                            {
                                cmd.Parameters.AddWithValue("@batch_id", batch_id);
                                cmd.Parameters.AddWithValue("@supplier_id", supplier_id);
                                var result = cmd.ExecuteScalar();
                                if (result == null)
                                    throw new Exception("No matching bill found.");
                                billId = Convert.ToInt32(result);
                            }

                            // 3. Insert into supplierpricerecord
                            string insertRecord = @"INSERT INTO supplierpricerecord 
                                            (supplier_id, supplier_bill_id, date, payment, remarks)
                                            VALUES (@supplier_id, @bill_id, CURRENT_DATE(), @payment, 'Initial Payment')";
                            using (var cmd = new MySqlCommand(insertRecord, conn, tran))
                            {
                                cmd.Parameters.AddWithValue("@supplier_id", supplier_id);
                                cmd.Parameters.AddWithValue("@bill_id", billId);
                                cmd.Parameters.AddWithValue("@payment", s.paid_price);
                                cmd.ExecuteNonQuery();
                            }

                            tran.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            throw new Exception("Failed during transaction: " + ex.Message, ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update bill: " + ex.Message, ex);
            }
        }

        public List<Supplierbill> getbills(string text)
        {
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT    sb.supplier_Bill_ID, sb.batch_id,sb.supplier_id,  sb.paid_amount,   sb.total_price,    (sb.total_price-sb.paid_amount) as pending,   sb.Date,    s.name,    b.batch_name,sb.payment_status as status FROM   supplierbills sb      JOIN   suppliers s ON s.supplier_id = sb.supplier_id      JOIN    batches b ON b.batch_id = sb.batch_id    where s.name like @text or b.batch_name like @text or sb.payment_status like @text ;";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        var list = new List<Supplierbill>();
                        cmd.Parameters.AddWithValue("@text", "%" + text + "%");
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int billid = reader.GetInt32("supplier_Bill_ID");
                                string suppname = reader.GetString("name");
                                string Bname = reader.GetString("batch_name");
                                decimal paidamount = reader.IsDBNull(reader.GetOrdinal("paid_amount")) ? 0 : reader.GetDecimal(reader.GetOrdinal("paid_amount"));
                                decimal totalamount = reader.IsDBNull(reader.GetOrdinal("total_price")) ? 0 : reader.GetDecimal(reader.GetOrdinal("total_price"));
                                decimal pending = reader.IsDBNull(reader.GetOrdinal("pending")) ? 0 : reader.GetDecimal(reader.GetOrdinal("pending"));
                                DateTime date = reader.GetDateTime("date");
                                int batch_id = reader.GetInt32("batch_id");
                                int suppid = reader.GetInt32("supplier_id");
                                string status = reader.GetString("status");
                                var bills = new Supplierbill(billid, suppname, date, totalamount, paidamount, Bname, pending, batch_id, suppid, status);
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
        public List<Supplierbill> getbill()
        {
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT    sb.supplier_Bill_ID,   sb.paid_amount, sb.batch_id,sb.supplier_id,   sb.total_price,    (sb.total_price-sb.paid_amount) as pending,   sb.Date,    s.name,    b.batch_name,sb.payment_status as Status FROM   supplierbills sb      JOIN   suppliers s ON s.supplier_id = sb.supplier_id      JOIN    batches b ON b.batch_id = sb.batch_id ";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        var list = new List<Supplierbill>();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int billid = reader.GetInt32("supplier_Bill_ID");
                                string suppname = reader.GetString("name");
                                string Bname = reader.GetString("batch_name");
                                decimal paidamount = reader.IsDBNull(reader.GetOrdinal("paid_amount")) ? 0 : reader.GetDecimal(reader.GetOrdinal("paid_amount"));
                                decimal totalamount = reader.IsDBNull(reader.GetOrdinal("total_price")) ? 0 : reader.GetDecimal(reader.GetOrdinal("total_price"));
                                decimal pending = reader.IsDBNull(reader.GetOrdinal("pending")) ? 0 : reader.GetDecimal(reader.GetOrdinal("pending"));
                                string status = reader.GetString("Status");
                                DateTime date = reader.GetDateTime("date");
                                int batch_id = reader.GetInt32("batch_id");
                                int suppid = reader.GetInt32("supplier_id");
                                var bills = new Supplierbill(billid, suppname, date, totalamount, paidamount, Bname, pending, batch_id, suppid, status);

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
        public List<Supplierbill> getbills(int billid)
        {
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT    sb.supplier_Bill_ID,   sb.paid_amount, sb.batch_id,sb.supplier_id,  sb.total_price,    (sb.total_price-sb.paid_amount) as pending,   sb.Date,    s.name,    b.batch_name,sb.payment_status as status FROM   supplierbills sb      JOIN   suppliers s ON s.supplier_id = sb.supplier_id      JOIN    batches b ON b.batch_id = sb.batch_id    where sb.supplier_Bill_ID=@billid";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        var list = new List<Supplierbill>();
                        cmd.Parameters.AddWithValue("@billid", billid);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int billsid = reader.GetInt32("supplier_Bill_ID");
                                string suppname = reader.GetString("name");
                                string Bname = reader.GetString("batch_name");
                                decimal paidamount = reader.IsDBNull(reader.GetOrdinal("paid_amount")) ? 0 : reader.GetDecimal(reader.GetOrdinal("paid_amount"));
                                decimal totalamount = reader.IsDBNull(reader.GetOrdinal("total_price")) ? 0 : reader.GetDecimal(reader.GetOrdinal("total_price"));
                                decimal pending = reader.IsDBNull(reader.GetOrdinal("pending")) ? 0 : reader.GetDecimal(reader.GetOrdinal("pending"));
                                string status = reader.GetString("status");
                                DateTime date = reader.GetDateTime("date");

                                int batch_id = reader.GetInt32("batch_id");
                                int suppid = reader.GetInt32("supplier_id");
                                var bills = new Supplierbill(billsid, suppname, date, totalamount, paidamount, Bname, pending, batch_id, suppid, status);

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
    }
}
