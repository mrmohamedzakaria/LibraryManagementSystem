// SettingsForm.Designer.cs
namespace LibraryManagementSystem
{
    partial class SettingsForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkRightToLeft = new System.Windows.Forms.CheckBox();
            this.txtAppName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblThemePreview = new System.Windows.Forms.Label();
            this.panelThemePreview = new System.Windows.Forms.Panel();
            this.panelBackground = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panelPrimary = new System.Windows.Forms.Panel();
            this.panelSecondary = new System.Windows.Forms.Panel();
            this.panelDanger = new System.Windows.Forms.Panel();
            this.panelWarning = new System.Windows.Forms.Panel();
            this.panelInfo = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.cmbTheme = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabLoan = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.numBorrowDays = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.tabDisplay = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chkColorCoding = new System.Windows.Forms.CheckBox();
            this.tabDatabase = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnTestConnection = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnResetDefaults = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panelThemePreview.SuspendLayout();
            this.panelBackground.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.tabLoan.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBorrowDays)).BeginInit();
            this.tabDisplay.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabDatabase.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(584, 60);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label1.Size = new System.Drawing.Size(584, 60);
            this.label1.TabIndex = 0;
            this.label1.Text = "إعدادات النظام";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabGeneral);
            this.tabControl1.Controls.Add(this.tabLoan);
            this.tabControl1.Controls.Add(this.tabDisplay);
            this.tabControl1.Controls.Add(this.tabDatabase);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(0, 60);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.RightToLeftLayout = true;
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(584, 301);
            this.tabControl1.TabIndex = 1;
            // 
            // tabGeneral
            // 
            this.tabGeneral.Controls.Add(this.groupBox2);
            this.tabGeneral.Controls.Add(this.groupBox1);
            this.tabGeneral.Location = new System.Drawing.Point(4, 25);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabGeneral.Size = new System.Drawing.Size(576, 272);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "عام";
            this.tabGeneral.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.chkRightToLeft);
            this.groupBox2.Controls.Add(this.txtAppName);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(8, 181);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupBox2.Size = new System.Drawing.Size(560, 83);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "إعدادات النظام";
            // 
            // chkRightToLeft
            // 
            this.chkRightToLeft.AutoSize = true;
            this.chkRightToLeft.Location = new System.Drawing.Point(305, 51);
            this.chkRightToLeft.Name = "chkRightToLeft";
            this.chkRightToLeft.Size = new System.Drawing.Size(126, 20);
            this.chkRightToLeft.TabIndex = 2;
            this.chkRightToLeft.Text = "واجهة من اليمين لليسار";
            this.chkRightToLeft.UseVisualStyleBackColor = true;
            // 
            // txtAppName
            // 
            this.txtAppName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAppName.Location = new System.Drawing.Point(6, 23);
            this.txtAppName.Name = "txtAppName";
            this.txtAppName.Size = new System.Drawing.Size(424, 22);
            this.txtAppName.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(479, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 16);
            this.label3.TabIndex = 0;
            this.label3.Text = "اسم التطبيق:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.lblThemePreview);
            this.groupBox1.Controls.Add(this.panelThemePreview);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.cmbTheme);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(8, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupBox1.Size = new System.Drawing.Size(560, 169);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "مظهر التطبيق";
            // 
            // lblThemePreview
            // 
            this.lblThemePreview.AutoSize = true;
            this.lblThemePreview.Location = new System.Drawing.Point(211, 90);
            this.lblThemePreview.Name = "lblThemePreview";
            this.lblThemePreview.Size = new System.Drawing.Size(185, 16);
            this.lblThemePreview.TabIndex = 4;
            this.lblThemePreview.Text = "معاينة الألوان والخلفية والنصوص بالنظام";
            // 
            // panelThemePreview
            // 
            this.panelThemePreview.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelThemePreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelThemePreview.Controls.Add(this.panelBackground);
            this.panelThemePreview.Location = new System.Drawing.Point(6, 77);
            this.panelThemePreview.Name = "panelThemePreview";
            this.panelThemePreview.Size = new System.Drawing.Size(545, 85);
            this.panelThemePreview.TabIndex = 3;
            // 
            // panelBackground
            // 
            this.panelBackground.Controls.Add(this.flowLayoutPanel1);
            this.panelBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBackground.Location = new System.Drawing.Point(0, 0);
            this.panelBackground.Name = "panelBackground";
            this.panelBackground.Size = new System.Drawing.Size(543, 83);
            this.panelBackground.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.panelPrimary);
            this.flowLayoutPanel1.Controls.Add(this.panelSecondary);
            this.flowLayoutPanel1.Controls.Add(this.panelDanger);
            this.flowLayoutPanel1.Controls.Add(this.panelWarning);
            this.flowLayoutPanel1.Controls.Add(this.panelInfo);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 43);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.flowLayoutPanel1.Size = new System.Drawing.Size(543, 40);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // panelPrimary
            // 
            this.panelPrimary.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.panelPrimary.Location = new System.Drawing.Point(515, 3);
            this.panelPrimary.Name = "panelPrimary";
            this.panelPrimary.Size = new System.Drawing.Size(25, 25);
            this.panelPrimary.TabIndex = 0;
            // 
            // panelSecondary
            // 
            this.panelSecondary.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.panelSecondary.Location = new System.Drawing.Point(484, 3);
            this.panelSecondary.Name = "panelSecondary";
            this.panelSecondary.Size = new System.Drawing.Size(25, 25);
            this.panelSecondary.TabIndex = 1;
            // 
            // panelDanger
            // 
            this.panelDanger.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.panelDanger.Location = new System.Drawing.Point(453, 3);
            this.panelDanger.Name = "panelDanger";
            this.panelDanger.Size = new System.Drawing.Size(25, 25);
            this.panelDanger.TabIndex = 2;
            // 
            // panelWarning
            // 
            this.panelWarning.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(126)))), ((int)(((byte)(34)))));
            this.panelWarning.Location = new System.Drawing.Point(422, 3);
            this.panelWarning.Name = "panelWarning";
            this.panelWarning.Size = new System.Drawing.Size(25, 25);
            this.panelWarning.TabIndex = 3;
            // 
            // panelInfo
            // 
            this.panelInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.panelInfo.Location = new System.Drawing.Point(391, 3);
            this.panelInfo.Name = "panelInfo";
            this.panelInfo.Size = new System.Drawing.Size(25, 25);
            this.panelInfo.TabIndex = 4;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(478, 58);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(40, 16);
            this.label9.TabIndex = 2;
            this.label9.Text = "معاينة:";
            // 
            // cmbTheme
            // 
            this.cmbTheme.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbTheme.FormattingEnabled = true;
            this.cmbTheme.Location = new System.Drawing.Point(6, 23);
            this.cmbTheme.Name = "cmbTheme";
            this.cmbTheme.Size = new System.Drawing.Size(424, 24);
            this.cmbTheme.TabIndex = 1;
            this.cmbTheme.SelectedIndexChanged += new System.EventHandler(this.cmbTheme_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(479, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "اختر المظهر:";
            // 
            // tabLoan
            // 
            this.tabLoan.Controls.Add(this.groupBox3);
            this.tabLoan.Location = new System.Drawing.Point(4, 25);
            this.tabLoan.Name = "tabLoan";
            this.tabLoan.Padding = new System.Windows.Forms.Padding(3);
            this.tabLoan.Size = new System.Drawing.Size(576, 272);
            this.tabLoan.TabIndex = 1;
            this.tabLoan.Text = "الإعارة";
            this.tabLoan.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.numBorrowDays);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Location = new System.Drawing.Point(8, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupBox3.Size = new System.Drawing.Size(560, 76);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "إعدادات الإعارة";
            // 
            // numBorrowDays
            // 
            this.numBorrowDays.Location = new System.Drawing.Point(383, 31);
            this.numBorrowDays.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numBorrowDays.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numBorrowDays.Name = "numBorrowDays";
            this.numBorrowDays.Size = new System.Drawing.Size(61, 22);
            this.numBorrowDays.TabIndex = 1;
            this.numBorrowDays.Value = new decimal(new int[] {
            14,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(450, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 16);
            this.label4.TabIndex = 0;
            this.label4.Text = "مدة الإعارة (بالأيام):";
            // 
            // tabDisplay
            // 
            this.tabDisplay.Controls.Add(this.groupBox4);
            this.tabDisplay.Location = new System.Drawing.Point(4, 25);
            this.tabDisplay.Name = "tabDisplay";
            this.tabDisplay.Padding = new System.Windows.Forms.Padding(3);
            this.tabDisplay.Size = new System.Drawing.Size(576, 272);
            this.tabDisplay.TabIndex = 2;
            this.tabDisplay.Text = "العرض";
            this.tabDisplay.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.chkColorCoding);
            this.groupBox4.Location = new System.Drawing.Point(8, 6);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupBox4.Size = new System.Drawing.Size(560, 74);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "إعدادات العرض";
            // 
            // chkColorCoding
            // 
            this.chkColorCoding.AutoSize = true;
            this.chkColorCoding.Location = new System.Drawing.Point(310, 32);
            this.chkColorCoding.Name = "chkColorCoding";
            this.chkColorCoding.Size = new System.Drawing.Size(244, 20);
            this.chkColorCoding.TabIndex = 0;
            this.chkColorCoding.Text = "تلوين الصفوف حسب الحالة (متأخرة، غير متوفرة)";
            this.chkColorCoding.UseVisualStyleBackColor = true;
            // 
            // tabDatabase
            // 
            this.tabDatabase.Controls.Add(this.groupBox5);
            this.tabDatabase.Location = new System.Drawing.Point(4, 25);
            this.tabDatabase.Name = "tabDatabase";
            this.tabDatabase.Padding = new System.Windows.Forms.Padding(3);
            this.tabDatabase.Size = new System.Drawing.Size(576, 272);
            this.tabDatabase.TabIndex = 3;
            this.tabDatabase.Text = "قاعدة البيانات";
            this.tabDatabase.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.btnTestConnection);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.label5);
            this.groupBox5.Location = new System.Drawing.Point(8, 6);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupBox5.Size = new System.Drawing.Size(560, 144);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "إعدادات قاعدة البيانات";
            // 
            // btnTestConnection
            // 
            this.btnTestConnection.Location = new System.Drawing.Point(6, 93);
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.Size = new System.Drawing.Size(170, 30);
            this.btnTestConnection.TabIndex = 2;
            this.btnTestConnection.Text = "اختبار الاتصال";
            this.btnTestConnection.UseVisualStyleBackColor = true;
            this.btnTestConnection.Click += new System.EventHandler(this.btnTestConnection_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(229, 60);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(325, 16);
            this.label6.TabIndex = 1;
            this.label6.Text = "تحذير: تغيير إعدادات قاعدة البيانات قد يؤدي إلى فقدان الوصول للبيانات";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(346, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(208, 16);
            this.label5.TabIndex = 0;
            this.label5.Text = "يتم استخدام قاعدة بيانات SQLite المحلية";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.Controls.Add(this.btnResetDefaults);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnSaveSettings);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 361);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(584, 50);
            this.panel2.TabIndex = 2;
            // 
            // btnResetDefaults
            // 
            this.btnResetDefaults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnResetDefaults.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(126)))), ((int)(((byte)(34)))));
            this.btnResetDefaults.FlatAppearance.BorderSize = 0;
            this.btnResetDefaults.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResetDefaults.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnResetDefaults.ForeColor = System.Drawing.Color.White;
            this.btnResetDefaults.Location = new System.Drawing.Point(12, 10);
            this.btnResetDefaults.Name = "btnResetDefaults";
            this.btnResetDefaults.Size = new System.Drawing.Size(152, 30);
            this.btnResetDefaults.TabIndex = 2;
            this.btnResetDefaults.Text = "استعادة الإعدادات الافتراضية";
            this.btnResetDefaults.UseVisualStyleBackColor = false;
            this.btnResetDefaults.Click += new System.EventHandler(this.btnResetDefaults_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(422, 10);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(70, 30);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "إلغاء";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnSaveSettings.FlatAppearance.BorderSize = 0;
            this.btnSaveSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveSettings.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveSettings.ForeColor = System.Drawing.Color.White;
            this.btnSaveSettings.Location = new System.Drawing.Point(498, 10);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(74, 30);
            this.btnSaveSettings.TabIndex = 0;
            this.btnSaveSettings.Text = "حفظ";
            this.btnSaveSettings.UseVisualStyleBackColor = false;
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.btnSaveSettings;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(584, 411);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.MinimumSize = new System.Drawing.Size(600, 450);
            this.Name = "SettingsForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "إعدادات النظام";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panelThemePreview.ResumeLayout(false);
            this.panelBackground.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.tabLoan.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBorrowDays)).EndInit();
            this.tabDisplay.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tabDatabase.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.TabPage tabLoan;
        private System.Windows.Forms.TabPage tabDisplay;
        private System.Windows.Forms.TabPage tabDatabase;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbTheme;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtAppName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkRightToLeft;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnSaveSettings;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numBorrowDays;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox chkColorCoding;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnTestConnection;
        private System.Windows.Forms.Button btnResetDefaults;
        private System.Windows.Forms.Panel panelThemePreview;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel panelBackground;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel panelPrimary;
        private System.Windows.Forms.Panel panelSecondary;
        private System.Windows.Forms.Panel panelDanger;
        private System.Windows.Forms.Panel panelWarning;
        private System.Windows.Forms.Panel panelInfo;
        private System.Windows.Forms.Label lblThemePreview;
    }
}