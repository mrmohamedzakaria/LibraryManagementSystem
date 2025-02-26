// BackupForm.Designer.cs
namespace LibraryManagementSystem
{
    partial class BackupForm
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
            this.tabBackup = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnRestoreBackup = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnCreateBackup = new System.Windows.Forms.Button();
            this.btnBrowseBackupPath = new System.Windows.Forms.Button();
            this.txtBackupPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabExport = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnExportBorrowings = new System.Windows.Forms.Button();
            this.btnExportMembers = new System.Windows.Forms.Button();
            this.btnExportBooks = new System.Windows.Forms.Button();
            this.tabImport = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnImportMembers = new System.Windows.Forms.Button();
            this.btnImportBooks = new System.Windows.Forms.Button();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.dtpScheduleTime = new System.Windows.Forms.DateTimePicker();
            this.lblScheduleTime = new System.Windows.Forms.Label();
            this.lblDays = new System.Windows.Forms.Label();
            this.numBackupDays = new System.Windows.Forms.NumericUpDown();
            this.chkAutoBackup = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabBackup.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabExport.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabImport.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabSettings.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBackupDays)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(684, 60);
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
            this.label1.Size = new System.Drawing.Size(684, 60);
            this.label1.TabIndex = 0;
            this.label1.Text = "النسخ الاحتياطي والاستيراد/التصدير";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabBackup);
            this.tabControl1.Controls.Add(this.tabExport);
            this.tabControl1.Controls.Add(this.tabImport);
            this.tabControl1.Controls.Add(this.tabSettings);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(0, 60);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.RightToLeftLayout = true;
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(684, 351);
            this.tabControl1.TabIndex = 1;
            // 
            // tabBackup
            // 
            this.tabBackup.Controls.Add(this.groupBox2);
            this.tabBackup.Controls.Add(this.groupBox1);
            this.tabBackup.Location = new System.Drawing.Point(4, 25);
            this.tabBackup.Name = "tabBackup";
            this.tabBackup.Padding = new System.Windows.Forms.Padding(3);
            this.tabBackup.Size = new System.Drawing.Size(676, 322);
            this.tabBackup.TabIndex = 0;
            this.tabBackup.Text = "النسخ الاحتياطي";
            this.tabBackup.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnRestoreBackup);
            this.groupBox2.Location = new System.Drawing.Point(8, 167);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupBox2.Size = new System.Drawing.Size(660, 147);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "استعادة النسخة الاحتياطية";
            // 
            // btnRestoreBackup
            // 
            this.btnRestoreBackup.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnRestoreBackup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnRestoreBackup.FlatAppearance.BorderSize = 0;
            this.btnRestoreBackup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRestoreBackup.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRestoreBackup.ForeColor = System.Drawing.Color.White;
            this.btnRestoreBackup.Location = new System.Drawing.Point(218, 59);
            this.btnRestoreBackup.Name = "btnRestoreBackup";
            this.btnRestoreBackup.Size = new System.Drawing.Size(225, 40);
            this.btnRestoreBackup.TabIndex = 0;
            this.btnRestoreBackup.Text = "استعادة نسخة احتياطية";
            this.btnRestoreBackup.UseVisualStyleBackColor = false;
            this.btnRestoreBackup.Click += new System.EventHandler(this.btnRestoreBackup_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnCreateBackup);
            this.groupBox1.Controls.Add(this.btnBrowseBackupPath);
            this.groupBox1.Controls.Add(this.txtBackupPath);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(8, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupBox1.Size = new System.Drawing.Size(660, 155);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "إنشاء نسخة احتياطية";
            // 
            // btnCreateBackup
            // 
            this.btnCreateBackup.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnCreateBackup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnCreateBackup.FlatAppearance.BorderSize = 0;
            this.btnCreateBackup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreateBackup.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreateBackup.ForeColor = System.Drawing.Color.White;
            this.btnCreateBackup.Location = new System.Drawing.Point(218, 94);
            this.btnCreateBackup.Name = "btnCreateBackup";
            this.btnCreateBackup.Size = new System.Drawing.Size(225, 40);
            this.btnCreateBackup.TabIndex = 3;
            this.btnCreateBackup.Text = "إنشاء نسخة احتياطية";
            this.btnCreateBackup.UseVisualStyleBackColor = false;
            this.btnCreateBackup.Click += new System.EventHandler(this.btnCreateBackup_Click);
            // 
            // btnBrowseBackupPath
            // 
            this.btnBrowseBackupPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseBackupPath.Location = new System.Drawing.Point(32, 48);
            this.btnBrowseBackupPath.Name = "btnBrowseBackupPath";
            this.btnBrowseBackupPath.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseBackupPath.TabIndex = 2;
            this.btnBrowseBackupPath.Text = "استعراض";
            this.btnBrowseBackupPath.UseVisualStyleBackColor = true;
            this.btnBrowseBackupPath.Click += new System.EventHandler(this.btnBrowseBackupPath_Click);
            // txtBackupPath
            this.txtBackupPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBackupPath.Location = new System.Drawing.Point(113, 48);
            this.txtBackupPath.Name = "txtBackupPath";
            this.txtBackupPath.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtBackupPath.Size = new System.Drawing.Size(432, 22);
            this.txtBackupPath.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(551, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "مسار النسخ الاحتياطي:";
            // 
            // tabExport
            // 
            this.tabExport.Controls.Add(this.groupBox3);
            this.tabExport.Location = new System.Drawing.Point(4, 25);
            this.tabExport.Name = "tabExport";
            this.tabExport.Padding = new System.Windows.Forms.Padding(3);
            this.tabExport.Size = new System.Drawing.Size(676, 322);
            this.tabExport.TabIndex = 1;
            this.tabExport.Text = "تصدير البيانات";
            this.tabExport.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.btnExportBorrowings);
            this.groupBox3.Controls.Add(this.btnExportMembers);
            this.groupBox3.Controls.Add(this.btnExportBooks);
            this.groupBox3.Location = new System.Drawing.Point(8, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupBox3.Size = new System.Drawing.Size(660, 308);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "تصدير البيانات";
            // 
            // btnExportBorrowings
            // 
            this.btnExportBorrowings.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnExportBorrowings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(126)))), ((int)(((byte)(34)))));
            this.btnExportBorrowings.FlatAppearance.BorderSize = 0;
            this.btnExportBorrowings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportBorrowings.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportBorrowings.ForeColor = System.Drawing.Color.White;
            this.btnExportBorrowings.Location = new System.Drawing.Point(218, 202);
            this.btnExportBorrowings.Name = "btnExportBorrowings";
            this.btnExportBorrowings.Size = new System.Drawing.Size(225, 40);
            this.btnExportBorrowings.TabIndex = 2;
            this.btnExportBorrowings.Text = "تصدير بيانات الإعارات";
            this.btnExportBorrowings.UseVisualStyleBackColor = false;
            this.btnExportBorrowings.Click += new System.EventHandler(this.btnExportBorrowings_Click);
            // 
            // btnExportMembers
            // 
            this.btnExportMembers.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnExportMembers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnExportMembers.FlatAppearance.BorderSize = 0;
            this.btnExportMembers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportMembers.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportMembers.ForeColor = System.Drawing.Color.White;
            this.btnExportMembers.Location = new System.Drawing.Point(218, 129);
            this.btnExportMembers.Name = "btnExportMembers";
            this.btnExportMembers.Size = new System.Drawing.Size(225, 40);
            this.btnExportMembers.TabIndex = 1;
            this.btnExportMembers.Text = "تصدير بيانات الأعضاء";
            this.btnExportMembers.UseVisualStyleBackColor = false;
            this.btnExportMembers.Click += new System.EventHandler(this.btnExportMembers_Click);
            // 
            // btnExportBooks
            // 
            this.btnExportBooks.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnExportBooks.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnExportBooks.FlatAppearance.BorderSize = 0;
            this.btnExportBooks.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportBooks.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportBooks.ForeColor = System.Drawing.Color.White;
            this.btnExportBooks.Location = new System.Drawing.Point(218, 56);
            this.btnExportBooks.Name = "btnExportBooks";
            this.btnExportBooks.Size = new System.Drawing.Size(225, 40);
            this.btnExportBooks.TabIndex = 0;
            this.btnExportBooks.Text = "تصدير بيانات الكتب";
            this.btnExportBooks.UseVisualStyleBackColor = false;
            this.btnExportBooks.Click += new System.EventHandler(this.btnExportBooks_Click);
            // 
            // tabImport
            // 
            this.tabImport.Controls.Add(this.groupBox4);
            this.tabImport.Location = new System.Drawing.Point(4, 25);
            this.tabImport.Name = "tabImport";
            this.tabImport.Padding = new System.Windows.Forms.Padding(3);
            this.tabImport.Size = new System.Drawing.Size(676, 322);
            this.tabImport.TabIndex = 2;
            this.tabImport.Text = "استيراد البيانات";
            this.tabImport.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.btnImportMembers);
            this.groupBox4.Controls.Add(this.btnImportBooks);
            this.groupBox4.Location = new System.Drawing.Point(8, 6);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupBox4.Size = new System.Drawing.Size(660, 308);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "استيراد البيانات";
            // 
            // btnImportMembers
            // 
            this.btnImportMembers.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnImportMembers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnImportMembers.FlatAppearance.BorderSize = 0;
            this.btnImportMembers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImportMembers.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImportMembers.ForeColor = System.Drawing.Color.White;
            this.btnImportMembers.Location = new System.Drawing.Point(218, 156);
            this.btnImportMembers.Name = "btnImportMembers";
            this.btnImportMembers.Size = new System.Drawing.Size(225, 40);
            this.btnImportMembers.TabIndex = 1;
            this.btnImportMembers.Text = "استيراد بيانات الأعضاء";
            this.btnImportMembers.UseVisualStyleBackColor = false;
            this.btnImportMembers.Click += new System.EventHandler(this.btnImportMembers_Click);
            // 
            // btnImportBooks
            // 
            this.btnImportBooks.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnImportBooks.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnImportBooks.FlatAppearance.BorderSize = 0;
            this.btnImportBooks.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImportBooks.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImportBooks.ForeColor = System.Drawing.Color.White;
            this.btnImportBooks.Location = new System.Drawing.Point(218, 83);
            this.btnImportBooks.Name = "btnImportBooks";
            this.btnImportBooks.Size = new System.Drawing.Size(225, 40);
            this.btnImportBooks.TabIndex = 0;
            this.btnImportBooks.Text = "استيراد بيانات الكتب";
            this.btnImportBooks.UseVisualStyleBackColor = false;
            this.btnImportBooks.Click += new System.EventHandler(this.btnImportBooks_Click);
            // 
            // tabSettings
            // 
            this.tabSettings.Controls.Add(this.groupBox5);
            this.tabSettings.Location = new System.Drawing.Point(4, 25);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabSettings.Size = new System.Drawing.Size(676, 322);
            this.tabSettings.TabIndex = 3;
            this.tabSettings.Text = "الإعدادات";
            this.tabSettings.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.btnSaveSettings);
            this.groupBox5.Controls.Add(this.dtpScheduleTime);
            this.groupBox5.Controls.Add(this.lblScheduleTime);
            this.groupBox5.Controls.Add(this.lblDays);
            this.groupBox5.Controls.Add(this.numBackupDays);
            this.groupBox5.Controls.Add(this.chkAutoBackup);
            this.groupBox5.Location = new System.Drawing.Point(8, 6);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupBox5.Size = new System.Drawing.Size(660, 308);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "إعدادات النسخ الاحتياطي التلقائي";
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnSaveSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnSaveSettings.FlatAppearance.BorderSize = 0;
            this.btnSaveSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveSettings.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveSettings.ForeColor = System.Drawing.Color.White;
            this.btnSaveSettings.Location = new System.Drawing.Point(218, 202);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(225, 40);
            this.btnSaveSettings.TabIndex = 5;
            this.btnSaveSettings.Text = "حفظ الإعدادات";
            this.btnSaveSettings.UseVisualStyleBackColor = false;
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // dtpScheduleTime
            // 
            this.dtpScheduleTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpScheduleTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpScheduleTime.Location = new System.Drawing.Point(447, 151);
            this.dtpScheduleTime.Name = "dtpScheduleTime";
            this.dtpScheduleTime.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.dtpScheduleTime.ShowUpDown = true;
            this.dtpScheduleTime.Size = new System.Drawing.Size(96, 22);
            this.dtpScheduleTime.TabIndex = 4;
            // 
            // lblScheduleTime
            // 
            this.lblScheduleTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblScheduleTime.AutoSize = true;
            this.lblScheduleTime.Location = new System.Drawing.Point(549, 154);
            this.lblScheduleTime.Name = "lblScheduleTime";
            this.lblScheduleTime.Size = new System.Drawing.Size(62, 16);
            this.lblScheduleTime.TabIndex = 3;
            this.lblScheduleTime.Text = "وقت التنفيذ:";
            // 
            // lblDays
            // 
            this.lblDays.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDays.AutoSize = true;
            this.lblDays.Location = new System.Drawing.Point(549, 104);
            this.lblDays.Name = "lblDays";
            this.lblDays.Size = new System.Drawing.Size(105, 16);
            this.lblDays.TabIndex = 2;
            this.lblDays.Text = "التكرار كل (أيام):";
            // 
            // numBackupDays
            // 
            this.numBackupDays.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numBackupDays.Location = new System.Drawing.Point(477, 102);
            this.numBackupDays.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numBackupDays.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numBackupDays.Name = "numBackupDays";
            this.numBackupDays.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.numBackupDays.Size = new System.Drawing.Size(66, 22);
            this.numBackupDays.TabIndex = 1;
            this.numBackupDays.Value = new decimal(new int[] {
            7,
            0,
            0,
            0});
            // 
            // chkAutoBackup
            // 
            this.chkAutoBackup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkAutoBackup.AutoSize = true;
            this.chkAutoBackup.Location = new System.Drawing.Point(461, 50);
            this.chkAutoBackup.Name = "chkAutoBackup";
            this.chkAutoBackup.Size = new System.Drawing.Size(149, 20);
            this.chkAutoBackup.TabIndex = 0;
            this.chkAutoBackup.Text = "تفعيل النسخ الاحتياطي التلقائي";
            this.chkAutoBackup.UseVisualStyleBackColor = true;
            this.chkAutoBackup.CheckedChanged += new System.EventHandler(this.chkAutoBackup_CheckedChanged);
            // 
            // BackupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 411);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.MinimumSize = new System.Drawing.Size(700, 450);
            this.Name = "BackupForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "النسخ الاحتياطي والاستيراد/التصدير";
            this.Load += new System.EventHandler(this.BackupForm_Load);
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabBackup.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabExport.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.tabImport.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.tabSettings.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBackupDays)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabBackup;
        private System.Windows.Forms.TabPage tabExport;
        private System.Windows.Forms.TabPage tabImport;
        private System.Windows.Forms.TabPage tabSettings;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBackupPath;
        private System.Windows.Forms.Button btnBrowseBackupPath;
        private System.Windows.Forms.Button btnCreateBackup;
        private System.Windows.Forms.Button btnRestoreBackup;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnExportBooks;
        private System.Windows.Forms.Button btnExportMembers;
        private System.Windows.Forms.Button btnExportBorrowings;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnImportBooks;
        private System.Windows.Forms.Button btnImportMembers;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox chkAutoBackup;
        private System.Windows.Forms.Label lblDays;
        private System.Windows.Forms.NumericUpDown numBackupDays;
        private System.Windows.Forms.Label lblScheduleTime;
        private System.Windows.Forms.DateTimePicker dtpScheduleTime;
        private System.Windows.Forms.Button btnSaveSettings;
    }
}