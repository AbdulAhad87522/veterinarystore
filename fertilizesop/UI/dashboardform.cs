using FontAwesome.Sharp;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fertilizesop.UI
{
    public partial class dashboardform : Form
    {
        private Form activeForm = null;
        private IconButton currentBtn;

        private readonly Color[] sidebarColors = new Color[]
        {
            Color.FromArgb(0, 126, 250),    // [0] Tech Blue
            Color.FromArgb(0, 207, 255),    // [1] Sky Cyan
            Color.FromArgb(26, 188, 156),   // [2] Lime Mint
            Color.FromArgb(255, 140, 66),   // [3] Coral Orange
            Color.FromArgb(155, 89, 182),   // [4] Soft Purple
            Color.FromArgb(46, 204, 113),   // [5] Leaf Green
            Color.FromArgb(231, 76, 60),    // [6] Rose Red
            Color.FromArgb(52, 73, 94),     // [7] Deep Slate
            Color.FromArgb(241, 196, 15),   // [8] Sunny Yellow
            Color.FromArgb(22, 160, 133),   // [9] Sea Green
            Color.FromArgb(230, 126, 34),   // [10] Pumpkin
            Color.FromArgb(149, 165, 166),  // [11] Cloud Gray
            Color.FromArgb(44, 62, 80),     // [12] Midnight Blue
            Color.FromArgb(127, 140, 141),  // [13] Silver Gray
            Color.FromArgb(192, 57, 43)     // [14] Crimson Red
        };
        public dashboardform()
        {
            InitializeComponent();
            this.Activated += Dashboard_Activated;
        }
        private void Dashboard_Activated(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.TopMost = false;
            this.BringToFront();
        }

        public async void LoadFormIntoPanel(Form newForm)
        {
            if (newForm == null || newForm == activeForm) return;

            if (activeForm != null)
            {
                await FadeOutFormAsync(activeForm);
                panel10.Controls.Remove(activeForm);
                activeForm.Dispose();
            }

            activeForm = newForm;
            newForm.TopLevel = false;
            newForm.FormBorderStyle = FormBorderStyle.None;
            newForm.Dock = DockStyle.Fill;
            newForm.Opacity = 0;
            panel10.Controls.Add(newForm);
            newForm.Show();

            await FadeInFormAsync(newForm);
        }

        private async Task FadeOutFormAsync(Form form)
        {
            if (form == null || form.IsDisposed || !form.IsHandleCreated) return;

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
            catch (ObjectDisposedException) { }
        }

        private async Task FadeInFormAsync(Form form)
        {
            if (form == null || form.IsDisposed || !form.IsHandleCreated) return;

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
            catch (ObjectDisposedException) { }
        }

        private void ExpandPanel(Panel panel, int expandedHeight)
        {
            panel.Height = expandedHeight;
        }

        private void CollapsePanel(Panel panel, int collapsedHeight)
        {
            panel.Height = collapsedHeight;
        }

        private void CollapseAllTogglePanels()
        {
            CollapsePanel(panelbatch, 60);
            CollapsePanel(panelcust, 60);
            CollapsePanel(panelsupp, 60);
            //CollapsePanel(panelreturn, 60);
            CollapsePanel(panelinventory, 60);
            CollapsePanel(panelorder, 60);
        }

        private void activebutton(object senderBtn, Color color)
        {
            disablebutton();

            currentBtn = (IconButton)senderBtn;
            currentBtn.BackColor = Color.FromArgb(11, 64, 31);
            currentBtn.ForeColor = color;
            currentBtn.IconColor = color;
            currentBtn.TextImageRelation = TextImageRelation.TextBeforeImage;
        }

        private void disablebutton()
        {
            if (currentBtn != null)
            {
                currentBtn.BackColor = Color.Transparent;
                currentBtn.ForeColor = Color.White;
                currentBtn.IconColor = Color.White;
                currentBtn.TextAlign = ContentAlignment.MiddleLeft;
                currentBtn.TextImageRelation = TextImageRelation.ImageBeforeText;
                currentBtn.ImageAlign = ContentAlignment.MiddleLeft;
            }
        }

        // Panel toggles
        private void iconPictureBox2_Click(object sender, EventArgs e)
        {
            if (panelbatch.Height == 130)
                CollapsePanel(panelbatch, 60);
            else
            {
                CollapseAllTogglePanels();
                ExpandPanel(panelbatch, 130);
            }
        }

        private void iconPictureBox5_Click(object sender, EventArgs e)
        {
            if (panelsupp.Height == 132)
                CollapsePanel(panelsupp, 60);
            else
            {
                CollapseAllTogglePanels();
                ExpandPanel(panelsupp, 132);
            }
        }

        private void iconPictureBox6_Click(object sender, EventArgs e)
        {
            //if (panelreturn.Height == 195)
            //    CollapsePanel(panelreturn, 60);
            //else
            //{
            //    CollapseAllTogglePanels();
            //    ExpandPanel(panelreturn, 195);
            //}
        }

        private void iconPictureBox1_Click(object sender, EventArgs e)
        {
            if (panelinventory.Height == 131)
                CollapsePanel(panelinventory, 60);
            else
            {
                CollapseAllTogglePanels();
                ExpandPanel(panelinventory, 131);
            }
        }

        private void iconPictureBox4_Click(object sender, EventArgs e)
        {
            if (panelcust.Height == 130)
                CollapsePanel(panelcust, 60);
            else
            {
                CollapseAllTogglePanels();
                ExpandPanel(panelcust, 130);
            }
        }

        // Button Clicks

        private void Form1_Load(object sender, EventArgs e)
        {

            activebutton(btndashboard, sidebarColors[1]);
            var f = Program.ServiceProvider.GetRequiredService<HomeContentform>();
            LoadFormIntoPanel(f);
        }

        private void btndashboard_Click(object sender, EventArgs e)
        {
            activebutton(sender, sidebarColors[1]); // Sky Cyan
            var f = Program.ServiceProvider.GetRequiredService<HomeContentform>();
            LoadFormIntoPanel(f);
        }

        private void btnsuppliers_Click(object sender, EventArgs e)
        {
            activebutton(sender, sidebarColors[5]);
            var f = Program.ServiceProvider.GetRequiredService<supplierform>();
            LoadFormIntoPanel(f);
        }

        private void btnSbills_Click(object sender, EventArgs e)
        {
        }

        private void btnorder_Click(object sender, EventArgs e)
        {
       
        }

        private void btnlogout_Click(object sender, EventArgs e)
        {
            activebutton(sender, sidebarColors[14]);
            var f = Program.ServiceProvider.GetRequiredService<bankform>();
            LoadFormIntoPanel(f);// Crimson Red
            //Application.Exit();
        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            //Application.Exit(); // Close button
        }

        private void btnreturns_Click(object sender, EventArgs e)
        {
            activebutton(sender, sidebarColors[6]); // Rose Red
                                                    // LoadFormIntoPanel(new ReturnsForm());
        }

        private void btncustomers_Click(object sender, EventArgs e)
        {
            activebutton(sender, sidebarColors[4]); // Soft Purple
                                                    // LoadFormIntoPanel(new CustomersForm());
        }

        private void btnsale_Click(object sender, EventArgs e)
        {
            activebutton(sender, sidebarColors[3]); // Coral Orange
            var f = Program.ServiceProvider.GetRequiredService<Customersale>();
            LoadFormIntoPanel(f);                               // LoadFormIntoPanel(new SalesForm());
        }

        private void btnbatches_Click(object sender, EventArgs e)
        {
            activebutton(sender, sidebarColors[8]); // Sunny Yellow
            var f = Program.ServiceProvider.GetRequiredService<Batchform>();

            LoadFormIntoPanel(f);                       // LoadFormIntoPanel(new BatchesForm());
        }

        private void btninventory_Click(object sender, EventArgs e)
        {
            activebutton(sender, sidebarColors[1]);
            var f = Program.ServiceProvider.GetRequiredService<Productsform>();
            LoadFormIntoPanel(f);
        }

        private void btnrecord_Click(object sender, EventArgs e)
        {

        }

        private void btnproducts_Click(object sender, EventArgs e)
        {

        }

        private void btncategories_Click(object sender, EventArgs e)
        {

        }

        private void btnbatchdetails_Click(object sender, EventArgs e)
        {

        }

        private void btnadddetails_Click(object sender, EventArgs e)
        {

        }


        private void iconButton2_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btncustomers_Click_1(object sender, EventArgs e)
        {
            activebutton(sender, sidebarColors[5]);
            var f = Program.ServiceProvider.GetRequiredService<CustomerForm>();
            LoadFormIntoPanel(f);
        }

        private void btnbatchdetails_Click_1(object sender, EventArgs e)
        {
            var f = Program.ServiceProvider.GetRequiredService<BatchDetailsform>();
            LoadFormIntoPanel(f);
        }

        private void btnadddetails_Click_1(object sender, EventArgs e)
        {

        }

        private void btnSbills_Click_1(object sender, EventArgs e)
        {
            var f = Program.ServiceProvider.GetRequiredService<Supplierbillsform>();
            LoadFormIntoPanel(f);
        }

        private void btnrecord_Click_1(object sender, EventArgs e)
        {
            var f = Program.ServiceProvider.GetRequiredService<Inventorylogform>();
            LoadFormIntoPanel(f);
        }

        private void btncustomerbill_Click(object sender, EventArgs e)
        {
            var f = Program.ServiceProvider.GetRequiredService<customer_bills>();
            LoadFormIntoPanel(f);
        }

        private void iconButton3_Click(object sender, EventArgs e)
        {
            activebutton(sender, sidebarColors[0]);
            var f = Program.ServiceProvider.GetRequiredService<OrderStatus>();

            LoadFormIntoPanel(f);
        }

        private void iconButton4_Click(object sender, EventArgs e)
        {
            var f = Program.ServiceProvider.GetRequiredService<OrdersMain>();

            LoadFormIntoPanel(f);
        }

        private void iconPictureBox3_Click(object sender, EventArgs e)
        {
            if (panelorder.Height == 131)
                CollapsePanel(panelorder, 60);
            else
            {
                CollapseAllTogglePanels();
                ExpandPanel(panelorder, 131);
            }
        }

        private void iconButton5_Click(object sender, EventArgs e)
        {
            var f = Program.ServiceProvider.GetRequiredService<Customerreturnform>();
            LoadFormIntoPanel(f);
        }
    }
}
