using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using fertilizesop.BL.Models;
using fertilizesop.Interfaces.DLinterfaces;
using KIMS;
using MySql.Data.MySqlClient;
using Mysqlx;

namespace fertilizesop.DL
{
    internal class Supplierdl : Isupplierdl
    {
        public bool addsupplier(Suppliers s)
        {
            try
            {
                using(var con = DatabaseHelper.Instance.GetConnection())
                {
                    con.Open();
                    string query = "insert into suppliers (name , phone, address) values (@name, @phone, @address)";
                    using(var cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("name", s.first_Name);
                        cmd.Parameters.AddWithValue("phone", s.phonenumber);
                        cmd.Parameters.AddWithValue("address", s.Address);
                        int id = cmd.ExecuteNonQuery();
                        return id > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding supplier " );
            }
        }

        public bool deletesupplier(int s)
        {
            try
            {
                using (var con = DatabaseHelper.Instance.GetConnection())
                {
                    con.Open();
                    string query = "delete from suppliers where supplier_id = @s";
                    using (var cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("s", s);
                        int executedid = cmd.ExecuteNonQuery();
                        return executedid > 0;
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception("DL error occurred while deleting supplier: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in deleting the supplier in the datah layer" );
            }
        }

        public List<Suppliers> getsupplier()
        {
            List<Suppliers> suppliers = new List<Suppliers>();
            try
            {
                using(var con = DatabaseHelper.Instance.GetConnection())
                {
                    con.Open();
                    string query = "select * from suppliers";
                    using(var cmd = new MySqlCommand(query , con))
                    {
                        using(var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var supplier = new Suppliers(
                                reader.GetInt32("supplier_id"),
                                reader.GetString("name"),
                                reader.IsDBNull(reader.GetOrdinal("phone")) ? "" : reader.GetString("phone"),
                                reader.IsDBNull(reader.GetOrdinal("address")) ? "" : reader.GetString("address")    
                                );
                                suppliers.Add(supplier);                            
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return suppliers;
        }

        public List<Suppliers> searchsupplier(string text)
        {
            List<Suppliers> suppliers = new List<Suppliers>();
            try
            {
                using (var con = DatabaseHelper.Instance.GetConnection())
                {
                    con.Open();
                    string query = "select * from suppliers where name like @text";
                    using (var cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("text", $"{text}%");
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var supplier = new Suppliers(
                                    reader.GetInt32("supplier_id"),
                                    reader.GetString("name"),
                                    reader.IsDBNull(reader.GetOrdinal("address")) ? "" : reader.GetString("address"),
                                    reader.IsDBNull(reader.GetOrdinal("phone")) ? "" : reader.GetString("phone")
                                    );
                                suppliers.Add(supplier);
                            }
                        }
                    }
                }
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
               return suppliers ;
        }

        public bool updatesupplier(Suppliers s)
        {
            try
            {
                using (var con = DatabaseHelper.Instance.GetConnection())
                {
                    con.Open();
                    string query = "update suppliers set name = @name , phone = @phone , address = @address where supplier_id = @id";
                    using (var cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("name", s.first_Name);
                        cmd.Parameters.AddWithValue("phone", string.IsNullOrEmpty(s.phonenumber) ? (object)DBNull.Value : s.phonenumber);
                        cmd.Parameters.AddWithValue("address", string.IsNullOrEmpty(s.Address) ? (object)DBNull.Value : s.Address);
                        cmd.Parameters.AddWithValue("Id", s.Id);
                        int executeid = cmd.ExecuteNonQuery();
                        return executeid > 0;
                    }
                }
            }
            catch(Exception e)
            {
                throw new Exception("erroe in supplierdl " +e.Message);
            }
            throw new NotImplementedException();
        }
    }
}
