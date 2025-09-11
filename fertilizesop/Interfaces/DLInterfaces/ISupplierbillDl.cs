using fertilizesop.BL.Models;
using System.Collections.Generic;

namespace fertilizesop.DL
{
    public interface ISupplierbillDl
    {
        List<Supplierbill> getbill();
        List<Supplierbill> getbills(int billid);
        List<Supplierbill> getbills(string text);
        Supplierbill GetSupplierBillByBatchName(string batchName);
        bool UpdateBill(Supplierbill s);
    }
}