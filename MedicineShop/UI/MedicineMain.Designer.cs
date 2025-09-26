namespace MedicineShop
{
    partial class MedicineMain
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toplbl = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnDelete = new FontAwesome.Sharp.IconButton();
            this.btnAdd = new FontAwesome.Sharp.IconButton();
            this.iconButton1 = new FontAwesome.Sharp.IconButton();
            this.iconButton2 = new FontAwesome.Sharp.IconButton();
            this.panelbill = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pckcmb = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbCompany = new System.Windows.Forms.ComboBox();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.txtDesc = new System.Windows.Forms.TextBox();
            this.btnUpdate = new FontAwesome.Sharp.IconButton();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtPrice = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txttotalprice = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnsave = new FontAwesome.Sharp.IconButton();
            this.lblBatch = new System.Windows.Forms.Label();
            this.lblProducts = new System.Windows.Forms.Label();
            this.lblQuantity = new System.Windows.Forms.Label();
            this.iconButton4 = new FontAwesome.Sharp.IconButton();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panelbill.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(148)))), ((int)(((byte)(197)))));
            this.panel1.Controls.Add(this.toplbl);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1439, 84);
            this.panel1.TabIndex = 212;
            // 
            // toplbl
            // 
            this.toplbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.toplbl.AutoSize = true;
            this.toplbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toplbl.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.toplbl.Location = new System.Drawing.Point(601, 26);
            this.toplbl.Name = "toplbl";
            this.toplbl.Size = new System.Drawing.Size(272, 38);
            this.toplbl.TabIndex = 6;
            this.toplbl.Text = "Medicine Details";
            this.toplbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.Location = new System.Drawing.Point(24, 150);
            this.txtSearch.Multiline = true;
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(316, 51);
            this.txtSearch.TabIndex = 214;
            this.txtSearch.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Baskerville Old Face", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(148)))), ((int)(((byte)(197)))));
            this.label1.Location = new System.Drawing.Point(19, 109);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(293, 26);
            this.label1.TabIndex = 213;
            this.label1.Text = "Search Medicine by Name :";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.AntiqueWhite;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.OldLace;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.AliceBlue;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.GridColor = System.Drawing.SystemColors.AppWorkspace;
            this.dataGridView1.Location = new System.Drawing.Point(81, 298);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView1.Size = new System.Drawing.Size(1267, 579);
            this.dataGridView1.TabIndex = 211;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnDelete.FlatAppearance.BorderColor = System.Drawing.Color.Indigo;
            this.btnDelete.FlatAppearance.BorderSize = 2;
            this.btnDelete.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DodgerBlue;
            this.btnDelete.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(51)))), ((int)(((byte)(69)))));
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.ForeColor = System.Drawing.Color.Black;
            this.btnDelete.IconChar = FontAwesome.Sharp.IconChar.PlusSquare;
            this.btnDelete.IconColor = System.Drawing.Color.Beige;
            this.btnDelete.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnDelete.IconSize = 35;
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(611, 150);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(231, 51);
            this.btnDelete.TabIndex = 210;
            this.btnDelete.Text = "Delete";
            this.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(148)))), ((int)(((byte)(197)))));
            this.btnAdd.FlatAppearance.BorderColor = System.Drawing.Color.Indigo;
            this.btnAdd.FlatAppearance.BorderSize = 2;
            this.btnAdd.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DodgerBlue;
            this.btnAdd.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(51)))), ((int)(((byte)(69)))));
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnAdd.IconChar = FontAwesome.Sharp.IconChar.PlusSquare;
            this.btnAdd.IconColor = System.Drawing.Color.Gainsboro;
            this.btnAdd.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnAdd.IconSize = 35;
            this.btnAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAdd.Location = new System.Drawing.Point(364, 150);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(231, 51);
            this.btnAdd.TabIndex = 208;
            this.btnAdd.Text = "Add";
            this.btnAdd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // iconButton1
            // 
            this.iconButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.iconButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(148)))), ((int)(((byte)(197)))));
            this.iconButton1.FlatAppearance.BorderColor = System.Drawing.Color.Indigo;
            this.iconButton1.FlatAppearance.BorderSize = 2;
            this.iconButton1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DodgerBlue;
            this.iconButton1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(51)))), ((int)(((byte)(69)))));
            this.iconButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButton1.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iconButton1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.iconButton1.IconChar = FontAwesome.Sharp.IconChar.PlusSquare;
            this.iconButton1.IconColor = System.Drawing.Color.Gainsboro;
            this.iconButton1.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconButton1.IconSize = 35;
            this.iconButton1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iconButton1.Location = new System.Drawing.Point(1117, 150);
            this.iconButton1.Name = "iconButton1";
            this.iconButton1.Size = new System.Drawing.Size(231, 51);
            this.iconButton1.TabIndex = 215;
            this.iconButton1.Text = "Add Category";
            this.iconButton1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iconButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.iconButton1.UseVisualStyleBackColor = false;
            this.iconButton1.Click += new System.EventHandler(this.iconButton1_Click);
            // 
            // iconButton2
            // 
            this.iconButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.iconButton2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(148)))), ((int)(((byte)(197)))));
            this.iconButton2.FlatAppearance.BorderColor = System.Drawing.Color.Indigo;
            this.iconButton2.FlatAppearance.BorderSize = 2;
            this.iconButton2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DodgerBlue;
            this.iconButton2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(51)))), ((int)(((byte)(69)))));
            this.iconButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButton2.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iconButton2.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.iconButton2.IconChar = FontAwesome.Sharp.IconChar.PlusSquare;
            this.iconButton2.IconColor = System.Drawing.Color.Gainsboro;
            this.iconButton2.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconButton2.IconSize = 35;
            this.iconButton2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iconButton2.Location = new System.Drawing.Point(864, 150);
            this.iconButton2.Name = "iconButton2";
            this.iconButton2.Size = new System.Drawing.Size(231, 51);
            this.iconButton2.TabIndex = 216;
            this.iconButton2.Text = "Add Packing";
            this.iconButton2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iconButton2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.iconButton2.UseVisualStyleBackColor = false;
            this.iconButton2.Click += new System.EventHandler(this.iconButton2_Click);
            // 
            // panelbill
            // 
            this.panelbill.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(137)))), ((int)(((byte)(200)))), ((int)(((byte)(254)))));
            this.panelbill.Controls.Add(this.panel2);
            this.panelbill.Location = new System.Drawing.Point(414, 222);
            this.panelbill.Name = "panelbill";
            this.panelbill.Size = new System.Drawing.Size(527, 712);
            this.panelbill.TabIndex = 217;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(137)))), ((int)(((byte)(200)))), ((int)(((byte)(254)))));
            this.panel2.Controls.Add(this.iconButton4);
            this.panel2.Controls.Add(this.pckcmb);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.cmbCompany);
            this.panel2.Controls.Add(this.cmbCategory);
            this.panel2.Controls.Add(this.txtDesc);
            this.panel2.Controls.Add(this.btnUpdate);
            this.panel2.Controls.Add(this.txtName);
            this.panel2.Controls.Add(this.txtPrice);
            this.panel2.Controls.Add(this.textBox3);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.textBox2);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.txttotalprice);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.btnsave);
            this.panel2.Controls.Add(this.lblBatch);
            this.panel2.Controls.Add(this.lblProducts);
            this.panel2.Controls.Add(this.lblQuantity);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(527, 712);
            this.panel2.TabIndex = 208;
            // 
            // pckcmb
            // 
            this.pckcmb.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pckcmb.FormattingEnabled = true;
            this.pckcmb.Location = new System.Drawing.Point(138, 175);
            this.pckcmb.Name = "pckcmb";
            this.pckcmb.Size = new System.Drawing.Size(286, 37);
            this.pckcmb.TabIndex = 221;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(133, 418);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(157, 25);
            this.label6.TabIndex = 220;
            this.label6.Text = "Select Company";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(133, 315);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(152, 25);
            this.label5.TabIndex = 219;
            this.label5.Text = "Select Category";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(133, 512);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 25);
            this.label2.TabIndex = 218;
            this.label2.Text = "Description";
            // 
            // cmbCompany
            // 
            this.cmbCompany.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbCompany.FormattingEnabled = true;
            this.cmbCompany.Location = new System.Drawing.Point(138, 460);
            this.cmbCompany.Name = "cmbCompany";
            this.cmbCompany.Size = new System.Drawing.Size(286, 37);
            this.cmbCompany.TabIndex = 217;
            // 
            // cmbCategory
            // 
            this.cmbCategory.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Location = new System.Drawing.Point(138, 364);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(286, 37);
            this.cmbCategory.TabIndex = 216;
            // 
            // txtDesc
            // 
            this.txtDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDesc.Location = new System.Drawing.Point(138, 540);
            this.txtDesc.Multiline = true;
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Size = new System.Drawing.Size(286, 87);
            this.txtDesc.TabIndex = 214;
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(148)))), ((int)(((byte)(197)))));
            this.btnUpdate.FlatAppearance.BorderColor = System.Drawing.Color.Indigo;
            this.btnUpdate.FlatAppearance.BorderSize = 2;
            this.btnUpdate.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DodgerBlue;
            this.btnUpdate.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(51)))), ((int)(((byte)(69)))));
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdate.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnUpdate.IconChar = FontAwesome.Sharp.IconChar.PlusSquare;
            this.btnUpdate.IconColor = System.Drawing.Color.Gainsboro;
            this.btnUpdate.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnUpdate.IconSize = 35;
            this.btnUpdate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUpdate.Location = new System.Drawing.Point(119, 646);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(140, 53);
            this.btnUpdate.TabIndex = 212;
            this.btnUpdate.Text = "Edit";
            this.btnUpdate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUpdate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtName.Location = new System.Drawing.Point(138, 63);
            this.txtName.Multiline = true;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(286, 46);
            this.txtName.TabIndex = 211;
            // 
            // txtPrice
            // 
            this.txtPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPrice.Location = new System.Drawing.Point(138, 254);
            this.txtPrice.Multiline = true;
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.Size = new System.Drawing.Size(286, 46);
            this.txtPrice.TabIndex = 210;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(944, 779);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(250, 22);
            this.textBox3.TabIndex = 209;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(842, 775);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(110, 25);
            this.label4.TabIndex = 208;
            this.label4.Text = "Reamining:";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(547, 777);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(250, 22);
            this.textBox2.TabIndex = 207;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(484, 774);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 25);
            this.label3.TabIndex = 206;
            this.label3.Text = "Paid:";
            // 
            // txttotalprice
            // 
            this.txttotalprice.Location = new System.Drawing.Point(178, 778);
            this.txttotalprice.Name = "txttotalprice";
            this.txttotalprice.Size = new System.Drawing.Size(250, 22);
            this.txttotalprice.TabIndex = 205;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(61, 774);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(111, 25);
            this.label7.TabIndex = 204;
            this.label7.Text = "Total Price:";
            // 
            // btnsave
            // 
            this.btnsave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(148)))), ((int)(((byte)(197)))));
            this.btnsave.FlatAppearance.BorderColor = System.Drawing.Color.Indigo;
            this.btnsave.FlatAppearance.BorderSize = 2;
            this.btnsave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DodgerBlue;
            this.btnsave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(51)))), ((int)(((byte)(69)))));
            this.btnsave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnsave.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnsave.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnsave.IconChar = FontAwesome.Sharp.IconChar.PlusSquare;
            this.btnsave.IconColor = System.Drawing.Color.Gainsboro;
            this.btnsave.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnsave.IconSize = 35;
            this.btnsave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnsave.Location = new System.Drawing.Point(1226, 763);
            this.btnsave.Name = "btnsave";
            this.btnsave.Size = new System.Drawing.Size(207, 49);
            this.btnsave.TabIndex = 200;
            this.btnsave.Text = "Save";
            this.btnsave.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnsave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnsave.UseVisualStyleBackColor = false;
            // 
            // lblBatch
            // 
            this.lblBatch.AutoSize = true;
            this.lblBatch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBatch.Location = new System.Drawing.Point(133, 35);
            this.lblBatch.Name = "lblBatch";
            this.lblBatch.Size = new System.Drawing.Size(148, 25);
            this.lblBatch.TabIndex = 157;
            this.lblBatch.Text = "Medicine Name";
            // 
            // lblProducts
            // 
            this.lblProducts.AutoSize = true;
            this.lblProducts.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProducts.Location = new System.Drawing.Point(133, 134);
            this.lblProducts.Name = "lblProducts";
            this.lblProducts.Size = new System.Drawing.Size(82, 25);
            this.lblProducts.TabIndex = 158;
            this.lblProducts.Text = "Packing";
            // 
            // lblQuantity
            // 
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQuantity.Location = new System.Drawing.Point(133, 226);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(101, 25);
            this.lblQuantity.TabIndex = 159;
            this.lblQuantity.Text = "Sale Price";
            // 
            // iconButton4
            // 
            this.iconButton4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(38)))), ((int)(((byte)(64)))));
            this.iconButton4.FlatAppearance.BorderColor = System.Drawing.Color.Indigo;
            this.iconButton4.FlatAppearance.BorderSize = 2;
            this.iconButton4.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DodgerBlue;
            this.iconButton4.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(51)))), ((int)(((byte)(69)))));
            this.iconButton4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButton4.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iconButton4.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.iconButton4.IconChar = FontAwesome.Sharp.IconChar.FloppyDisk;
            this.iconButton4.IconColor = System.Drawing.Color.Red;
            this.iconButton4.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconButton4.IconSize = 35;
            this.iconButton4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iconButton4.Location = new System.Drawing.Point(297, 646);
            this.iconButton4.Name = "iconButton4";
            this.iconButton4.Size = new System.Drawing.Size(131, 53);
            this.iconButton4.TabIndex = 222;
            this.iconButton4.Text = "Cancel";
            this.iconButton4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iconButton4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.iconButton4.UseVisualStyleBackColor = false;
            this.iconButton4.Click += new System.EventHandler(this.iconButton4_Click);
            // 
            // MedicineMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1439, 888);
            this.Controls.Add(this.panelbill);
            this.Controls.Add(this.iconButton2);
            this.Controls.Add(this.iconButton1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAdd);
            this.Name = "MedicineMain";
            this.Text = "MedicineMain";
            this.Load += new System.EventHandler(this.MedicineMain_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panelbill.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label toplbl;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private FontAwesome.Sharp.IconButton btnDelete;
        private FontAwesome.Sharp.IconButton btnAdd;
        private FontAwesome.Sharp.IconButton iconButton1;
        private FontAwesome.Sharp.IconButton iconButton2;
        private System.Windows.Forms.Panel panelbill;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox pckcmb;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbCompany;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.TextBox txtDesc;
        private FontAwesome.Sharp.IconButton btnUpdate;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtPrice;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txttotalprice;
        private System.Windows.Forms.Label label7;
        private FontAwesome.Sharp.IconButton btnsave;
        private System.Windows.Forms.Label lblBatch;
        private System.Windows.Forms.Label lblProducts;
        private System.Windows.Forms.Label lblQuantity;
        private FontAwesome.Sharp.IconButton iconButton4;
    }
}