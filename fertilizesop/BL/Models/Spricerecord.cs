using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fertilizesop.BL.Models
{
    public class Spricerecord
    {
        public int id { get; set; }
        public string name { get; set; }
        public decimal payement { get; set; }
        public DateTime date { get; set; }
        public int bill_id { get; set; }
        public string remarks { get; set; }
        public int suppid { get; set; }
        public Spricerecord(int id, string name, decimal payement, DateTime date, int bill_id, string remarks)
        {
            this.id = id;
            this.name = name;
            this.payement = payement;
            this.date = date;
            this.bill_id = bill_id;
            this.remarks = remarks;
        }
        public Spricerecord(int id, int suppid, decimal payement, DateTime date, int bill_id, string remarks)
        {
            this.id = id;
            this.payement = payement;
            this.date = date;
            this.bill_id = bill_id;
            this.remarks = remarks;
            this.suppid = suppid;
        }
    }
}
