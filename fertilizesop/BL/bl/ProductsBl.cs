using fertilizesop.BL.Models;
using fertilizesop.DL;
using fertilizesop.Interfaces.BLInterfaces;
using Mysqlx.Session;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fertilizesop.BL.Bl
{
    public class ProductsBl : IProductBl
    {
        private readonly IProductsDl idl;
        public ProductsBl(IProductsDl idl)
        {
            this.idl = idl;
        }

        public bool Addproduct(Products p)
        {
            if (!IsValidProduct(p, isNew: true, out string error))
                throw new ArgumentException(error);

            bool result= idl.Addproduct(p);
            if (result)
            {
                MySqlBackupHelper.CreateBackup();
            }
            return result;
        }

        public List<Products> GetProducts()
        {
            return idl.GetProducts();
        }

        public List<Products> searchproducts(string text)
        {
            

            return idl.searchproducts(text);
        }

        public bool update(Products p)
        {
            if (!IsValidProduct(p, isNew: false, out string error))
                throw new ArgumentException(error);

            bool result = idl.update(p);
            if (result)
            {
                MySqlBackupHelper.CreateBackup();
            }
            return result;
        }

        private bool IsValidProduct(Products p, bool isNew, out string error)
        {
            error = "";

            if (!isNew && p.Id <= 0)
                error = "Invalid product ID.";
            else if (string.IsNullOrWhiteSpace(p.Name))
                error = "Product name is required.";
            else if (p.Name.Length > 100)
                error = "Product name must be less than 100 characters.";
            else if (p.Description?.Length > 500)
                error = "Description must be less than 500 characters.";
            else if (p.Price < 0)
                error = "Price cannot be negative.";
            else if (p.quantity < 0)
                error = "Quantity cannot be negative.";

            return string.IsNullOrEmpty(error);
        }
    }
}
