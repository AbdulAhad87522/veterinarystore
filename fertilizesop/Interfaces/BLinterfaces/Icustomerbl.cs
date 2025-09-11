﻿using fertilizesop.BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fertilizesop.Interfaces.BLInterfaces
{
    public interface ICustomerBl
    {
        bool Addcustomer(Customers s);
        List<Isupplier> getcustomers();
        List<Isupplier> searchcustomers(string text);
        bool update(Customers c);
    }
}