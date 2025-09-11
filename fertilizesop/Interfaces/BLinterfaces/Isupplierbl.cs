using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fertilizesop.BL.Models;

namespace fertilizesop.Interfaces.BLInterfaces
{
    public interface Isupplierbl
    {
        bool addsupplier(Suppliers s);
        bool deletesupplier(int s);
        bool updatesupplier(Suppliers s);
        List<Suppliers> searchsupplier(string text);
        List<Suppliers> getsupplier();
    }
}
