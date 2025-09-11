using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fertilizesop.DL;

namespace fertilizesop.BL.Bl
{
    public class BillingRecordsOverviewBL
    {
        private readonly BillingRecordsOverviewDL _billingDL;

        public BillingRecordsOverviewBL()
        {
            _billingDL = new BillingRecordsOverviewDL();
        }

        public string FormatAsPKR(object amount)
        {
            if (amount == null || amount == DBNull.Value)
                return "Rs. 0.00";

            decimal decimalAmount;
            if (amount is int intAmount)
                decimalAmount = Convert.ToDecimal(intAmount);
            else if (amount is decimal)
                decimalAmount = (decimal)amount;
            else
                decimal.TryParse(amount.ToString(), out decimalAmount);

            return $"Rs. {decimalAmount:N2}";
        }

        public DataTable GetBillingRecords(string searchTerm = "")
        {
            try
            {
                DataTable dt = _billingDL.GetCustomerBillingRecords(searchTerm);

                // Handle potential INT values from database
                if (dt.Columns.Contains("TotalAmount"))
                    dt.Columns["TotalAmount"].DataType = typeof(decimal);
                if (dt.Columns.Contains("PaidAmount"))
                    dt.Columns["PaidAmount"].DataType = typeof(decimal);
                if (dt.Columns.Contains("DueAmount"))
                    dt.Columns["DueAmount"].DataType = typeof(decimal);

                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in business logic while getting billing records: " + ex.Message);
            }
        }


        public DataTable GetBillDetails(int billId)
        {
            try
            {
                return _billingDL.GetBillDetails(billId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in business logic while getting bill details: " + ex.Message);
            }
        }

        public DataTable GetPaymentHistory(int billId)
        {
            try
            {
                return _billingDL.GetPaymentHistory(billId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in business logic while getting payment history: " + ex.Message);
            }
        }

        public decimal CalculateDueAmount(decimal totalAmount, decimal paidAmount)
        {
            return totalAmount - paidAmount;
        }

        public string GetPaymentStatus(decimal totalAmount, decimal paidAmount)
        {
            return paidAmount >= totalAmount ? "Paid" : "Due";
        }
    }
}
