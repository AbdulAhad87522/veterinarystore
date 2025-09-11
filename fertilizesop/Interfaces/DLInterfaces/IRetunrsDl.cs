using fertilizesop.BL.Models;
using System.Collections.Generic;

namespace fertilizesop.DL
{
    public interface IRetunrsDl
    {
        bool addreturn(returns r);
        List<Products> getproducts(string name);
    }
}