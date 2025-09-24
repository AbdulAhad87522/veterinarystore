﻿using fertilizesop.UI;
using MedicineShop.BL;
using MedicineShop.BL.Bl;
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
            var mainForm = ServiceProvider.GetRequiredService<HomeContentform>();
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
            services.AddTransient<Batchform>();
            services.AddTransient<AddBatchdetailsform>();
            services.AddTransient<HomeContentform>();

            // Register other dependencies like Bl classes, DbContext, etc.
            services.AddScoped<ICompanyBillsDl, CompanyBillsDl>();
            services.AddScoped<ICompanyBillBl, CompanyBillBl>();
            services.AddScoped<IBatchesBl, BatchesBl>();
            services.AddScoped<IBatchesDl, BatchesDl>();
            services.AddScoped<IBatchItemsBl, BatchItemsBl>();
            services.AddScoped<IBatchItemsDl, BatchItemsDl>();

            // Add DbContext registration here if needed
        }
    }
}
