using MedicineShop.BL;
using MedicineShop.BL.Models;
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace MedicineShop.DL
{
    public class BatchesDl
    {
        public bool AddBatches(Batches b, List<BatchItems> itemsList)
        {
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Insert into purchase_batches
                            string insertBatchQuery = @"
                                INSERT INTO purchase_batches 
                                (purchase_date, BatchName, TotalPrice, PaidAmount, company_id) 
                                VALUES (@date, @BatchName, @TotalPrice, @PaidAmount, @CompanyID);";

                            var batchParams = new MySqlParameter[]
                            {
                                new MySqlParameter("@date", DateTime.Now),
                                new MySqlParameter("@BatchName", b.BatchName),
                                new MySqlParameter("@TotalPrice", b.TotalPrice),
                                new MySqlParameter("@PaidAmount", b.PaidAmount),
                                new MySqlParameter("@CompanyID", b.company_id)
                            };

                            DatabaseHelper.Instance.ExecuteNonQueryTransaction(insertBatchQuery, batchParams, transaction);

                            int batchId = DatabaseHelper.Instance.GetLastInsertId(transaction,conn);

                            // Insert all batch items
                            string insertBatchItemQuery = @"
                                INSERT INTO batch_items 
                                (purchase_batch_id, product_id, quantity_received, purchase_price, expiry_date) 
                                VALUES (@BatchID, @MedicineID, @Quantity, @PurchasePrice, @ExpiryDate);";

                            foreach (var item in itemsList)
                            {
                                var batchItemParams = new MySqlParameter[]
                                {
                                    new MySqlParameter("@BatchID", batchId),
                                    new MySqlParameter("@MedicineID", item.MedicineID),
                                    new MySqlParameter("@Quantity", item.Quantity),
                                    new MySqlParameter("@PurchasePrice", item.Purcahseprice),
                                    new MySqlParameter("@ExpiryDate", item.ExpiryDate)
                                };

                                DatabaseHelper.Instance.ExecuteNonQueryTransaction(insertBatchItemQuery, batchItemParams, transaction);

                                // Update sale price per product (if needed)
                                string updateSalePriceQuery = "UPDATE medicines SET sale_price = @saleprice WHERE product_id = @prodid;";
                                var updateParams = new MySqlParameter[]
                                {
                                    new MySqlParameter("@saleprice", item.SalePrice),
                                    new MySqlParameter("@prodid", item.MedicineID)
                                };
                                DatabaseHelper.Instance.ExecuteNonQueryTransaction(updateSalePriceQuery, updateParams, transaction);

                                // Optional: insert into stock_log for tracking
                                string insertStockLogQuery = @"
                                    INSERT INTO stock_log (batch_id, change_type, quantity_change, remarks) 
                                    VALUES (@BatchID, 'PURCHASE', @Quantity, 'New stock added');";
                                var logParams = new MySqlParameter[]
                                {
                                    new MySqlParameter("@BatchID", batchId),
                                    new MySqlParameter("@Quantity", item.Quantity)
                                };
                                DatabaseHelper.Instance.ExecuteNonQueryTransaction(insertStockLogQuery, logParams, transaction);
                            }

                            transaction.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            Console.WriteLine("Transaction failed: " + ex.ToString());
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Connection error: " + ex.ToString());
                return false;
            }
        }
        public decimal SalePrice(int product_id)
        {
            return DatabaseHelper.Instance.getsaleprice(product_id);
        }   
    }
}
