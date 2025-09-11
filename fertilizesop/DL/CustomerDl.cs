using fertilizesop.BL.Models;
using KIMS;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace fertilizesop.DL
{
    public class CustomerDl : ICustomerDl
    {
        public bool Addcustomer(Customers s)
        {
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = "INSERT INTO customers(first_name, last_name, phone, address) VALUES (@first_name, @last_name, @phone, @address);";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@first_name", s.first_Name);
                        cmd.Parameters.AddWithValue("@last_name", s.last_name);
                        cmd.Parameters.AddWithValue("@phone", s.phonenumber);
                        cmd.Parameters.AddWithValue("@address", string.IsNullOrEmpty(s.Address) ? (object)DBNull.Value : s.Address);

                        int rowsaffected = cmd.ExecuteNonQuery();
                        return rowsaffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in DL: " + ex.Message, ex);
            }
        }

        public List<Isupplier> getcustomers()
        {
            var customers = new List<Isupplier>();
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT * FROM customers;";
                    using (var cmd = new MySqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32("customer_id");
                            string first_name = reader.IsDBNull(reader.GetOrdinal("first_name")) ? "" : reader.GetString("first_name");
                            string last_name = reader.IsDBNull(reader.GetOrdinal("last_name")) ? "" : reader.GetString("last_name");
                            string phone = reader.IsDBNull(reader.GetOrdinal("phone")) ? "" : reader.GetString("phone");
                            string address = reader.IsDBNull(reader.GetOrdinal("address")) ? "" : reader.GetString("address");

                            Customers c = new Customers(id, first_name, phone, address, last_name);
                            customers.Add(c);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving customers: " + ex.Message, ex);
            }
            return customers;
        }

        public bool update(Customers c)
        {
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = "UPDATE customers SET first_name = @first_name, last_name = @last_name, phone = @phone, address = @address WHERE customer_id = @id;";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@first_name", c.first_Name);
                        cmd.Parameters.AddWithValue("@last_name", c.last_name);
                        cmd.Parameters.AddWithValue("@phone", c.phonenumber);
                        cmd.Parameters.AddWithValue("@address", string.IsNullOrEmpty(c.Address) ? (object)DBNull.Value : c.Address);
                        cmd.Parameters.AddWithValue("@id", c.Id);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception("Database error occurred while updating customer: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating customer: " + ex.Message, ex);
            }
        }

        public List<Isupplier> searchcustomers(string text)
        {
            var customers = new List<Isupplier>();
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = @"
                        SELECT * FROM customers 
                        WHERE first_name LIKE @keyword 
                           OR last_name LIKE @keyword 
                           OR phone LIKE @keyword;";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@keyword", $"{text}%");
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32("customer_id");
                                string first_name = reader.IsDBNull(reader.GetOrdinal("first_name")) ? "" : reader.GetString("first_name");
                                string last_name = reader.IsDBNull(reader.GetOrdinal("last_name")) ? "" : reader.GetString("last_name");
                                string phone = reader.IsDBNull(reader.GetOrdinal("phone")) ? "" : reader.GetString("phone");
                                string address = reader.IsDBNull(reader.GetOrdinal("address")) ? "" : reader.GetString("address");

                                Customers c = new Customers(id, first_name, phone, address, last_name);
                                customers.Add(c);
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception("Error searching customers: " + ex.Message, ex);
            }
            return customers;
        }
    }
}
