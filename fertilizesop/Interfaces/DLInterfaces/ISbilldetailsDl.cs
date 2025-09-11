using fertilizesop.BL.Models;
using System.Collections.Generic;

namespace fertilizesop.DL
{
    public interface ISbilldetailsDl
    {
        bool addrecord(Spricerecord s);
        List<Supplierpayment> getdetails(int billid);
        List<Spricerecord> getrecord(int billid);
    }
}