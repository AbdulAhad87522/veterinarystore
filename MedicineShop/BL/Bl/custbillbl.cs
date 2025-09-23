using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicineShop.BL.Models;

namespace MedicineShop.BL.Bl
{
    internal class custbillbl : Icustomerbillbl
    {
        public bool AddcustomerPayment(int companyId, decimal paymentAmount)
        {
            throw new NotImplementedException();
        }

        public List<CompanyBill> GetAllCustomerBills(string search = "")
        {
            throw new NotImplementedException();
        }

        public List<CompanyBill> GetCustomerBillById(int companyId)
        {
            throw new NotImplementedException();
        }

        public List<PaymentRecord> GetcustPaymentRecords(int companyId)
        {
            throw new NotImplementedException();
        }

        public List<PaymentRecord> getcustrecord(int company_id)
        {
            throw new NotImplementedException();
        }
    }
}
