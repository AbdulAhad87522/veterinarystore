using fertilizesop.BL.Models;
using System.Collections.Generic;

namespace fertilizesop.BL.Bl
{
    public interface IReturnsBl
    {
        bool AddReturn(returns r);
        List<Products> GetProducts(string name);
    }
}