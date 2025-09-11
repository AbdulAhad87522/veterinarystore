using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fertilizesop.BL.Models;
using fertilizesop.DL;
using System.Windows.Forms;
using fertilizesop.Interfaces.BLInterfaces;

namespace fertilizesop.BL.Bl
{
    internal class OrderBl : IOrderBl
    {
        private readonly OrderDAL _orderDL;

        public OrderBl()
        {
            _orderDL = new OrderDAL();
        }

        public int InsertOrder(Order order)
        {
            try
            {
                int orderId = _orderDL.InsertOrder(order);

                if (orderId > 0)
                {
                    // ✅ Create MySQL full backup after order is successfully inserted
                    MySqlBackupHelper.CreateBackup();
                }

                return orderId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inserting order: " + ex.Message);
                return 0;
            }
        }


public void InsertOrderDetail(OrderDetail detail)
    {
        try
        {
            _orderDL.InsertOrderDetail(detail);

            // ✅ Create MySQL backup after inserting order detail
            MySqlBackupHelper.CreateBackup();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error inserting order detail: " + ex.Message);
        }
    }

    public void UpdateOrderDetail(OrderDetail detail)
    {
        try
        {
            _orderDL.UpdateOrderDetail(detail);

            // ✅ Create MySQL backup after updating order detail
            MySqlBackupHelper.CreateBackup();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error updating order detail: " + ex.Message);
        }
    }


    public bool Delete(int orderDetailId)
        {
            try
            {
                return _orderDL.Delete(orderDetailId);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting order detail: " + ex.Message);
                return false;
            }
        }

        public DataTable LoadOrdersWithDetails()
        {
            try
            {
                return _orderDL.LoadOrdersWithDetails();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading orders: " + ex.Message);
                return new DataTable();
            }
        }

        public DataTable GetOrders()
        {
            try
            {
                return _orderDL.GetOrders();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading orders: " + ex.Message);
                return new DataTable();
            }
        }

        public DataTable GetProducts()
        {
            try
            {
                return _orderDL.GetProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading products: " + ex.Message);
                return new DataTable();
            }
        }

        public DataTable GetAllSuppliers()
        {
            try
            {
                return _orderDL.GetAllSuppliers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading suppliers: " + ex.Message);
                return new DataTable();
            }
        }

        public void MarkOrderAsCompleted(int Id)
        {
            try
            {
                _orderDL.MarkOrderAsCompleted(Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating order detail: " + ex.Message);
            }
        }

        public List<Suppliers> GetSuppliers(string text)
        {
            try
            {
                return _orderDL.GetSuppliers(text);  // Calls the DL function that returns List<Suppliers>
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading suppliers: " + ex.Message);
                return new List<Suppliers>(); // Return an empty list instead of undefined "List"
            }
        }

        public void CreateOrderInvoicePdf(DataGridView cart, string filePath, string name, DateTime saleDate)
        {
            try
            {
                _orderDL.CreateOrderInvoicePdf(cart, filePath, name, saleDate);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating invoice PDF: " + ex.Message);
            }
        }

        public void PrintOrderInvoiceDirectly(DataGridView cart, string supplierName, DateTime date)
        {
            try
            {
                _orderDL.PrintOrderInvoiceDirectly(cart, supplierName, date);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error printing invoice: " + ex.Message);
            }
        }

        public void DrawPurchaseInvoice(PrintPageEventArgs e, DataGridView cart, string supplierName, DateTime date)
        {
            try
            {
                _orderDL.DrawPurchaseInvoice(e, cart, supplierName, date);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error drawing invoice: " + ex.Message);
            }
        }
    }
}
