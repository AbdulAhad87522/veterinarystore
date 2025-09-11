using fertilizesop.BL.Models;
using System.Collections.Generic;

namespace fertilizesop.BL.Bl
{
    public interface ISbilldetailsBl
    {
        bool addrecord(Spricerecord s);
        List<Supplierpayment> getdetails(int billid);
    }
}