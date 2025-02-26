// BackupForm.cs
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace LibraryManagementSystem
{
    public partial class BackupForm : Form
    {
        public BackupForm()
        {
            InitializeComponent();
        }

        private void BackupForm_Load(object sender, EventArgs e)
        {
            // تطبيق الثيم على النموذج
            ThemeManager.ApplyTheme(this);

            // تعيين مسار النسخ الاحتياطي الافتراضي
            txtBackupPath.Text = Properties.Settings.Default.BackupPath;
            if (string.IsNullOrEmpty(txtBackupPath.Text))
            {
                txtBackupPath.Text = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "LibraryBackups"
                );
            }

            // تعيين التاريخ والوقت الحاليين
            dtpScheduleTime.Value = DateTime.Now.AddHours(1);

            // تحميل الإعدادات المحفوظة
            chkAutoBackup.Checked = Properties.Settings.Default.AutoBackup;
            numBackupDays.Value = Properties.Settings.Default.BackupDays > 0 ?
                Properties.Settings.Default.BackupDays : 7;

            // تحديث حالة عناصر التحكم
            UpdateControlsState();
        }

        private void UpdateControlsState()
        {
            // تفعيل أو تعطيل العناصر المرتبطة بالنسخ الاحتياطي التلقائي
            numBackupDays.Enabled = chkAutoBackup.Checked;
            dtpScheduleTime.Enabled = chkAutoBackup.Checked;
            lblDays.Enabled = chkAutoBackup.Checked;
            lblScheduleTime.Enabled = chkAutoBackup.Checked;
        }

        private void btnBrowseBackupPath_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "اختر مجلد النسخ الاحتياطي";
                folderDialog.SelectedPath = txtBackupPath.Text;

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    txtBackupPath.Text = folderDialog.SelectedPath;
                }
            }
        }

        private void btnCreateBackup_Click(object sender, EventArgs e)
        {
            try
            {
                // التأكد من وجود المجلد
                if (!Directory.Exists(txtBackupPath.Text))
                {
                    Directory.CreateDirectory(txtBackupPath.Text);
                }

                // إنشاء اسم ملف النسخ الاحتياطي
                string backupFileName = $"LibraryBackup_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.lbk";
                string backupFilePath = Path.Combine(txtBackupPath.Text, backupFileName);

                // إظهار مؤشر الانتظار
                Cursor = Cursors.WaitCursor;

                // إنشاء النسخة الاحتياطية
                bool success = DatabaseManager.ExportFullDatabase(backupFilePath);

                // إعادة المؤشر إلى الوضع الطبيعي
                Cursor = Cursors.Default;

                if (success)
                {
                    UIHelper.ShowSuccessMessage("تم إنشاء النسخة الاحتياطية بنجاح");

                    // تحديث تاريخ آخر نسخة احتياطية
                    Properties.Settings.Default.LastBackupDate = DateTime.Now;
                    Properties.Settings.Default.Save();
                }
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                UIHelper.ShowErrorMessage($"حدث خطأ أثناء إنشاء النسخة الاحتياطية: {ex.Message}");
            }
        }

        private void btnRestoreBackup_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "ملفات النسخ الاحتياطي (*.lbk)|*.lbk|جميع الملفات (*.*)|*.*";
                openFileDialog.Title = "اختر ملف النسخة الاحتياطية";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // التحقق من تأكيد المستخدم
                        bool confirmed = UIHelper.ShowConfirmationMessage(
                            "سيؤدي استعادة النسخة الاحتياطية إلى حذف جميع البيانات الحالية. هل أنت متأكد من المتابعة؟",
                            "تأكيد الاستعادة"
                        );

                        if (confirmed)
                        {
                            // إظهار مؤشر الانتظار
                            Cursor = Cursors.WaitCursor;

                            // استعادة النسخة الاحتياطية
                            bool success = DatabaseManager.ImportFullDatabase(openFileDialog.FileName);

                            // إعادة المؤشر إلى الوضع الطبيعي
                            Cursor = Cursors.Default;

                            if (success)
                            {
                                UIHelper.ShowSuccessMessage("تم استعادة النسخة الاحتياطية بنجاح");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Cursor = Cursors.Default;
                        UIHelper.ShowErrorMessage($"حدث خطأ أثناء استعادة النسخة الاحتياطية: {ex.Message}");
                    }
                }
            }
        }

        private void btnExportBooks_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "ملفات CSV (*.csv)|*.csv|ملفات Excel (*.xlsx)|*.xlsx";
                saveFileDialog.Title = "تصدير بيانات الكتب";
                saveFileDialog.FileName = $"Books_{DateTime.Now:yyyy-MM-dd}.csv";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // إظهار مؤشر الانتظار
                        Cursor = Cursors.WaitCursor;

                        bool success = false;

                        if (Path.GetExtension(saveFileDialog.FileName).ToLower() == ".csv")
                        {
                            success = DatabaseManager.ExportBooksToCSV(saveFileDialog.FileName);
                        }
                        else
                        {
                            // استخدم طريقة Excel إذا كان الامتداد xlsx
                            DataTable booksTable = DatabaseManager.GetAllBooks();
                            success = ExportImportManager.ExportToExcel(booksTable, saveFileDialog.FileName);
                        }

                        // إعادة المؤشر إلى الوضع الطبيعي
                        Cursor = Cursors.Default;

                        if (success)
                        {
                            UIHelper.ShowSuccessMessage("تم تصدير بيانات الكتب بنجاح");
                        }
                    }
                    catch (Exception ex)
                    {
                        Cursor = Cursors.Default;
                        UIHelper.ShowErrorMessage($"حدث خطأ أثناء تصدير بيانات الكتب: {ex.Message}");
                    }
                }
            }
        }

        private void btnExportMembers_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "ملفات CSV (*.csv)|*.csv|ملفات Excel (*.xlsx)|*.xlsx";
                saveFileDialog.Title = "تصدير بيانات الأعضاء";
                saveFileDialog.FileName = $"Members_{DateTime.Now:yyyy-MM-dd}.csv";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // إظهار مؤشر الانتظار
                        Cursor = Cursors.WaitCursor;

                        bool success = false;

                        if (Path.GetExtension(saveFileDialog.FileName).ToLower() == ".csv")
                        {
                            success = DatabaseManager.ExportMembersToCSV(saveFileDialog.FileName);
                        }
                        else
                        {
                            // استخدم طريقة Excel إذا كان الامتداد xlsx
                            DataTable membersTable = DatabaseManager.GetAllMembers();
                            success = ExportImportManager.ExportToExcel(membersTable, saveFileDialog.FileName);
                        }

                        // إعادة المؤشر إلى الوضع الطبيعي
                        Cursor = Cursors.Default;

                        if (success)
                        {
                            UIHelper.ShowSuccessMessage("تم تصدير بيانات الأعضاء بنجاح");
                        }
                    }
                    catch (Exception ex)
                    {
                        Cursor = Cursors.Default;
                        UIHelper.ShowErrorMessage($"حدث خطأ أثناء تصدير بيانات الأعضاء: {ex.Message}");
                    }
                }
            }
        }

        private void btnExportBorrowings_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "ملفات CSV (*.csv)|*.csv|ملفات Excel (*.xlsx)|*.xlsx";
                saveFileDialog.Title = "تصدير بيانات الإعارات";
                saveFileDialog.FileName = $"Borrowings_{DateTime.Now:yyyy-MM-dd}.csv";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // إظهار مؤشر الانتظار
                        Cursor = Cursors.WaitCursor;

                        bool success = false;

                        if (Path.GetExtension(saveFileDialog.FileName).ToLower() == ".csv")
                        {
                            success = DatabaseManager.ExportBorrowingsToCSV(saveFileDialog.FileName);
                        }
                        else
                        {
                            // استخدم طريقة Excel إذا كان الامتداد xlsx
                            DataTable borrowingsTable = DatabaseManager.GetAllBorrowings();
                            success = ExportImportManager.ExportToExcel(borrowingsTable, saveFileDialog.FileName);
                        }

                        // إعادة المؤشر إلى الوضع الطبيعي
                        Cursor = Cursors.Default;

                        if (success)
                        {
                            UIHelper.ShowSuccessMessage("تم تصدير بيانات الإعارات بنجاح");
                        }
                    }
                    catch (Exception ex)
                    {
                        Cursor = Cursors.Default;
                        UIHelper.ShowErrorMessage($"حدث خطأ أثناء تصدير بيانات الإعارات: {ex.Message}");
                    }
                }
            }
        }

        private void btnImportBooks_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "ملفات CSV (*.csv)|*.csv|ملفات Excel (*.xlsx)|*.xlsx";
                openFileDialog.Title = "استيراد بيانات الكتب";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // التحقق من تأكيد المستخدم
                        bool confirmed = UIHelper.ShowConfirmationMessage(
                            "سيتم إضافة الكتب الجديدة إلى قاعدة البيانات. هل تريد المتابعة؟",
                            "تأكيد الاستيراد"
                        );

                        if (confirmed)
                        {
                            // إظهار مؤشر الانتظار
                            Cursor = Cursors.WaitCursor;

                            bool success = false;

                            if (Path.GetExtension(openFileDialog.FileName).ToLower() == ".csv")
                            {
                                success = ExportImportManager.ImportBooksFromCSV(openFileDialog.FileName);
                            }
                            else
                            {
                                // استخدام طريقة Excel إذا كان الامتداد xlsx
                                DataTable booksTable = ExportImportManager.ImportFromExcel(openFileDialog.FileName);
                                if (booksTable != null)
                                {
                                    // استيراد الكتب من الجدول
                                    // يمكن تنفيذ هذا الجزء عند الحاجة
                                    success = true;
                                }
                            }

                            // إعادة المؤشر إلى الوضع الطبيعي
                            Cursor = Cursors.Default;

                            if (success)
                            {
                                UIHelper.ShowSuccessMessage("تم استيراد بيانات الكتب بنجاح");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Cursor = Cursors.Default;
                        UIHelper.ShowErrorMessage($"حدث خطأ أثناء استيراد بيانات الكتب: {ex.Message}");
                    }
                }
            }
        }

        private void btnImportMembers_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "ملفات CSV (*.csv)|*.csv|ملفات Excel (*.xlsx)|*.xlsx";
                openFileDialog.Title = "استيراد بيانات الأعضاء";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // التحقق من تأكيد المستخدم
                        bool confirmed = UIHelper.ShowConfirmationMessage(
                            "سيتم إضافة الأعضاء الجدد إلى قاعدة البيانات. هل تريد المتابعة؟",
                            "تأكيد الاستيراد"
                        );

                        if (confirmed)
                        {
                            // إظهار مؤشر الانتظار
                            Cursor = Cursors.WaitCursor;

                            bool success = false;

                            if (Path.GetExtension(openFileDialog.FileName).ToLower() == ".csv")
                            {
                                success = ExportImportManager.ImportMembersFromCSV(openFileDialog.FileName);
                            }
                            else
                            {
                                // استخدام طريقة Excel إذا كان الامتداد xlsx
                                DataTable membersTable = ExportImportManager.ImportFromExcel(openFileDialog.FileName);
                                if (membersTable != null)
                                {
                                    // استيراد الأعضاء من الجدول
                                    // يمكن تنفيذ هذا الجزء عند الحاجة
                                    success = true;
                                }
                            }

                            // إعادة المؤشر إلى الوضع الطبيعي
                            Cursor = Cursors.Default;

                            if (success)
                            {
                                UIHelper.ShowSuccessMessage("تم استيراد بيانات الأعضاء بنجاح");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Cursor = Cursors.Default;
                        UIHelper.ShowErrorMessage($"حدث خطأ أثناء استيراد بيانات الأعضاء: {ex.Message}");
                    }
                }
            }
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            try
            {
                // حفظ إعدادات النسخ الاحتياطي
                Properties.Settings.Default.BackupPath = txtBackupPath.Text;
                Properties.Settings.Default.AutoBackup = chkAutoBackup.Checked;
                Properties.Settings.Default.BackupDays = (int)numBackupDays.Value;
                Properties.Settings.Default.Save();

                UIHelper.ShowSuccessMessage("تم حفظ الإعدادات بنجاح");

                // تنشيط أو إلغاء تنشيط المهمة المجدولة للنسخ الاحتياطي
                if (chkAutoBackup.Checked)
                {
                    AppConfig.SetupAutoBackup((int)numBackupDays.Value, dtpScheduleTime.Value.TimeOfDay);
                }
                else
                {
                    AppConfig.DisableAutoBackup();
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowErrorMessage($"حدث خطأ أثناء حفظ الإعدادات: {ex.Message}");
            }
        }

        private void chkAutoBackup_CheckedChanged(object sender, EventArgs e)
        {
            // تحديث حالة عناصر التحكم
            UpdateControlsState();
        }
    }
}