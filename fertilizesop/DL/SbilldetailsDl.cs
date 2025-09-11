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
    public class SbilldetailsDl : ISbilldetailsDl
    {
        public List<Supplierpayment> getdetails(int billid)
        {
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = @"
                                SELECT 
                                    p.product_id,
                                    p.name,
                                    p.description,
                                SUM(sb.quantity) AS quantity
                                FROM supplier_bill_details sb
                                JOIN supplierbills sbd ON sbd.supplier_bill_id = sb.supplier_bill_id
                                JOIN products p ON p.product_id = sb.product_id
                                WHERE sb.supplier_bill_id = @billid
                                GROUP BY p.product_id, p.name, p.description;
                                ";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        var list = new List<Supplierpayment>();
                        cmd.Parameters.AddWithValue("@billid", billid);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string Pname = reader.GetString("name");
                                string description = reader.GetString("description");
                                int quantity = reader.GetInt32("quantity");

                                // If Supplierpayment needs bill ID or dummy detail ID, you can pass 0
                                var bill = new Supplierpayment(billid, 0, quantity, Pname, description);
                                list.Add(bill);
                            }

                        }
                        return list;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error" + ex.Message, ex);
            }

        }
        public bool addrecord(Spricerecord s)
        {
            int supplier_id = DatabaseHelper.Instance.getsuppierid(s.name);
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();

                    using (var tran = conn.BeginTransaction())
                    {
                        try
                        {
                            // 1. Insert into supplierpricerecord
                            string insertQuery = @"INSERT INTO supplierpricerecord
                                           (supplier_id, supplier_bill_id, date, payment, remarks)
                                           VALUES (@supp_id, @billid, @date, @payment, @remarks);";

                            using (var cmd = new MySqlCommand(insertQuery, conn, tran))
                            {
                                cmd.Parameters.AddWithValue("@supp_id", supplier_id);
                                cmd.Parameters.AddWithValue("@billid", s.bill_id);
                                cmd.Parameters.AddWithValue("@date", s.date);
                                cmd.Parameters.AddWithValue("@payment", s.payement);
                                cmd.Parameters.AddWithValue("@remarks", s.remarks);

                                cmd.ExecuteNonQuery();
                            }

                            // 2. Update paid_amount in supplierbills
                            string updateQuery = @"UPDATE supplierbills 
                                           SET paid_amount = paid_amount + @payment
                                           WHERE supplier_bill_id = @billid";

                            using (var cmd = new MySqlCommand(updateQuery, conn, tran))
                            {
                                cmd.Parameters.AddWithValue("@payment", s.payement);
                                cmd.Parameters.AddWithValue("@billid", s.bill_id);

                                cmd.ExecuteNonQuery();
                            }

                            tran.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            throw new Exception("Transaction failed: " + ex.Message, ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message, ex);
            }
        }

        public List<Spricerecord> getrecord(int billid)
        {
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    var listt = new List<Spricerecord>();
                    conn.Open();
                    string query = "select * from supplierpricerecord where supplier_bill_id=@billid;";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@billid", billid);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32("supplier_record_id");
                                int billsid = reader.GetInt32("supplier_bill_id");
                                int suppid = reader.GetInt32("supplier_id");
                                string remarks = reader.GetString("remarks");
                                DateTime date = reader.GetDateTime("date");
                                decimal payments = reader.GetDecimal("payment");
                                var record = new Spricerecord(id, suppid, payments, date, billsid, remarks);
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
