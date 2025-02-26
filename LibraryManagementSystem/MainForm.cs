using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data.SqlTypes;
using System.Reflection.Emit;

namespace LibraryManagementSystem
{
    public partial class MainForm : Form
    {
        private int notificationCount = 0;
        private bool isFirstLoad = true;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                // تطبيق الثيم على النموذج
                ThemeManager.ApplyTheme(this);

                // تهيئة قاعدة البيانات عند بدء التطبيق
                if (isFirstLoad)
                {
                    LoadingOperation("جاري تحميل البيانات...", () =>
                    {
                        DatabaseManager.InitializeDatabase();
                    });
                    isFirstLoad = false;
                }

                // التحقق من وجود إعارات متأخرة
                CheckOverdueBooks();

                // عرض الإحصائيات في لوحة التحكم
                UpdateDashboardStats();

                // تحديث تاريخ وتوقيت اليوم
                UpdateDateTime();

                // تحديث بيانات المستخدم
                UpdateUserInfo();

                // بدء مؤقت لتحديث الوقت
                timerClock.Start();

                // التحقق من توفر الإصدار الجديد
                CheckForNewVersion();

                // تسجيل فتح النموذج الرئيسي
                AppConfig.LogToFile(AppConfig.LogLevel.Info, "تم فتح النموذج الرئيسي");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ أثناء تحميل النافذة الرئيسية: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AppConfig.LogToFile(AppConfig.LogLevel.Error, $"خطأ في تحميل النموذج الرئيسي: {ex.Message}");
            }
        }

        private void LoadingOperation(string message, Action operation)
        {
            // إظهار رسالة تحميل
            lblStatus.Text = message;
            lblStatus.Visible = true;
            progressBar.Visible = true;
            progressBar.Style = ProgressBarStyle.Marquee;

            // تحديث واجهة المستخدم
            Application.DoEvents();

            try
            {
                // تنفيذ العملية
                operation.Invoke();
            }
            finally
            {
                // إخفاء رسالة التحميل
                lblStatus.Visible = false;
                progressBar.Visible = false;
            }
        }

        private void CheckOverdueBooks()
        {
            try
            {
                // التحقق من وجود كتب متأخرة والإشعار بها
                int overdueCount = GetOverdueCount();
                if (overdueCount > 0)
                {
                    ShowNotification($"هناك {overdueCount} كتاب متأخر. يرجى مراجعة تقرير الكتب المتأخرة.");
                }
            }
            catch (Exception ex)
            {
                AppConfig.LogToFile(AppConfig.LogLevel.Error, $"خطأ في التحقق من الكتب المتأخرة: {ex.Message}");
            }
        }

        private int GetOverdueCount()
        {
            // الحصول على عدد الكتب المتأخرة (يمكن تنفيذها في DatabaseManager)
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                string query = @"
                    SELECT COUNT(*) 
                    FROM Borrowings 
                    WHERE ReturnDate IS NULL AND DueDate < @Today";

                using (var command = new System.Data.SQLite.SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Today", DateTime.Now.ToString("yyyy-MM-dd"));
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        private void UpdateDashboardStats()
        {
            try
            {
                var stats = DatabaseManager.GetStatistics();

                lblTotalBooks.Text = stats["إجمالي الكتب"].ToString();
                lblAvailableBooks.Text = stats["الكتب المتاحة"].ToString();
                lblBorrowedBooks.Text = stats["الكتب المعارة"].ToString();
                lblTotalMembers.Text = stats["إجمالي الأعضاء"].ToString();
                lblActiveMembers.Text = stats["الأعضاء النشطين"].ToString();
                lblTodayBorrowings.Text = stats["إعارات اليوم"].ToString();

                // تلوين الإحصائيات حسب الحالة
                if (Properties.Settings.Default.UseColorCoding)
                {
                    // تلوين الكتب المتاحة باللون الأخضر إذا كانت كل الكتب متاحة
                    if (stats["إجمالي الكتب"] > 0 && stats["الكتب المتاحة"] == stats["إجمالي الكتب"])
                    {
                        lblAvailableBooks.ForeColor = Color.DarkGreen;
                    }

                    // تلوين الكتب المعارة باللون الأحمر إذا كانت جميع الكتب معارة
                    if (stats["إجمالي الكتب"] > 0 && stats["الكتب المعارة"] == stats["إجمالي الكتب"])
                    {
                        lblBorrowedBooks.ForeColor = Color.DarkRed;
                    }
                }
            }
            catch (Exception ex)
            {
                AppConfig.LogToFile(AppConfig.LogLevel.Error, $"خطأ في تحديث إحصائيات لوحة التحكم: {ex.Message}");
            }
        }

        private void UpdateDateTime()
        {
            lblDateTime.Text = DateTime.Now.ToString("dddd, dd/MM/yyyy hh:mm:ss tt");
        }

        private void UpdateUserInfo()
        {
            lblUser.Text = $"المستخدم: {Environment.UserName}";
        }

        private void CheckForNewVersion()
        {
            // التحقق من وجود إصدار جديد (يمكن تنفيذها في تحديث قادم)
            // هذه مجرد محاكاة لعرض إشعار
            if (DateTime.Now.Day == 1) // على سبيل المثال: التحقق في اليوم الأول من كل شهر
            {
                ShowNotification("يتوفر إصدار جديد من التطبيق. هل ترغب في التحديث؟", true);
            }
        }

        private void ShowNotification(string message, bool isImportant = false)
        {
            try
            {
                // إضافة إشعار للمستخدم
                notificationCount++;

                Panel notificationPanel = new Panel();
                notificationPanel.BackColor = isImportant ? Color.FromArgb(255, 200, 200) : Color.FromArgb(230, 230, 255);
                notificationPanel.BorderStyle = BorderStyle.FixedSingle;
                notificationPanel.Size = new Size(250, 60);
                notificationPanel.Location = new Point(20, 60 + (notificationCount - 1) * 70);

                Label lblMessage = new Label();
                lblMessage.Text = message;
                lblMessage.Font = new Font("Arial", 9);
                lblMessage.AutoSize = false;
                lblMessage.Size = new Size(230, 40);
                lblMessage.Location = new Point(10, 10);
                lblMessage.TextAlign = ContentAlignment.MiddleCenter;

                Button btnClose = new Button();
                btnClose.Text = "×";
                btnClose.Size = new Size(20, 20);
                btnClose.Location = new Point(notificationPanel.Width - 25, 5);
                btnClose.FlatStyle = FlatStyle.Flat;
                btnClose.FlatAppearance.BorderSize = 0;
                btnClose.Click += (s, e) => { notificationPanel.Dispose(); };

                notificationPanel.Controls.Add(lblMessage);
                notificationPanel.Controls.Add(btnClose);
                panelNotifications.Controls.Add(notificationPanel);

                // إظهار لوحة الإشعارات إذا كانت مخفية
                panelNotifications.Visible = true;

                // إخفاء الإشعار تلقائياً بعد مدة
                Timer timer = new Timer();
                timer.Interval = 10000; // 10 ثواني
                timer.Tick += (s, e) =>
                {
                    notificationPanel.Dispose();
                    timer.Dispose();

                    // إخفاء لوحة الإشعارات إذا لم يعد بها إشعارات
                    if (panelNotifications.Controls.Count == 0)
                    {
                        panelNotifications.Visible = false;
                        notificationCount = 0;
                    }
                };
                timer.Start();
            }
            catch (Exception ex)
            {
                AppConfig.LogToFile(AppConfig.LogLevel.Error, $"خطأ في عرض الإشعار: {ex.Message}");
            }
        }

        private void btnManageBooks_Click(object sender, EventArgs e)
        {
            try
            {
                BooksForm booksForm = new BooksForm();
                booksForm.ShowDialog();

                // تحديث الإحصائيات بعد إغلاق نموذج إدارة الكتب
                UpdateDashboardStats();

                AppConfig.LogToFile(AppConfig.LogLevel.Info, "تم فتح نموذج إدارة الكتب");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ أثناء فتح نموذج إدارة الكتب: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AppConfig.LogToFile(AppConfig.LogLevel.Error, $"خطأ في فتح نموذج إدارة الكتب: {ex.Message}");
            }
        }

        private void btnManageMembers_Click(object sender, EventArgs e)
        {
            try
            {
                MembersForm membersForm = new MembersForm();
                membersForm.ShowDialog();

                // تحديث الإحصائيات بعد إغلاق نموذج إدارة الأعضاء
                UpdateDashboardStats();

                AppConfig.LogToFile(AppConfig.LogLevel.Info, "تم فتح نموذج إدارة الأعضاء");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ أثناء فتح نموذج إدارة الأعضاء: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AppConfig.LogToFile(AppConfig.LogLevel.Error, $"خطأ في فتح نموذج إدارة الأعضاء: {ex.Message}");
            }
        }

        private void btnManageBorrowings_Click(object sender, EventArgs e)
        {
            try
            {
                BorrowingsForm borrowingsForm = new BorrowingsForm();
                borrowingsForm.ShowDialog();

                // تحديث الإحصائيات بعد إغلاق نموذج إدارة الإعارات
                UpdateDashboardStats();

                AppConfig.LogToFile(AppConfig.LogLevel.Info, "تم فتح نموذج إدارة الإعارات");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ أثناء فتح نموذج إدارة الإعارات: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AppConfig.LogToFile(AppConfig.LogLevel.Error, $"خطأ في فتح نموذج إدارة الإعارات: {ex.Message}");
            }
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            try
            {
                ReportsForm reportsForm = new ReportsForm();
                reportsForm.ShowDialog();

                AppConfig.LogToFile(AppConfig.LogLevel.Info, "تم فتح نموذج التقارير");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ أثناء فتح نموذج التقارير: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AppConfig.LogToFile(AppConfig.LogLevel.Error, $"خطأ في فتح نموذج التقارير: {ex.Message}");
            }
        }

        private void btnSearchBooks_Click(object sender, EventArgs e)
        {
            try
            {
                SearchBooksForm searchForm = new SearchBooksForm();
                searchForm.ShowDialog();

                AppConfig.LogToFile(AppConfig.LogLevel.Info, "تم فتح نموذج البحث عن الكتب");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ أثناء فتح نموذج البحث عن الكتب: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AppConfig.LogToFile(AppConfig.LogLevel.Error, $"خطأ في فتح نموذج البحث عن الكتب: {ex.Message}");
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            try
            {
                SettingsForm settingsForm = new SettingsForm();
                if (settingsForm.ShowDialog() == DialogResult.OK)
                {
                    // تطبيق الثيم الجديد على النموذج الرئيسي
                    ThemeManager.ApplyTheme(this);

                    // تحديث اسم التطبيق
                    this.Text = AppConfig.AppName;
                    label1.Text = AppConfig.AppName;
                }

                AppConfig.LogToFile(AppConfig.LogLevel.Info, "تم فتح نموذج الإعدادات");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ أثناء فتح نموذج الإعدادات: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AppConfig.LogToFile(AppConfig.LogLevel.Error, $"خطأ في فتح نموذج الإعدادات: {ex.Message}");
            }
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            try
            {
                BackupForm backupForm = new BackupForm();
                backupForm.ShowDialog();

                AppConfig.LogToFile(AppConfig.LogLevel.Info, "تم فتح نموذج النسخ الاحتياطي");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ أثناء فتح نموذج النسخ الاحتياطي: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AppConfig.LogToFile(AppConfig.LogLevel.Error, $"خطأ في فتح نموذج النسخ الاحتياطي: {ex.Message}");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                // إظهار رسالة تأكيد
                DialogResult result = MessageBox.Show("هل أنت متأكد من الخروج من البرنامج؟", "تأكيد الخروج",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    AppConfig.LogToFile(AppConfig.LogLevel.Info, "إغلاق التطبيق بواسطة المستخدم");
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                AppConfig.LogToFile(AppConfig.LogLevel.Error, $"خطأ في إغلاق التطبيق: {ex.Message}");
                Application.Exit();
            }
        }

        private void timerClock_Tick(object sender, EventArgs e)
        {
            // تحديث الوقت
            UpdateDateTime();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // تسجيل إغلاق النموذج
            AppConfig.LogToFile(AppConfig.LogLevel.Info, "تم إغلاق النموذج الرئيسي");
        }
    }
}