using fertilizesop.BL.Models;
using System.Collections.Generic;

namespace fertilizesop.DL
{
    public interface ICustomerDl
    {
        bool Addcustomer(Customers s);
        List<Isupplier> getcustomers();
        List<Isupplier> searchcustomers(string text);
        bool update(Customers c);
    }
}