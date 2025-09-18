using System;
using System.Collections.Generic;
using System.Data;
using MedicineShop.Models;
using MySql.Data.MySqlClient;

namespace MedicineShop.DL
{
    public class MedicineDL
    {
        public int AddMedicine(Medicine medicine)
        {
            string query = @"INSERT INTO medicines 
                (name, description, company_id, Category_id, Packing, sale_price) 
                VALUES (@name, @desc, @companyId, @catId, @packing, @price)";

            MySqlParameter[] parameters =
            {
                new MySqlParameter("@name", medicine.Name),
                new MySqlParameter("@desc", medicine.Description),
                new MySqlParameter("@companyId", medicine.CompanyId),
                new MySqlParameter("@catId", medicine.CategoryId),
                new MySqlParameter("@packing", medicine.Packing),
                new MySqlParameter("@price", medicine.SalePrice)
            };

            return DatabaseHelper.Instance.ExecuteNonQuery(query, parameters);
        }

        public int UpdateMedicine(Medicine medicine)
        {
            string query = @"UPDATE medicines 
                SET name=@name, description=@desc, company_id=@companyId, Category_id=@catId, Packing=@packing, sale_price=@price
                WHERE product_id=@id";

            MySqlParameter[] parameters =
            {
                new MySqlParameter("@id", medicine.ProductId),
                new MySqlParameter("@name", medicine.Name),
                new MySqlParameter("@desc", medicine.Description),
                new MySqlParameter("@companyId", medicine.CompanyId),
                new MySqlParameter("@catId", medicine.CategoryId),
                new MySqlParameter("@packing", medicine.Packing),
                new MySqlParameter("@price", medicine.SalePrice)
            };

            return DatabaseHelper.Instance.ExecuteNonQuery(query, parameters);
        }

        public int DeleteMedicine(int id)
        {
            string query = "DELETE FROM medicines WHERE product_id=@id";
            MySqlParameter[] parameters = { new MySqlParameter("@id", id) };

            return DatabaseHelper.Instance.ExecuteNonQuery(query, parameters);
        }

        public DataTable GetAllMedicines()
        {
            string query = @"SELECT m.product_id, m.name, m.description, c.company_name, cat.category_name, m.Packing, m.sale_price, m.company_id, m.Category_id
                             FROM medicines m
                             JOIN company c ON m.company_id = c.company_id
                             JOIN categories cat ON m.Category_id = cat.category_id";

            return DatabaseHelper.Instance.ExecuteDataTable(query);
        }

        public DataTable SearchMedicines(string keyword)
        {
            string query = @"SELECT m.product_id, m.name, m.description, c.company_name, cat.category_name, 
                            m.Packing, m.sale_price, m.company_id, m.Category_id
                     FROM medicines m
                     JOIN company c ON m.company_id = c.company_id
                     JOIN categories cat ON m.Category_id = cat.category_id
                     WHERE m.name LIKE @keyword 
                        OR c.company_name LIKE @keyword
                        OR cat.category_name LIKE @keyword";

            MySqlParameter[] parameters =
            {
        new MySqlParameter("@keyword", "%" + keyword + "%")
    };

            return DatabaseHelper.Instance.ExecuteDataTable(query, parameters);
        }


        public DataTable GetCompanies(string keyword = "")
        {
            string query = "SELECT company_id, company_name FROM company WHERE company_name LIKE @keyword";
            MySqlParameter[] parameters = { new MySqlParameter("@keyword", "%" + keyword + "%") };
            return DatabaseHelper.Instance.ExecuteDataTable(query, parameters);
        }

        public DataTable GetCategories(string keyword = "")
        {
            string query = "SELECT category_id, category_name FROM categories WHERE category_name LIKE @keyword";
            MySqlParameter[] parameters = { new MySqlParameter("@keyword", "%" + keyword + "%") };
            return DatabaseHelper.Instance.ExecuteDataTable(query, parameters);
        }
    }
}
