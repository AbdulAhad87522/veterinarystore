using fertilizesop.BL.Models;
using System.Collections.Generic;

namespace fertilizesop.DL
{
    public interface IProductsDl
    {
        bool Addproduct(Products p);
        List<Products> GetProducts();
        List<Products> searchproducts(string text);
        bool update(Products c);
    }
}