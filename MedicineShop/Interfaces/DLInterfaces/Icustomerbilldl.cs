using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicineShop.BL.Models;

namespace MedicineShop.Interfaces.DLInterfaces
{
    internal interface Icustomerbilldl
    {
        bool AddCustomerPayment(int companyId, decimal paymentAmount);
        List<custbill> GetCustomerBills(int companyid);
        List<custbill> GetCustomerBills(string text);
        List<custPaymentRecord> GetCustomerPaymentRecords(int companyId);
        List<custPaymentRecord> GetcustPaymentRecords(int companyId);
    }
}
