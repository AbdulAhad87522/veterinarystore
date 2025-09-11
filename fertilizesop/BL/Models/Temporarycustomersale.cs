using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fertilizesop.BL.Models
{
    public class Temporarycustomersale
    {
        public string customername {  get; set; }
        public string productname {  get; set; }
        public int totaldiscount { get; set; }
        public decimal finalpriceafterdisc { get; set; }
        public decimal totalprice {  get; set; }
        public DateTime date {  get; set; }
        public List<saleitems> items { get; set; }

    }

    public class saleitems
    {
        public string productname { get; set; }
        public string description { get; set; }
        public int unitprice {  get; set; }
        public int quantity { get; set; }
        public int discount { get; set; }
        public int total { get; set; }
        public decimal finalprice {  get; set; }
    }
}
