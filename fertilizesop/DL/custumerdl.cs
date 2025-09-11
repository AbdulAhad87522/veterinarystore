//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using fertilizesop.BL.Models;
//using fertilizesop.BL.Models.persons;
//using fertilizesop.Interfaces.DLInterfaces;
//using KIMS;
//using MySql.Data.MySqlClient;

//namespace fertilizesop.DL
//{
//    internal class custumerdl : ICustomerDl
//    {
//        public bool addcustomer(Ipersons p)
//        {
//            try
//            {
//                Customer customer = p as Customer;
//                if(p == null)
//                {
//                    throw new ArgumentNullException("Expected Customer object");
//                }
//                using(var con = DatabaseHelper.Instance.GetConnection())
//                {
//                    con.Open();
//                    //string query = "insert into customers (first_name , last_name, type, email , address, phone) values (@first , @last, @type, @email, @address, @phone)";
//                    //using (var cmd = new MySqlCommand(query, con))
//                    //{
//                    //    cmd.Parameters.AddWithValue("@first", customer.first_name);
//                    //    cmd.Parameters.AddWithValue("@last", customer.last_name);
//                    //    cmd.Parameters.AddWithValue("@type", customer.type);
//                    //    cmd.Parameters.AddWithValue("email", customer.email);
//                    //    cmd.Parameters.AddWithValue("address", customer.address);
//                    //    cmd.Parameters.AddWithValue("phone", customer.phone);

//                    //    int affectedrows = cmd.ExecuteNonQuery();
//                    //    return affectedrows > 0;
//                    //}
//                    //
//                    string query = "INSERT INTO customers (first_name , last_name, type, email , address, phone) VALUES (@first , @last, @type, @email, @address, @phone)";
//                    using (var cmd = new MySqlCommand(query, con))
//                    {
//                        cmd.Parameters.AddWithValue("@first", customer.first_name);
//                        cmd.Parameters.AddWithValue("@last", customer.last_name);
//                        cmd.Parameters.AddWithValue("@type", customer.type);
//                        cmd.Parameters.AddWithValue("@email", customer.email);
//                        cmd.Parameters.AddWithValue("@address", customer.address);
//                        cmd.Parameters.AddWithValue("@phone", customer.phone);

//                        cmd.ExecuteNonQuery();

//                        // Get last inserted ID
//                        cmd.CommandText = "SELECT LAST_INSERT_ID();";
//                        int insertedId = Convert.ToInt32(cmd.ExecuteScalar());

//                        // Assign the ID back to the object (if needed)
//                        customer.id = insertedId;

//                        return true;
//                    }

//                }

//            }
//            catch(MySqlException e)
//            {
//                throw new Exception("Database error occured while saving the customer" + e.Message);
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("Error from the database, while adding the customer" + ex.Message);
//            }
            
//        }

//        public bool deletecustomer(Ipersons p)
//        {
//            throw new NotImplementedException();
//        }

//        public List<Ipersons> getcustomers()
//        {
//            List<Ipersons> customers = new List<Ipersons>();
//            try
//            {
//                using (var con = DatabaseHelper.Instance.GetConnection())
//                {
//                    con.Open();
//                    string query = "Select * from customers";
//                    using (var cmd = new MySqlCommand(query, con))
//                    {
//                        using (var reader = cmd.ExecuteReader())
//                        {
//                            while (reader.Read())
//                            {
//                                Ipersons customer = new Customer
//                                (
//                                    reader.GetInt32("customer_id"),
//                                    reader.GetString("first_name"),
//                                    reader.IsDBNull(reader.GetOrdinal("last_name")) ? "" : reader.GetString("last_name"),
//                                    reader.GetString("type"),
//                                    reader.IsDBNull(reader.GetOrdinal("address")) ? "" : reader.GetString("address"),
//                                    reader.IsDBNull(reader.GetOrdinal("email")) ? "" : reader.GetString("emmail"),
//                                    reader.IsDBNull(reader.GetOrdinal("contact")) ? "" : reader.GetString("contact")
//                                    );
//                                customers.Add(customer);
//                            }
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("error loading customers from database + ex.message");
//            }
//            return customers;
//        }

//        public List<Ipersons> searchcustomer(string s)
//        {
//            List<Ipersons> customers = new List<Ipersons>();
//            try
//            {
//                using(var con = DatabaseHelper.Instance.GetConnection())
//                {
//                    string query = "select * from customers where first_name like @s OR last_name like @s OR type like @type";   
//                    con.Open(); 

//                    using(var cmd = new MySqlCommand(query,con) )
//                    {
//                        cmd.Parameters.AddWithValue("@s", $"{s}");
//                        using (var reader = cmd.ExecuteReader())
//                        {
//                            while (reader.Read())
//                            {
//                                var customer = new Customer(
//                                reader.GetInt32("customer_d"),
//                                reader.GetString("first_name"),
//                                reader.IsDBNull(reader.GetOrdinal("last_name")) ? "" : reader.GetString("last_name"),
//                                reader.GetString("type"),
//                                reader.IsDBNull(reader.GetOrdinal("address")) ? "" : reader.GetString("address"),
//                                reader.IsDBNull(reader.GetOrdinal("email")) ? "" : reader.GetString("email"),
//                                reader.IsDBNull(reader.GetOrdinal("contact")) ? "" : reader.GetString("contact")
//                                ); 
//                                customers.Add(customer);
//                            }
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Error retrieving customers from the database" + ex.Message);
//            }
//            return customers;
//            throw new NotImplementedException();
//        }

//        public bool updatecustomer(Ipersons p)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
