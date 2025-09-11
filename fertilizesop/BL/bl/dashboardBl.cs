using fertilizesop.BL.Models;
using fertilizesop.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fertilizesop.BL.Bl
{
    public class dashboardBl : IdashboardBl
    {
        private readonly IDashboardDL idl;
        public dashboardBl(IDashboardDL idl)
        {
            this.idl = idl;
        }
        public Dashboard GetDashboardSummary()
        {
            return new Dashboard
            {
                totalproducts = idl.totalproducts(),
                totalcustomers = idl.totalcustomers(),
                totalsuppliers = idl.totalsuppliers(),
                total_stock_value = idl.totalstockvalue(),
                salestodays = idl.salestoday(),
                pendingbills = idl.getpendingbills(),
                outproduct = idl.outofstocks(),
                total_stock = idl.totalstock()

            };
        }
        public List<(string MonthName, decimal TotalSales)> GetMonthlySalesComparison()
        {
            return idl.GetMonthlySalesComparison();
        }

        public List<(DateTime Day, decimal TotalSales)> GetMonthlySalesTrend()
        {
            return idl.GetMonthlySalesTrend();
        }

        public List<(string name, int quantity)> gettopsellingproducts() => idl.GetTopSellingProducts();

        public List<(string SupplierName, int TotalBatches)> GetTopSupplierContributions()
        {
            return idl.GetTopSupplierContributions();
        }

        public List<Products> outofstock()
        {
            return idl.outofstock();
        }

        public List<inventorylog> recentlogs()
        {
            return idl.recentlogs();
        }
    }
}
