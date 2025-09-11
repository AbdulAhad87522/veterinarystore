using fertilizesop.BL.Models;
using System;
using System.Collections.Generic;

namespace fertilizesop.BL.Bl
{
    public interface IdashboardBl
    {
        Dashboard GetDashboardSummary();
        List<(string MonthName, decimal TotalSales)> GetMonthlySalesComparison();
        List<(DateTime Day, decimal TotalSales)> GetMonthlySalesTrend();
        List<(string name, int quantity)> gettopsellingproducts();
        List<(string SupplierName, int TotalBatches)> GetTopSupplierContributions();
        List<Products> outofstock();
        List<inventorylog> recentlogs();
    }
}