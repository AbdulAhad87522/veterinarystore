using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fertilizesop.BL.Models;
using System.Windows.Forms;

namespace fertilizesop.Interfaces.BLInterfaces
{
    internal interface IOrderBl
    {
        int InsertOrder(Order order);
        void InsertOrderDetail(OrderDetail detail);
        void UpdateOrderDetail(OrderDetail detail);
        bool Delete(int orderDetailId);
        DataTable LoadOrdersWithDetails();
        DataTable GetProducts();
        DataTable GetAllSuppliers();
        DataTable GetOrders();

        List<Suppliers> GetSuppliers(string searchText);

        void MarkOrderAsCompleted(int orderId);

        void CreateOrderInvoicePdf(DataGridView cart, string filePath, string Name, DateTime saleDate);
        void PrintOrderInvoiceDirectly(DataGridView cart, string supplierName, DateTime Date);
        void DrawPurchaseInvoice(PrintPageEventArgs e, DataGridView cart, string supplierName, DateTime Date);

    }
}
