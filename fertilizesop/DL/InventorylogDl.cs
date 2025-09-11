using fertilizesop.BL.Models;
using KIMS;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace fertilizesop.DL
{
    public class InventorylogDl : IInventorylogDl
    {
        public List<inventorylog> getlog()
        {
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = @"
                        SELECT p.name, i.quantity_change, i.log_date, i.change_type, i.remarks 
                        FROM inventory_log i 
                        JOIN products p ON p.product_id = i.product_id;";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        var list = new List<inventorylog>();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string name = reader.IsDBNull(reader.GetOrdinal("name")) ? "" : reader.GetString("name");
                                int quantityChange = reader.GetInt32("quantity_change");
                                DateTime logDate = reader.GetDateTime("log_date");
                                string changeType = reader.IsDBNull(reader.GetOrdinal("change_type")) ? "" : reader.GetString("change_type");
                                string remarks = reader.IsDBNull(reader.GetOrdinal("remarks")) ? "" : reader.GetString("remarks");

                                var log = new inventorylog(name, quantityChange, logDate, changeType, remarks);
                                list.Add(log);
                            }
                        }
                        return list;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message, ex);
            }
        }

        public List<inventorylog> getlog(string searchTerm)
        {
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = @"
                        SELECT p.name, i.quantity_change, i.log_date, i.change_type, i.remarks 
                        FROM inventory_log i 
                        JOIN products p ON p.product_id = i.product_id 
                        WHERE 
                            p.name LIKE @search OR 
                            i.change_type LIKE @search OR 
                            DATE_FORMAT(i.log_date, '%Y-%m-%d') LIKE @search;";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@search", "%" + searchTerm + "%");

                        var list = new List<inventorylog>();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string name = reader.IsDBNull(reader.GetOrdinal("name")) ? "" : reader.GetString("name");
                                int quantityChange = reader.GetInt32("quantity_change");
                                DateTime logDate = reader.GetDateTime("log_date");
                                string changeType = reader.IsDBNull(reader.GetOrdinal("change_type")) ? "" : reader.GetString("change_type");
                                string remarks = reader.IsDBNull(reader.GetOrdinal("remarks")) ? "" : reader.GetString("remarks");

                                var log = new inventorylog(name, quantityChange, logDate, changeType, remarks);
                                list.Add(log);
                            }
                        }
                        return list;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message, ex);
            }
        }
    }
}
