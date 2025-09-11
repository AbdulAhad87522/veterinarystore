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
    public class BatchesDl : IBatchesDl
    {
        public List<string >getsuppliernames(string text)
        {
            return DatabaseHelper.Instance.GetSuppliers(text);
        }
        public bool addbatches(Batches b)
        {
            try
            {
                int supplierid = DatabaseHelper.Instance.getsuppierid(b.supplier_name);
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = "insert into batches (batch_name,supplier_id,recieved_date) values(@name,@id,@date);";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", b.batch_name);
                        cmd.Parameters.AddWithValue("@id", supplierid);
                        cmd.Parameters.AddWithValue("@date", b.received_date);
                        int rowsaffected = cmd.ExecuteNonQuery();
                        return rowsaffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error in dl " + ex.Message);
            }
        }
        public bool UpdateBatch(Batches b)
        {
            try
            {
                int supplierid = DatabaseHelper.Instance.getsuppierid(b.supplier_name);
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = "UPDATE batches SET batch_name = @name, supplier_id = @supplierId, recieved_date = @date WHERE batch_id = @id;";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", b.batch_name);
                        cmd.Parameters.AddWithValue("@supplierId", supplierid);
                        cmd.Parameters.AddWithValue("@date", b.received_date);
                        cmd.Parameters.AddWithValue("@id", b.batch_id);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating batch: " + ex.Message);
            }
        }
        public List<Batches> GetAllBatches()
        {
            var list = new List<Batches>();
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT b.batch_id, b.batch_name, s.name, s.phone, b.recieved_date ,b.supplier_id
                             FROM batches b 
                             JOIN suppliers s ON b.supplier_id = s.supplier_id;";
                    using (var cmd = new MySqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {


                            int id = reader.GetInt32("batch_id");
                            string batch_name = reader.GetString("batch_name");
                            string supplier_name = reader.GetString("name");
                            string supplier_phone = reader.GetString("phone");
                            DateTime received_date = reader.GetDateTime("recieved_date");
                            int supplier_id = reader.GetInt32("supplier_id");
                            var batches = new Batches(id, batch_name, received_date, supplier_name, supplier_phone,supplier_id);
                            list.Add(batches);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching batches: " + ex.Message);
            }

            return list;
        }
        public List<Batches> SearchBatches(string keyword)
        {
            var list = new List<Batches>();
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = @"
                SELECT b.batch_id, b.batch_name, s.name AS supplier_name, s.phone, b.recieved_date,b.supplier_id
                FROM batches b
                JOIN suppliers s ON b.supplier_id = s.supplier_id
                WHERE b.batch_name LIKE @kw 
                   OR s.name LIKE @kw 
                   OR s.phone LIKE @kw;";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@kw", "%" + keyword + "%");

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32("batch_id");
                                string batch_name = reader.GetString("batch_name");
                                string supplier_name = reader.GetString("supplier_name");
                                string supplier_phone = reader.GetString("phone");
                                DateTime received_date = reader.GetDateTime("recieved_date");
                                int supplier_id = reader.GetInt32("supplier_id");

                                var batch = new Batches(id, batch_name, received_date, supplier_name, supplier_phone, supplier_id);
                                list.Add(batch);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error searching batches: " + ex.Message);
            }

            return list;
        }

    }
}
