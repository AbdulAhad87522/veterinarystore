﻿using fertilizesop.BL.Models;
using fertilizesop.DL;
using fertilizesop.Interfaces.BLInterfaces;
using Mysqlx.Session;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace fertilizesop.BL.Bl
{
    public class CustomerBl : ICustomerBl
    {
        private readonly ICustomerDl idl;

        public CustomerBl(ICustomerDl idl)
        {
            this.idl = idl;
        }

        public bool Addcustomer(Customers s)
        {
            try
            {
                ValidateCustomer(s);
                bool result= idl.Addcustomer(s);
                if (result)
                {
                    MySqlBackupHelper.CreateBackup();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Business Layer (Addcustomer): " + ex.Message, ex);
            }
        }

        public List<Isupplier> getcustomers()
        {
            try
            {
                return idl.getcustomers();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Business Layer (getcustomers): " + ex.Message, ex);
            }
        }

        public List<Isupplier> searchcustomers(string text)
        {
            try
            {


                return idl.searchcustomers(text);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Business Layer (searchcustomers): " + ex.Message, ex);
            }
        }

        public bool update(Customers c)
        {
            try
            {
                ValidateCustomer(c);

                if (c.Id <= 0)
                    throw new ArgumentException("Invalid customer ID for update.");

                bool result = idl.update(c);
                if (result)
                {
                    MySqlBackupHelper.CreateBackup();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Business Layer (update): " + ex.Message, ex);
            }
        }

        private void ValidateCustomer(Customers c)
        {
            if (c == null)
                throw new ArgumentNullException("Customer cannot be null.");

            if (string.IsNullOrWhiteSpace(c.first_Name))
                throw new ArgumentException("First name is required.");

            if (string.IsNullOrWhiteSpace(c.last_name))
                throw new ArgumentException("Last name is required.");

            if (string.IsNullOrWhiteSpace(c.phonenumber))
                throw new ArgumentException("Phone number is required.");

            //if (!Regex.IsMatch(c.phonenumber, @"^\+?\d{7,15}$"))
            //    throw new ArgumentException("Invalid phone number format.");

            if (c.first_Name.Length > 50)
                throw new ArgumentException("First name is too long.");

            if (c.last_name.Length > 50)
                throw new ArgumentException("Last name is too long.");

            if (!string.IsNullOrEmpty(c.Address) && c.Address.Length > 255)
                throw new ArgumentException("Address is too long.");
        }
    }
}