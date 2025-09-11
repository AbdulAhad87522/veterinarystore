using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fertilizesop.BL.Models
{
    public class inventorylog
    {
        public string name { get; private set; }
        public int quantity_change { get; private set; }
        public DateTime log_date { get; private set; }
        public string type { get; private set; }
        public string remark { get; private set; }
        public inventorylog(string name, int quantity_change, DateTime log_date, string type, string remark)
        {
            this.name = name;
            this.quantity_change = quantity_change;
            this.log_date = log_date;
            this.type = type;
            this.remark = remark;
        }

        public inventorylog(string supplier, string description, int quantity)
        {
            name = supplier;
            this.quantity_change = quantity;
            this.type = description;
        }
    }
}
