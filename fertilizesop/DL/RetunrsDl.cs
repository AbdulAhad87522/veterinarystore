using fertilizesop.BL.Models;
using KIMS;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Animation;

namespace fertilizesop.DL
{
    public class RetunrsDl : IRetunrsDl
    {
        public bool addreturn(returns r)
        {
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();

                    using (var trans = conn.BeginTransaction())
                    {
                        // 1. Insert into customer_return_items
                        string query1 = @"INSERT INTO customer_return_items
                                  (product_id, quantity_returned, refund_amount, return_date, bill_id)
                                  VALUES(@product_id, @quantity_returned, @amount, @return_date, @bill_id);";
                        using (var cmd1 = new MySqlCommand(query1, conn, trans))
                        {
                            cmd1.Parameters.AddWithValue("@product_id", r.product_id);
                            cmd1.Parameters.AddWithValue("@quantity_returned", r.quantity_returned);
                            cmd1.Parameters.AddWithValue("@amount", r.amount);
                            cmd1.Parameters.AddWithValue("@return_date", r.return_date);
                            cmd1.Parameters.AddWithValue("@bill_id", r.bill_id);
                            cmd1.ExecuteNonQuery();
                        }

                        // 2. Update product stock
                        string query2 = @"UPDATE products 
                                  SET quantity = quantity + @quantity_returned 
                                  WHERE product_id = @product_id;";
                        using (var cmd2 = new MySqlCommand(query2, conn, trans))
                        {
                            cmd2.Parameters.AddWithValue("@quantity_returned", r.quantity_returned);
                            cmd2.Parameters.AddWithValue("@product_id", r.product_id);
                            cmd2.ExecuteNonQuery();
                        }

                        // 3. Insert into inventory log
                        string query3 = @"INSERT INTO inventory_log
                                  (product_id, change_type, quantity_change, log_date, remarks)
                                  VALUES(@product_id, @change_type, @change_quantity, @change_date, @remarks);";
                        using (var cmd3 = new MySqlCommand(query3, conn, trans))
                        {
                            cmd3.Parameters.AddWithValue("@product_id", r.product_id);
                            cmd3.Parameters.AddWithValue("@change_type", "return");
                            cmd3.Parameters.AddWithValue("@change_quantity", r.quantity_returned);
                            cmd3.Parameters.AddWithValue("@change_date", DateTime.Now);
                            cmd3.Parameters.AddWithValue("@remarks", $"Return for Bill ID: {r.bill_id}");
                            cmd3.ExecuteNonQuery();
                        }

                        // 4. Update bill based on payment_status
                        string query4 = @"
                    UPDATE customerbills 
                    SET 
                        total_price = total_price - @amount,
                        paid_amount = CASE 
                            WHEN payment_status = 'Paid' THEN paid_amount - @amount
                            ELSE paid_amount
                        END
                    WHERE BillID = @bill_id;";
                        using (var cmd4 = new MySqlCommand(query4, conn, trans))
                        {
                            cmd4.Parameters.AddWithValue("@amount", r.amount);
                            cmd4.Parameters.AddWithValue("@bill_id", r.bill_id);
                            cmd4.ExecuteNonQuery();
                        }

                        // 5. Fix payment_status after refund
                        string fixStatusQuery = @"
                    UPDATE customerbills
                    SET payment_status = 
                        CASE 
                            WHEN paid_amount >= total_price THEN 'Paid'
                            ELSE 'Due'
                        END
                    WHERE BillID = @bill_id;";
                        using (var cmdStatus = new MySqlCommand(fixStatusQuery, conn, trans))
                        {
                            cmdStatus.Parameters.AddWithValue("@bill_id", r.bill_id);
                            cmdStatus.ExecuteNonQuery();
                        }

                        // 6. Get customer_id for logging
                        int customerId = 0;
                        using (var getCustomerCmd = new MySqlCommand(
                            "SELECT CustomerID FROM customerbills WHERE BillID = @bill_id", conn, trans))
                        {
                            getCustomerCmd.Parameters.AddWithValue("@bill_id", r.bill_id);
                            var result = getCustomerCmd.ExecuteScalar();
                            if (result != null)
                                customerId = Convert.ToInt32(result);
                        }

                        // 7. Insert into customer_price_record
                        string query5 = @"
                    INSERT INTO customerpricerecord
                    (BillID, customer_id, date, payment, remarks)
                    VALUES(@bill_id, @customer_id, NOW(), @payment, @remarks);";
                        using (var cmd5 = new MySqlCommand(query5, conn, trans))
                        {
                            cmd5.Parameters.AddWithValue("@bill_id", r.bill_id);
                            cmd5.Parameters.AddWithValue("@customer_id", customerId);
                            cmd5.Parameters.AddWithValue("@payment", -r.amount); // Negative for refund
                            cmd5.Parameters.AddWithValue("@remarks", $"Refund for Product ID: {r.product_id}");
                            cmd5.ExecuteNonQuery();
                        }

                        trans.Commit();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding return: {ex.Message}", "DB Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }


        public List<Products> getproducts(string name)
        {
            return DatabaseHelper.Instance.GetProductsByNames(name);
        }

    }
}