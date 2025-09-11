using fertilizesop.BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fertilizesop.Interfaces.BLInterfaces
{
    public interface IProductBl
    {
        bool Addproduct(Products p);
        List<Products> GetProducts();
        List<Products> searchproducts(string text);
        bool update(Products c);
    }
    }

