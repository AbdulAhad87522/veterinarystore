using fertilizesop.BL.Models;
using System.Collections.Generic;

namespace fertilizesop.BL.Bl
{
    public interface ISupplierBillBl
    {
        List<Supplierbill> getbill();
        List<Supplierbill> getbillbyname(string text);
        List<Supplierbill> getbills(int billid);
        Supplierbill getbills(string batchname);
        bool updateamount(Supplierbill b);
    }
}