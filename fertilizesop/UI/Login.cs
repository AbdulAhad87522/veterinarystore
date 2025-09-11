using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fertilizesop.BL.Models;
using fertilizesop.DL;

namespace TechStore.UI
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = SystemFonts.DefaultFont; // ✅ ensures scaling matches
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                if (txtpassword.Focused)
                {
                    btnlogin.PerformClick();
                    return true;
                }
               
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void txtname_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtpassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            string username = txtname.Text.Trim();
            string password = txtpassword.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both username and password.");
                return;
            }

            if (!LoginDL.ValidateUser(username, password))
            {
                MessageBox.Show("Invalid credentials.");
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close(); // Closes the modal and triggers Application.Run for Dashboard
        }


        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
  

}
