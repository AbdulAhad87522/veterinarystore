using fertilizesop.BL.Bl;
using fertilizesop.BL.Models;
using KIMS;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace fertilizesop.DL
{
    public class ProductsDl : IProductsDl
    {
        public bool Addproduct(Products p)
        {
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    using (var tx = conn.BeginTransaction())
                    {
                        string insertProductQuery = "INSERT INTO products(name, sale_price, description, quantity) " +
                                                    "VALUES(@name, @sale, @descp, @quantity);";

                        using (var cmd = new MySqlCommand(insertProductQuery, conn, tx))
                        {
                            cmd.Parameters.AddWithValue("@name", p.Name);
                            cmd.Parameters.AddWithValue("@sale", p.Price);
                            cmd.Parameters.AddWithValue("@descp", p.Description);
                            cmd.Parameters.AddWithValue("@quantity", p.quantity);
                            int rows = cmd.ExecuteNonQuery();

                            if (rows == 0)
                            {
                                tx.Rollback();
                                return false;
                            }
                        }

                        // Get the last inserted product ID
                        // Get the last inserted product ID
                        int productId = Convert.ToInt32(new MySqlCommand("SELECT LAST_INSERT_ID();", conn, tx).ExecuteScalar());

                        // Insert into inventory_log
                        string insertLogQuery = @"INSERT INTO inventory_log (product_id, change_type, quantity_change, log_date, remarks)
                                          VALUES (@pid, 'manual_adjustment', @qty, NOW(), 'Manual adjustment by the user');";

                        using (var logCmd = new MySqlCommand(insertLogQuery, conn, tx))
                        {
                            logCmd.Parameters.AddWithValue("@pid", productId);
                            logCmd.Parameters.AddWithValue("@qty", p.quantity);
                            logCmd.ExecuteNonQuery();
                        }

                        tx.Commit();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding product with log: " + ex.Message, ex);
            }
        }

        public List<Products> GetProducts()
        {
            var list = new List<Products>();
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = "select * from products;";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                int id = reader.GetInt32("product_id");
                                string descp = reader.IsDBNull(reader.GetOrdinal("description")) ? "" : reader.GetString("description");

                                string name = reader.GetString("name");
                                decimal sale_price = reader.GetDecimal("sale_price");
                                int quantity = reader.GetInt32("quantity");
                                var products = new Products(id, name, descp, sale_price, quantity);
                                list.Add(products);


                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting products " + ex.Message);
            }
            return list;
        }
        public List<Products> searchproducts(string text)
        {
            var list = new List<Products>();
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = "select * from products where name like @name;";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            cmd.Parameters.AddWithValue("@name", $"{text}%");

                            while (reader.Read())
                            {

                                int id = reader.GetInt32("product_id");
                                string descp = reader.IsDBNull(reader.GetOrdinal("description")) ? "" : reader.GetString("description");

                                string name = reader.GetString("name");
                                decimal sale_price = reader.GetDecimal("sale_price");
                                int quantity = reader.GetInt32("quantity");
                                var products = new Products(id, name, descp, sale_price, quantity);
                                list.Add(products);


                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting products " + ex.Message);
            }
            return list;
        }
        public bool update(Products c)
        {
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    using (var tx = conn.BeginTransaction())
                    {
                        // Get current quantity before update
                        int oldQuantity = 0;
                        string getQtyQuery = "SELECT quantity FROM products WHERE product_id = @id;";
                        using (var getQtyCmd = new MySqlCommand(getQtyQuery, conn, tx))
                        {
                            getQtyCmd.Parameters.AddWithValue("@id", c.Id);
                            var result = getQtyCmd.ExecuteScalar();
                            if (result != null)
                                oldQuantity = Convert.ToInt32(result);
                        }

                        // Update product
                        string updateQuery = @"UPDATE products 
                                       SET name = @name, description = @descp, sale_price = @price, quantity = @quantity 
                                       WHERE product_id = @id;";
                        using (var cmd = new MySqlCommand(updateQuery, conn, tx))
                        {
                            cmd.Parameters.AddWithValue("@name", c.Name);
                            cmd.Parameters.AddWithValue("@descp", c.Description);
                            cmd.Parameters.AddWithValue("@price", c.Price);
                            cmd.Parameters.AddWithValue("@quantity", c.quantity);
                            cmd.Parameters.AddWithValue("@id", c.Id);

                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected == 0)
                            {
                                tx.Rollback();
                                return false;
                            }
                        }

                        int quantityChange = c.quantity - oldQuantity;
                        if (quantityChange != 0)
                        {
                            string insertLogQuery = @"INSERT INTO inventory_log (product_id, change_type, quantity_change, log_date, remarks)
                                              VALUES (@pid, 'manual_adjustment', @qtyChange, NOW(), 'Manual adjustment by the user');";

                            using (var logCmd = new MySqlCommand(insertLogQuery, conn, tx))
                            {
                                logCmd.Parameters.AddWithValue("@pid", c.Id);
                                logCmd.Parameters.AddWithValue("@qtyChange", quantityChange);
                                logCmd.ExecuteNonQuery();
                            }
                        }

                        tx.Commit();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating product with log: " + ex.Message, ex);
            }
        }

    }
}
