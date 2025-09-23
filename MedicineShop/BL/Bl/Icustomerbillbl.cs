using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicineShop.BL.Models;

namespace MedicineShop.BL.Bl
{
    internal interface Icustomerbillbl
    {
        bool AddcustomerPayment(int companyId, decimal paymentAmount);
        List<CompanyBill> GetAllCustomerBills(string search = "");
        List<CompanyBill> GetCustomerBillById(int companyId);
        List<PaymentRecord> GetcustPaymentRecords(int companyId);
        List<PaymentRecord> getcustrecord(int company_id);
    }
}
