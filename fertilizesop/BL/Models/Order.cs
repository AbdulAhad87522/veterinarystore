using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fertilizesop.BL.Models
{
   
        public class Order
        {
            
            public int SupplierId { get; set; }
            public DateTime OrderDate { get; set; }
            public string status { get; set; }
        public string supplier_name { get; set; }
        public Order(int supplierId,DateTime date,string sts)
            {
            SupplierId=supplierId;
            OrderDate=date;
            status = sts;
            }
        public Order(int supplierId, DateTime date, string sts,string supplier_name)
        {
            SupplierId = supplierId;
            OrderDate = date;
            status = sts;
            this.supplier_name = supplier_name;
        }
    }

    public class OrderDetail
        {
            
            public int OrderId { get; set; }
            public int ProductId { get; set; }
            public int Quantity { get; set; }

             public OrderDetail(int orderId, int productId, int quantity)
             {
            OrderId = orderId;
            ProductId = productId;
            Quantity = quantity;
            }
    }
}

