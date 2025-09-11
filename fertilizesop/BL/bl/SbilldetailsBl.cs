using fertilizesop.BL.Models;
using fertilizesop.DL;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fertilizesop.BL.Bl
{
    public class SbilldetailsBl : ISbilldetailsBl
    {
        private readonly ISbilldetailsDl ibl;
        public SbilldetailsBl(ISbilldetailsDl ibl)
        {
            this.ibl = ibl;
        }
        public bool addrecord(Spricerecord s)
        {
            if (s.payement <= 0)
            {
                throw new ArgumentException("Payement should be greater than zero");
            }
            bool result = ibl.addrecord(s);
            if (result)
            {
                MySqlBackupHelper.CreateBackup();
            }
            return result;
        }

        public List<Supplierpayment> getdetails(int billid)
        {
            return ibl.getdetails(billid);
        }
    }
}
