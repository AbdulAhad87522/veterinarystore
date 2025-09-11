using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using fertilizesop.BL.Models;


namespace fertilizesop.BL.Models.persons
{
    public class Customer : Ipersons
    {
        public string name {  get; set; }
        public int id { get; set; }
        public string email {  get; set; }
        public string address { get; set; }
        public string phone {  get; set; }
        public string first_name {  get; set; }
        public string last_name { get; set; }
        public string type { get; set; }


        public Customer() { }
        public Customer (int id, string first_name , string last_name ,string type ,  string address, string email, string phone)
        {
            this. first_name = first_name;
            this.last_name = last_name;
            this.type = type;
            this.id = id;
            this.email = email;
            this.address = address;
            this.phone = phone;
        }

        public Customer(string first_name, string last_name, string type, string address, string email, string phone)
        {
            this.first_name = first_name;
            this.last_name = last_name;
            this.type = type;
            this.email = email;
            this.address = address;
            this.phone = phone;
        }

    }
}
