using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using KIMS;
using MedicineShop;
using MySql.Data.MySqlClient;

namespace fertilizesop.DL
{
    internal class CustomerBill_SpecificProductsDL
    {
        private readonly DatabaseHelper _dbHelper;

        public CustomerBill_SpecificProductsDL()
        {
            _dbHelper = DatabaseHelper.Instance;
        }

        public DataTable GetBillDetails(int billId)
        {
            DataTable dt = new DataTable();

            try
            {
                string query = @"
                        SELECT 
                    m.name AS ProductName,
                    m.company as Company
                    cbd.quantity,
                    m.sale_price AS UnitPrice,
                    (p.sale_price * cbd.quantity) AS TotalPrice,
                    cbd.discount,
                    cbd.status
                    from sale_items cbd
                JOIN 
                    batch_items bi ON cbd.batch_item_id = p.batch_item_id
                JOIN
                    medicines m on p.product_id = bi.product_id
                JOIN 
                    company c on c.company_id = m.company_id
                WHERE 
                    cbd.sale_id = @billId;";


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

        public DataTable GetBillSummary(int billId)
        {
            DataTable dt = new DataTable();

            try
            {
                string query = @"
                    SELECT 
                        cb.sale_id,
                        CONCAT(c.first_name, ' ', c.last_name) AS CustomerName,
                        cb.sale_date,
                        cb.total_amount AS TotalAmount,
                        cb.paid_amount AS PaidAmount,
                        (cb.total_amount - IFNULL(cb.paid_amount, 0)) AS PendingAmount,
                    FROM 
                        sales cb    
                    JOIN 
                        customers c ON cb.customer_id = c.customer_id
                    WHERE 
                        cb.sale_id = @billId";

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
                throw new Exception("Error retrieving bill summary: " + ex.Message);
            }

            return dt;
        }
    }
}
