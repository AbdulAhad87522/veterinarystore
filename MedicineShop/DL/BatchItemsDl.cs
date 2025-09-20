using MedicineShop.BL.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace MedicineShop.DL
{
    public class BatchItemsDl
    {
        // ✅ Add Batch Item
        public bool AddBatchItem(BatchItems b)
        {
            //int batch_id = DatabaseHelper.Instance.getbatchid(b.batchname);

            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            string insertQuery = @"INSERT INTO batch_items 
                                (purchase_batch_id, product_id, quantity_received, purchase_price, expiry_date) 
                                VALUES (@BatchID, @MedicineID, @Quantity, @PurchasePrice, @ExpiryDate)";

                            using (var cmd = new MySqlCommand(insertQuery, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@BatchID",b.BatchID);
                                cmd.Parameters.AddWithValue("@MedicineID", b.MedicineID);
                                cmd.Parameters.AddWithValue("@Quantity", b.Quantity);
                                cmd.Parameters.AddWithValue("@PurchasePrice", b.PurchasePrice);
                                cmd.Parameters.AddWithValue("@ExpiryDate", b.ExpiryDate);
                                cmd.ExecuteNonQuery();
                            }

                            // update sale price in medicines
                            string updateMedicineQuery = "UPDATE medicines SET sale_price = @SalePrice WHERE product_id = @MedicineID";
                            using (var updateCmd = new MySqlCommand(updateMedicineQuery, conn, transaction))
                            {
                                updateCmd.Parameters.AddWithValue("@SalePrice", b.SalePrice);
                                updateCmd.Parameters.AddWithValue("@MedicineID", b.MedicineID);
                                updateCmd.ExecuteNonQuery();
                            }

                            transaction.Commit();
                            return true;
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error while adding batch item.", ex);
            }
        }

        // ✅ Get all batch items
        public List<BatchItems> GetAllBatchItems()
        {
            var items = new List<BatchItems>();
            try
            {
                string query = @"
    SELECT 
        bi.batch_item_id AS BatchItemID, 
        bi.purchase_batch_id AS BatchID, 
        bi.product_id AS ProductID,
        b.BatchName, 
        bi.quantity_received AS QuantityReceived, 
        bi.purchase_price AS PurchasePrice, 
        bi.expiry_date AS ExpiryDate, 
        m.`name` AS MedicineName, 
        m.sale_price AS SalePrice
    FROM batch_items bi 
    JOIN medicines m ON bi.product_id = m.product_id
    JOIN purchase_batches b ON b.purchase_batch_id = bi.purchase_batch_id";


                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            items.Add(new BatchItems
                            {
                                BatchItemID = reader.GetInt32("BatchItemID"),
                                BatchID = reader.GetInt32("BatchID"),
                                MedicineID = reader.GetInt32("ProductID"),
                                batchname = reader.GetString("BatchName"),
                                MedicineName = reader.GetString("MedicineName"),
                                Quantity = reader.GetInt32("QuantityReceived"),
                                PurchasePrice = reader.GetDecimal("PurchasePrice"),
                                SalePrice = reader.GetDecimal("SalePrice"),
                                ExpiryDate = reader.GetDateTime("ExpiryDate"),
                          
                            });


                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error while getting batch items.", ex);
            }
            return items;
        }

        // ✅ Get batch item by ID
        public List<BatchItems> GetBatchItemById(int id)
        {
            List<BatchItems> list= new List<BatchItems>();
            try
            {
                string query = @"SELECT bi.batch_item_id, bi.purchase_batch_id, bi.product_id, 
                                bi.quantity_received, bi.purchase_price, bi.expiry_date, m.name AS MedicineName, m.sale_price
                                FROM batch_items bi 
                                JOIN medicines m ON bi.product_id = m.medicine_id
                                WHERE bi.batch_item_id = @ID";

                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var item = new BatchItems
                                {
                                    BatchItemID = reader.GetInt32("batch_item_id"),
                                    BatchID = reader.GetInt32("purchase_batch_id"),
                                    MedicineID = reader.GetInt32("product_id"),
                                    Quantity = reader.GetInt32("quantity_received"),
                                    PurchasePrice = reader.GetDecimal("purchase_price"),
                                    SalePrice = reader.GetDecimal("sale_price"),
                                    ExpiryDate = reader.GetDateTime("expiry_date"),
                                    MedicineName = reader.GetString("MedicineName")
                                };
                                list.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error while getting batch item by ID.", ex);
            }
            return list;
        }

        // ✅ Update batch item
        public bool UpdateBatchItem(BatchItems b)
        {
            try
            {
                string query = @"UPDATE batch_items 
                                SET product_id = @MedicineID, quantity_received = @Quantity, 
                                purchase_price = @PurchasePrice, expiry_date = @ExpiryDate 
                                WHERE batch_item_id = @ID";

                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MedicineID", b.MedicineID);
                        cmd.Parameters.AddWithValue("@Quantity", b.Quantity);
                        cmd.Parameters.AddWithValue("@PurchasePrice", b.PurchasePrice);
                        cmd.Parameters.AddWithValue("@ExpiryDate", b.ExpiryDate);
                        cmd.Parameters.AddWithValue("@ID", b.BatchItemID);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error while updating batch item.", ex);
            }
        }

        // ✅ Delete batch item
        public bool DeleteBatchItem(int id)
        {
            try
            {
                string query = "DELETE FROM batch_items WHERE batch_item_id = @ID";

                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error while deleting batch item.", ex);
            }
        }

        // ✅ Search by medicine name (LIKE query)
        public List<BatchItems> SearchBatchItems(string searchTerm)
        {
            var items = new List<BatchItems>();
            try
            {
                string query = @"SELECT bi.batch_item_id, bi.purchase_batch_id, bi.product_id, 
                                bi.quantity_received, bi.purchase_price, bi.expiry_date, m.name AS MedicineName, m.sale_price
                                FROM batch_items bi 
                                JOIN medicines m ON bi.product_id = m.medicine_id
                                WHERE m.name LIKE @search";

                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@search", $"%{searchTerm}%");
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                items.Add(new BatchItems
                                {
                                    BatchItemID = reader.GetInt32("batch_item_id"),
                                    BatchID = reader.GetInt32("purchase_batch_id"),
                                    MedicineID = reader.GetInt32("product_id"),
                                    Quantity = reader.GetInt32("quantity_received"),
                                    PurchasePrice = reader.GetDecimal("purchase_price"),
                                    SalePrice = reader.GetDecimal("sale_price"),
                                    ExpiryDate = reader.GetDateTime("expiry_date"),
                                    MedicineName = reader.GetString("MedicineName")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error while searching batch items.", ex);
            }
            return items;
        }
    }
}
