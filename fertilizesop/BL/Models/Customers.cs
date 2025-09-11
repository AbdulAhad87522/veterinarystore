using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fertilizesop.BL.Models
{
    public class Customers:Suppliers,Isupplier
    {
        public string last_name {  get; set; }
        public Customers(int id, string first_name, string phone, string address, string last_name) : base(id, first_name, phone, address)
        {

            this.last_name = last_name;
        }
    }
}
