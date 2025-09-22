using FontAwesome.Sharp;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace MedicineShop
{
    public partial class Dashboard : Form
    {
        private Form activeForm = null;
        private IconButton currentBtn;

        public static Dashboard Instance { get; private set; }
        public Dashboard()
        {
            InitializeComponent();
            this.Activated += Dashboard_Activated;
            Instance=this;

        }

        private void btninventory_Click(object sender, EventArgs e)
        {

        }
        public async void LoadFormIntoPanel(Form newForm)
        {
            if (newForm == null || newForm == activeForm) return;

            if (activeForm != null)
            {
                await FadeOutFormAsync(activeForm);
                panel10.Controls.Remove(activeForm); // <- fix: match the one used below
                activeForm.Dispose();
            }

            activeForm = newForm;
            newForm.TopLevel = false;
            newForm.FormBorderStyle = FormBorderStyle.None;
            newForm.Dock = DockStyle.Fill;
            newForm.Opacity = 0;
            panel10.Controls.Add(newForm); // Use same panel here
            newForm.Show();

            await FadeInFormAsync(newForm);
        }

        private void activebutton(object senderbtn, System.Drawing.Color color)
        {
            // Reset previous button
            disablebutton();

            // Set the new button as current
            currentBtn = (IconButton)senderbtn;
            currentBtn.BackColor = System.Drawing.Color.FromArgb(5, 51, 69);
            currentBtn.ForeColor = color;
            //currentBtn.TextAlign = ContentAlignment.MiddleCenter;
            currentBtn.IconColor = color;
            currentBtn.TextImageRelation = TextImageRelation.TextBeforeImage;
            //currentBtn.ImageAlign = ContentAlignment.MiddleRight;
        }
        private void disablebutton()
        {
            if (currentBtn != null)
            {
                currentBtn.BackColor = System.Drawing.Color.Transparent;
                currentBtn.ForeColor = System.Drawing.Color.White; // Fixed: Assigning a valid color value  
                currentBtn.TextAlign = ContentAlignment.MiddleLeft;
                currentBtn.IconColor = System.Drawing.Color.White; // Fixed: Assigning a valid color value  
                currentBtn.TextImageRelation = TextImageRelation.ImageBeforeText;
                currentBtn.ImageAlign = ContentAlignment.MiddleLeft;
            }
        }

        private async Task FadeOutFormAsync(Form form)
        {
            if (form == null || form.IsDisposed || !form.IsHandleCreated)
                return;

            try
            {
                while (form.Opacity > 0)
                {
                    if (form.IsDisposed) return;

                    form.Opacity -= 0.05;
                    await Task.Delay(10);
                }
                form.Opacity = 0;
            }
            catch (ObjectDisposedException)
            {
                // Safe exit
            }
        }
        private async Task FadeInFormAsync(Form form)
        {
            if (form == null || form.IsDisposed || !form.IsHandleCreated)
                return;

            try
            {
                while (form.Opacity < 1)
                {
                    if (form.IsDisposed) return;

                    form.Opacity += 0.05;
                    await Task.Delay(10);
                }
                form.Opacity = 1;
            }
            catch (ObjectDisposedException)
            {
                // Safe exit
            }
        }

        private void Dashboard_Activated(object sender, EventArgs e)
        {
            this.TopMost = true;   // Push to front
            this.TopMost = false;  // Reset
            this.BringToFront();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           

        }

        private void btnbatches_Click(object sender, EventArgs e)
        {
            var f = Program.ServiceProvider.GetRequiredService<Batchform>();
            LoadFormIntoPanel(f);
            activebutton(sender, Color.FromArgb(253, 138, 114));
        }
    }
}
