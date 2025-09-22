using fertilizesop.UI;
using MedicineShop.BL;
using MedicineShop.DL;
using MedicineShop.UI;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MedicineShop
{
    internal static class Program
    {
        public static IServiceProvider ServiceProvider { get; private set; }
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var services = new ServiceCollection();
            configureServices(services);
            ServiceProvider = services.BuildServiceProvider();
            var mainForm = ServiceProvider.GetRequiredService<Batchform>();
            Application.Run(mainForm);

            ////Show login first(Modal)
            //var login = ServiceProvider.GetRequiredService<UI.Login>();
            //var result = login.ShowDialog();

            //if (result == DialogResult.OK)
            //{
            //    // Run dashboard only after login passes
            //    Application.Run(ServiceProvider.GetRequiredService<Dashboard>());
            //    //}
            //}


        }
        private static void configureServices(ServiceCollection services)
        {
            // Register all forms
            services.AddTransient<Dashboard>();
            //services.AddTransient<UI.Login>();
            services.AddTransient<CompanyMain>();
            services.AddTransient<AddCompany>();
            services.AddTransient<CompanyBill>();
            services.AddTransient<Customermain>();
            services.AddTransient<customer_bills>();
            services.AddTransient<CustomerBill_SpecificProducts>();

            services.AddTransient<Batchform>();
            services.AddTransient<AddBatchdetailsform>();
            // Register other dependencies like Bl classes, DbContext, etc.
            services.AddTransient<CompanyBL>();
            services.AddTransient<BL.Bl.CompanyBillBl>();
            services.AddTransient<BatchesBl>();
            services.AddTransient<CompanyBillsDl>();
            // Add DbContext registration here if needed
        }
    }
    }
