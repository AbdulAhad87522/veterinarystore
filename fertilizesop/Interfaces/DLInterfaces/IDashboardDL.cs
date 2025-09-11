using fertilizesop.BL.Models;
using System;
using System.Collections.Generic;

namespace fertilizesop.DL
{
    public interface IDashboardDL
    {
        List<(string MonthName, decimal TotalSales)> GetMonthlySalesComparison();
        List<(DateTime Day, decimal TotalSales)> GetMonthlySalesTrend();
        int getpendingbills();
        List<(string ProductName, int QuantitySold)> GetTopSellingProducts();
        List<(string SupplierName, int TotalBatches)> GetTopSupplierContributions();
        List<Products> outofstock();
        int outofstocks();
        List<inventorylog> recentlogs();
        int salestoday();
        int totalcustomers();
        int totalproducts();
        int totalstock();
        int totalstockvalue();
        int totalsuppliers();
    }
}