using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fertilizesop.DL;

namespace fertilizesop.BL.Bl
{
    internal class FinanceReportBL
    {
        private readonly FinanceReportDAL _dal;

        public FinanceReportBL()
        {
            _dal = new FinanceReportDAL();
        }

        public DataTable GetMonthlyReport(int month, int year)
        {
            return _dal.GetMonthlyReport(month, year);
        }

        public DataTable GetYearlyReport(int year)
        {
            return _dal.GetYearlyReport(year);
        }

        public void GenerateMonthlyReportPDF(DataTable reportData, string filePath,int month, int year)
        {
            _dal.GenerateMonthlyReportPDF(reportData, filePath,month,year);
        }
        public void GenerateYearlyReportPDF(DataTable reportData, string filePath, int year)
        {
            _dal.GenerateYearlyReportPDF(reportData, filePath, year);
        }
    }
}
