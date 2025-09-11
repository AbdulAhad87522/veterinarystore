namespace fertilizesop.UI
{
    partial class bankform
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(bankform));
            this.panel10 = new System.Windows.Forms.Panel();
            this.iconButton1 = new FontAwesome.Sharp.IconButton();
            this.btnbill = new FontAwesome.Sharp.IconButton();
            this.PanelBill = new System.Windows.Forms.Panel();
            this.txtremaining = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtDate = new System.Windows.Forms.DateTimePicker();
            this.btncanceldetail = new FontAwesome.Sharp.IconButton();
            this.btnsavedetail = new FontAwesome.Sharp.IconButton();
            this.txtbamount = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.panelbank = new System.Windows.Forms.Panel();
            this.txtname = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btncancle1 = new FontAwesome.Sharp.IconButton();
            this.btnsave = new FontAwesome.Sharp.IconButton();
            this.button1 = new System.Windows.Forms.Button();
            this.iconButton9 = new FontAwesome.Sharp.IconButton();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label12 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toplbl = new System.Windows.Forms.Label();
            this.txtamount = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbbank = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbtype = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel10.SuspendLayout();
            this.PanelBill.SuspendLayout();
            this.panelbank.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel10
            // 
            this.panel10.AutoScroll = true;
            this.panel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(255)))), ((int)(((byte)(228)))));
            this.panel10.Controls.Add(this.iconButton1);
            this.panel10.Controls.Add(this.btnbill);
            this.panel10.Controls.Add(this.PanelBill);
            this.panel10.Controls.Add(this.panelbank);
            this.panel10.Controls.Add(this.button1);
            this.panel10.Controls.Add(this.iconButton9);
            this.panel10.Controls.Add(this.dataGridView2);
            this.panel10.Controls.Add(this.pictureBox1);
            this.panel10.Controls.Add(this.label12);
            this.panel10.Controls.Add(this.textBox1);
            this.panel10.Controls.Add(this.panel1);
            this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel10.Location = new System.Drawing.Point(0, 0);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(1312, 792);
            this.panel10.TabIndex = 17;
            // 
            // iconButton1
            // 
            this.iconButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(64)))), ((int)(((byte)(31)))));
            this.iconButton1.FlatAppearance.BorderColor = System.Drawing.Color.Indigo;
            this.iconButton1.FlatAppearance.BorderSize = 2;
            this.iconButton1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DodgerBlue;
            this.iconButton1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(51)))), ((int)(((byte)(69)))));
            this.iconButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButton1.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iconButton1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.iconButton1.IconChar = FontAwesome.Sharp.IconChar.PlusSquare;
            this.iconButton1.IconColor = System.Drawing.Color.MediumTurquoise;
            this.iconButton1.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconButton1.IconSize = 35;
            this.iconButton1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iconButton1.Location = new System.Drawing.Point(181, 111);
            this.iconButton1.Name = "iconButton1";
            this.iconButton1.Size = new System.Drawing.Size(151, 42);
            this.iconButton1.TabIndex = 149;
            this.iconButton1.Text = "Add Details";
            this.iconButton1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iconButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.iconButton1.UseVisualStyleBackColor = false;
            this.iconButton1.Click += new System.EventHandler(this.iconButton1_Click);
            // 
            // btnbill
            // 
            this.btnbill.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(64)))), ((int)(((byte)(31)))));
            this.btnbill.FlatAppearance.BorderColor = System.Drawing.Color.Indigo;
            this.btnbill.FlatAppearance.BorderSize = 2;
            this.btnbill.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DodgerBlue;
            this.btnbill.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(51)))), ((int)(((byte)(69)))));
            this.btnbill.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnbill.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnbill.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnbill.IconChar = FontAwesome.Sharp.IconChar.PlusSquare;
            this.btnbill.IconColor = System.Drawing.Color.MediumTurquoise;
            this.btnbill.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnbill.IconSize = 35;
            this.btnbill.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnbill.Location = new System.Drawing.Point(348, 109);
            this.btnbill.Name = "btnbill";
            this.btnbill.Size = new System.Drawing.Size(151, 42);
            this.btnbill.TabIndex = 148;
            this.btnbill.Text = "Details";
            this.btnbill.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnbill.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnbill.UseVisualStyleBackColor = false;
            this.btnbill.Click += new System.EventHandler(this.btnbill_Click);
            // 
            // PanelBill
            // 
            this.PanelBill.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.PanelBill.Controls.Add(this.cmbtype);
            this.PanelBill.Controls.Add(this.label2);
            this.PanelBill.Controls.Add(this.label4);
            this.PanelBill.Controls.Add(this.cmbbank);
            this.PanelBill.Controls.Add(this.txtremaining);
            this.PanelBill.Controls.Add(this.label5);
            this.PanelBill.Controls.Add(this.txtDate);
            this.PanelBill.Controls.Add(this.btncanceldetail);
            this.PanelBill.Controls.Add(this.btnsavedetail);
            this.PanelBill.Controls.Add(this.txtbamount);
            this.PanelBill.Controls.Add(this.label7);
            this.PanelBill.Controls.Add(this.label9);
            this.PanelBill.Controls.Add(this.label10);
            this.PanelBill.Location = new System.Drawing.Point(605, 110);
            this.PanelBill.Name = "PanelBill";
            this.PanelBill.Size = new System.Drawing.Size(443, 610);
            this.PanelBill.TabIndex = 147;
            // 
            // txtremaining
            // 
            this.txtremaining.BackColor = System.Drawing.Color.Gainsboro;
            this.txtremaining.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtremaining.Location = new System.Drawing.Point(51, 174);
            this.txtremaining.Multiline = true;
            this.txtremaining.Name = "txtremaining";
            this.txtremaining.ReadOnly = true;
            this.txtremaining.Size = new System.Drawing.Size(313, 37);
            this.txtremaining.TabIndex = 169;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(64)))), ((int)(((byte)(31)))));
            this.label5.Location = new System.Drawing.Point(46, 377);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 30);
            this.label5.TabIndex = 168;
            this.label5.Text = "Date";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtDate
            // 
            this.txtDate.Location = new System.Drawing.Point(48, 420);
            this.txtDate.Name = "txtDate";
            this.txtDate.Size = new System.Drawing.Size(316, 22);
            this.txtDate.TabIndex = 167;
            // 
            // btncanceldetail
            // 
            this.btncanceldetail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(64)))), ((int)(((byte)(31)))));
            this.btncanceldetail.FlatAppearance.BorderColor = System.Drawing.Color.Indigo;
            this.btncanceldetail.FlatAppearance.BorderSize = 2;
            this.btncanceldetail.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DodgerBlue;
            this.btncanceldetail.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(51)))), ((int)(((byte)(69)))));
            this.btncanceldetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btncanceldetail.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btncanceldetail.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btncanceldetail.IconChar = FontAwesome.Sharp.IconChar.FloppyDisk;
            this.btncanceldetail.IconColor = System.Drawing.Color.Red;
            this.btncanceldetail.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btncanceldetail.IconSize = 35;
            this.btncanceldetail.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btncanceldetail.Location = new System.Drawing.Point(243, 471);
            this.btncanceldetail.Name = "btncanceldetail";
            this.btncanceldetail.Size = new System.Drawing.Size(151, 42);
            this.btncanceldetail.TabIndex = 166;
            this.btncanceldetail.Text = "Cancel";
            this.btncanceldetail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btncanceldetail.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btncanceldetail.UseVisualStyleBackColor = false;
            this.btncanceldetail.Click += new System.EventHandler(this.btncanceldetail_Click);
            // 
            // btnsavedetail
            // 
            this.btnsavedetail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(64)))), ((int)(((byte)(31)))));
            this.btnsavedetail.FlatAppearance.BorderColor = System.Drawing.Color.Indigo;
            this.btnsavedetail.FlatAppearance.BorderSize = 2;
            this.btnsavedetail.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DodgerBlue;
            this.btnsavedetail.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(51)))), ((int)(((byte)(69)))));
            this.btnsavedetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnsavedetail.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnsavedetail.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnsavedetail.IconChar = FontAwesome.Sharp.IconChar.FloppyDisk;
            this.btnsavedetail.IconColor = System.Drawing.Color.LimeGreen;
            this.btnsavedetail.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnsavedetail.IconSize = 35;
            this.btnsavedetail.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnsavedetail.Location = new System.Drawing.Point(30, 471);
            this.btnsavedetail.Name = "btnsavedetail";
            this.btnsavedetail.Size = new System.Drawing.Size(151, 42);
            this.btnsavedetail.TabIndex = 165;
            this.btnsavedetail.Text = "Save";
            this.btnsavedetail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnsavedetail.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnsavedetail.UseVisualStyleBackColor = false;
            this.btnsavedetail.Click += new System.EventHandler(this.btnsavedetail_Click);
            // 
            // txtbamount
            // 
            this.txtbamount.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbamount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbamount.Location = new System.Drawing.Point(54, 247);
            this.txtbamount.Multiline = true;
            this.txtbamount.Name = "txtbamount";
            this.txtbamount.Size = new System.Drawing.Size(313, 37);
            this.txtbamount.TabIndex = 164;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(64)))), ((int)(((byte)(31)))));
            this.label7.Location = new System.Drawing.Point(49, 214);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(94, 30);
            this.label7.TabIndex = 163;
            this.label7.Text = "Amount";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(64)))), ((int)(((byte)(31)))));
            this.label9.Location = new System.Drawing.Point(52, 71);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(127, 30);
            this.label9.TabIndex = 161;
            this.label9.Text = "Bank Name";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(64)))), ((int)(((byte)(31)))));
            this.label10.Location = new System.Drawing.Point(113, 32);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(167, 38);
            this.label10.TabIndex = 160;
            this.label10.Text = "AddDetail";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelbank
            // 
            this.panelbank.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.panelbank.Controls.Add(this.txtamount);
            this.panelbank.Controls.Add(this.label3);
            this.panelbank.Controls.Add(this.txtname);
            this.panelbank.Controls.Add(this.label6);
            this.panelbank.Controls.Add(this.label1);
            this.panelbank.Controls.Add(this.btncancle1);
            this.panelbank.Controls.Add(this.btnsave);
            this.panelbank.Location = new System.Drawing.Point(82, 182);
            this.panelbank.Name = "panelbank";
            this.panelbank.Size = new System.Drawing.Size(443, 505);
            this.panelbank.TabIndex = 146;
            // 
            // txtname
            // 
            this.txtname.BackColor = System.Drawing.Color.Gainsboro;
            this.txtname.Location = new System.Drawing.Point(52, 125);
            this.txtname.Multiline = true;
            this.txtname.Name = "txtname";
            this.txtname.Size = new System.Drawing.Size(313, 35);
            this.txtname.TabIndex = 201;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(64)))), ((int)(((byte)(31)))));
            this.label6.Location = new System.Drawing.Point(48, 92);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(127, 30);
            this.label6.TabIndex = 205;
            this.label6.Text = "Bank Name";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(64)))), ((int)(((byte)(31)))));
            this.label1.Location = new System.Drawing.Point(92, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(177, 38);
            this.label1.TabIndex = 204;
            this.label1.Text = "Add Batch";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btncancle1
            // 
            this.btncancle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(64)))), ((int)(((byte)(31)))));
            this.btncancle1.FlatAppearance.BorderColor = System.Drawing.Color.Indigo;
            this.btncancle1.FlatAppearance.BorderSize = 2;
            this.btncancle1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DodgerBlue;
            this.btncancle1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(51)))), ((int)(((byte)(69)))));
            this.btncancle1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btncancle1.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btncancle1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btncancle1.IconChar = FontAwesome.Sharp.IconChar.FloppyDisk;
            this.btncancle1.IconColor = System.Drawing.Color.Red;
            this.btncancle1.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btncancle1.IconSize = 35;
            this.btncancle1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btncancle1.Location = new System.Drawing.Point(228, 301);
            this.btncancle1.Name = "btncancle1";
            this.btncancle1.Size = new System.Drawing.Size(151, 42);
            this.btncancle1.TabIndex = 200;
            this.btncancle1.Text = "Cancel";
            this.btncancle1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btncancle1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btncancle1.UseVisualStyleBackColor = false;
            // 
            // btnsave
            // 
            this.btnsave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(64)))), ((int)(((byte)(31)))));
            this.btnsave.FlatAppearance.BorderColor = System.Drawing.Color.Indigo;
            this.btnsave.FlatAppearance.BorderSize = 2;
            this.btnsave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DodgerBlue;
            this.btnsave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(51)))), ((int)(((byte)(69)))));
            this.btnsave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnsave.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnsave.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnsave.IconChar = FontAwesome.Sharp.IconChar.FloppyDisk;
            this.btnsave.IconColor = System.Drawing.Color.LimeGreen;
            this.btnsave.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnsave.IconSize = 35;
            this.btnsave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnsave.Location = new System.Drawing.Point(53, 301);
            this.btnsave.Name = "btnsave";
            this.btnsave.Size = new System.Drawing.Size(132, 42);
            this.btnsave.TabIndex = 190;
            this.btnsave.Text = "Save";
            this.btnsave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnsave.UseVisualStyleBackColor = false;
            this.btnsave.Click += new System.EventHandler(this.btnsave_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(64)))), ((int)(((byte)(31)))));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.button1.Location = new System.Drawing.Point(1171, 116);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 31);
            this.button1.TabIndex = 145;
            this.button1.Text = "Search";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // iconButton9
            // 
            this.iconButton9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(64)))), ((int)(((byte)(31)))));
            this.iconButton9.FlatAppearance.BorderColor = System.Drawing.Color.Indigo;
            this.iconButton9.FlatAppearance.BorderSize = 2;
            this.iconButton9.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DodgerBlue;
            this.iconButton9.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(51)))), ((int)(((byte)(69)))));
            this.iconButton9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButton9.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iconButton9.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.iconButton9.IconChar = FontAwesome.Sharp.IconChar.PlusSquare;
            this.iconButton9.IconColor = System.Drawing.Color.MediumTurquoise;
            this.iconButton9.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconButton9.IconSize = 35;
            this.iconButton9.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iconButton9.Location = new System.Drawing.Point(12, 111);
            this.iconButton9.Name = "iconButton9";
            this.iconButton9.Size = new System.Drawing.Size(151, 42);
            this.iconButton9.TabIndex = 144;
            this.iconButton9.Text = "Add";
            this.iconButton9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iconButton9.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.iconButton9.UseVisualStyleBackColor = false;
            this.iconButton9.Click += new System.EventHandler(this.iconButton9_Click);
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.AntiqueWhite;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Snow;
            this.dataGridView2.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView2.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView2.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(255)))), ((int)(((byte)(197)))));
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.GridColor = System.Drawing.SystemColors.AppWorkspace;
            this.dataGridView2.Location = new System.Drawing.Point(12, 182);
            this.dataGridView2.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.ReadOnly = true;
            this.dataGridView2.RowHeadersWidth = 51;
            this.dataGridView2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView2.Size = new System.Drawing.Size(1267, 440);
            this.dataGridView2.TabIndex = 143;
            this.dataGridView2.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellContentClick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(515, 104);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(43, 43);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 142;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(64)))), ((int)(((byte)(31)))));
            this.label12.Location = new System.Drawing.Point(818, 117);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(81, 25);
            this.label12.TabIndex = 140;
            this.label12.Text = "Search :";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(904, 116);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(230, 29);
            this.textBox1.TabIndex = 141;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(64)))), ((int)(((byte)(31)))));
            this.panel1.Controls.Add(this.toplbl);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1312, 84);
            this.panel1.TabIndex = 15;
            // 
            // toplbl
            // 
            this.toplbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.toplbl.AutoSize = true;
            this.toplbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toplbl.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.toplbl.Location = new System.Drawing.Point(538, 26);
            this.toplbl.Name = "toplbl";
            this.toplbl.Size = new System.Drawing.Size(143, 38);
            this.toplbl.TabIndex = 6;
            this.toplbl.Text = "Batches";
            this.toplbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtamount
            // 
            this.txtamount.BackColor = System.Drawing.Color.Gainsboro;
            this.txtamount.Location = new System.Drawing.Point(52, 215);
            this.txtamount.Multiline = true;
            this.txtamount.Name = "txtamount";
            this.txtamount.Size = new System.Drawing.Size(313, 35);
            this.txtamount.TabIndex = 206;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(64)))), ((int)(((byte)(31)))));
            this.label3.Location = new System.Drawing.Point(48, 182);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(206, 30);
            this.label3.TabIndex = 207;
            this.label3.Text = "Reamining Amount";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbbank
            // 
            this.cmbbank.FormattingEnabled = true;
            this.cmbbank.Location = new System.Drawing.Point(57, 108);
            this.cmbbank.Name = "cmbbank";
            this.cmbbank.Size = new System.Drawing.Size(313, 24);
            this.cmbbank.TabIndex = 173;
            this.cmbbank.SelectedIndexChanged += new System.EventHandler(this.cmbbank_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(64)))), ((int)(((byte)(31)))));
            this.label4.Location = new System.Drawing.Point(52, 135);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(206, 30);
            this.label4.TabIndex = 174;
            this.label4.Text = "Remaining Amount";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbtype
            // 
            this.cmbtype.FormattingEnabled = true;
            this.cmbtype.Items.AddRange(new object[] {
            "withdraw",
            "deposit"});
            this.cmbtype.Location = new System.Drawing.Point(51, 341);
            this.cmbtype.Name = "cmbtype";
            this.cmbtype.Size = new System.Drawing.Size(313, 24);
            this.cmbtype.TabIndex = 176;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(64)))), ((int)(((byte)(31)))));
            this.label2.Location = new System.Drawing.Point(46, 304);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 30);
            this.label2.TabIndex = 175;
            this.label2.Text = "Type";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bankform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1312, 792);
            this.Controls.Add(this.panel10);
            this.Name = "bankform";
            this.Text = "bankform";
            this.Load += new System.EventHandler(this.bankform_Load);
            this.panel10.ResumeLayout(false);
            this.panel10.PerformLayout();
            this.PanelBill.ResumeLayout(false);
            this.PanelBill.PerformLayout();
            this.panelbank.ResumeLayout(false);
            this.panelbank.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel10;
        private FontAwesome.Sharp.IconButton iconButton1;
        private FontAwesome.Sharp.IconButton btnbill;
        private System.Windows.Forms.Panel PanelBill;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbbank;
        private System.Windows.Forms.TextBox txtremaining;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker txtDate;
        private FontAwesome.Sharp.IconButton btncanceldetail;
        private FontAwesome.Sharp.IconButton btnsavedetail;
        private System.Windows.Forms.TextBox txtbamount;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Panel panelbank;
        private System.Windows.Forms.TextBox txtamount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtname;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private FontAwesome.Sharp.IconButton btncancle1;
        private FontAwesome.Sharp.IconButton btnsave;
        private System.Windows.Forms.Button button1;
        private FontAwesome.Sharp.IconButton iconButton9;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label toplbl;
        private System.Windows.Forms.ComboBox cmbtype;
        private System.Windows.Forms.Label label2;
    }
}