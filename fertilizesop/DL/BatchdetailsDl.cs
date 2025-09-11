using KIMS;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace fertilizesop.DL
{
    public class BatchdetailsDl : IBatchdetailsDl
    {
        public bool adddetails(BatchDetails b)
        {
            int batch_id = DatabaseHelper.Instance.getbatchid(b.batch_name);
                int old = oldquantity(b.product_id);

            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    using (var trans = conn.BeginTransaction())
                    {
                        // Insert into batch_details
                        string query1 = @"INSERT INTO batch_details(batch_id, product_id, cost_price, quantity_recived)
                                          VALUES(@batch_id, @product_id, @cost_price, @quantity);";

                        using (var cmd1 = new MySqlCommand(query1, conn, trans))
                        {
                            cmd1.Parameters.AddWithValue("@batch_id", batch_id);
                            cmd1.Parameters.AddWithValue("@product_id", b.product_id);
                            cmd1.Parameters.AddWithValue("@quantity", b.quantity_received);
                            cmd1.Parameters.AddWithValue("@cost_price", b.cost_price);
                            cmd1.ExecuteNonQuery();
                        }

                        // Update products with new quantity and sale price
                        int new_quantity = old + b.quantity_received;

                        string query2 = @"UPDATE products 
                                          SET sale_price = @sale, quantity = @quantity 
                                          WHERE product_id = @id;";

                        using (var cmd2 = new MySqlCommand(query2, conn, trans))
                        {
                            cmd2.Parameters.AddWithValue("@sale", b.sale_price);
                            cmd2.Parameters.AddWithValue("@quantity", new_quantity);
                            cmd2.Parameters.AddWithValue("@id", b.product_id);
                            cmd2.ExecuteNonQuery();
                        }

                        // Insert into inventory_log
                        string query3 = @"INSERT INTO inventory_log(product_id, change_type, quantity_change, log_date, remarks)
                                          VALUES(@product_id, @change_type, @change_quantity, @change_date, @remarks);";

                        using (var cmd3 = new MySqlCommand(query3, conn, trans))
                        {
                            cmd3.Parameters.AddWithValue("@product_id", b.product_id);
                            cmd3.Parameters.AddWithValue("@change_type", "purchase"); 
                            cmd3.Parameters.AddWithValue("@change_quantity", b.quantity_received);
                            cmd3.Parameters.AddWithValue("@change_date", DateTime.Now);
                            cmd3.Parameters.AddWithValue("@remarks", $"Batch: {b.batch_name}, Price: {b.cost_price}");
                            cmd3.ExecuteNonQuery();
                        }

                        trans.Commit();
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error in DL: " + e.Message);
            }
        }
        public List<BatchDetails> SearchBatchDetails(string searchText)
        {
            var list = new List<BatchDetails>();
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = @"
                SELECT 
                    bd.batch_detail_id,
                    b.batch_name,
                    p.product_id,
                    p.name AS product_name,
                   
                    bd.cost_price,
                    p.sale_price,
                    bd.quantity_recived
                FROM 
                    batch_details bd
                JOIN 
                    batches b ON bd.batch_id = b.batch_id
                JOIN 
                    products p ON bd.product_id = p.product_id
                WHERE 
                    b.batch_name LIKE @search OR
                    p.name LIKE @search;";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@search", $"%{searchText}%");

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int detailsId = reader.GetInt32("batch_detail_id");
                                string batchName = reader.GetString("batch_name");
                                int productId = reader.GetInt32("product_id");
                                string productName = reader.GetString("product_name");
                                decimal costPrice = reader.GetDecimal("cost_price");
                                decimal salePrice = reader.GetDecimal("sale_price");
                                int quantity = reader.GetInt32("quantity_recived");

                                var item = new BatchDetails(detailsId, batchName, costPrice, salePrice, productId, productName, quantity);
                                list.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error searching batch details: " + ex.Message);
            }

            return list;
        }

        private int oldquantity(int id)
        {
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT quantity FROM products WHERE product_id = @id;";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        object result = cmd.ExecuteScalar();
                        return result != null ? Convert.ToInt32(result) : 0;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error in DL: " + e.Message);
            }
        }
        public List<BatchDetails> GetAllBatchDetails()
        {
            var list = new List<BatchDetails>();
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = @"
                SELECT 
                    bd.batch_detail_id,
                    b.batch_name,
                    p.product_id,
                    p.name AS product_name,
                   
                    bd.cost_price,
                    p.sale_price,
                    bd.quantity_recived
                FROM 
                    batch_details bd
                JOIN 
                    batches b ON bd.batch_id = b.batch_id
                JOIN 
                    products p ON bd.product_id = p.product_id;";

                    using (var cmd = new MySqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int detailsId = reader.GetInt32("batch_detail_id");
                            string batchName = reader.GetString("batch_name");
                            int productId = reader.GetInt32("product_id");
                            string productName = reader.GetString("product_name");
                            decimal costPrice = reader.GetDecimal("cost_price");
                            decimal salePrice = reader.GetDecimal("sale_price");
                            int quantity = reader.GetInt32("quantity_recived");

                            var item = new BatchDetails(detailsId, batchName, costPrice, salePrice, productId, productName, quantity);
                            list.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting batch details: " + ex.Message);
            }

            return list;
        }
        public int getsaleprice(int product_id)
        {
            return DatabaseHelper.Instance.getsaleprice(product_id);
        }
    }
}
