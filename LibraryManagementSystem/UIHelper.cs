// UIHelper.cs - فئة لتوحيد أساليب واجهة المستخدم المتكررة
using System;
using System.Data;
using System.Windows.Forms;
using System.Drawing;

namespace LibraryManagementSystem
{
    public static class UIHelper
    {
        // أسلوب لتهيئة جدول DataGridView للكتب
        public static void InitializeBooksDataGridView(DataGridView dataGridView, DataTable dataTable)
        {
            dataGridView.DataSource = dataTable;

            // تحديد العناوين العربية للأعمدة
            dataGridView.Columns["BookID"]!.HeaderText = "رقم الكتاب";
            dataGridView.Columns["Title"].HeaderText = "العنوان";
            dataGridView.Columns["Author"].HeaderText = "المؤلف";
            dataGridView.Columns["Year"].HeaderText = "سنة النشر";
            dataGridView.Columns["Category"].HeaderText = "التصنيف";
            dataGridView.Columns["AvailableCopies"].HeaderText = "النسخ المتاحة";
            dataGridView.Columns["TotalCopies"].HeaderText = "إجمالي النسخ";
            dataGridView.Columns["ISBN"].HeaderText = "ISBN";

            // تنسيق عرض الجدول
            dataGridView.RightToLeft = RightToLeft.Yes;

            // ضبط أحجام الأعمدة
            SetColumnWidth(dataGridView, "BookID", 80);
            SetColumnWidth(dataGridView, "Title", 200);
            SetColumnWidth(dataGridView, "Author", 150);
            SetColumnWidth(dataGridView, "Year", 80);
            SetColumnWidth(dataGridView, "Category", 100);
            SetColumnWidth(dataGridView, "AvailableCopies", 100);
            SetColumnWidth(dataGridView, "TotalCopies", 100);
            SetColumnWidth(dataGridView, "ISBN", 100);
        }

        // أسلوب لتهيئة جدول DataGridView للأعضاء
        public static void InitializeMembersDataGridView(DataGridView dataGridView, DataTable dataTable)
        {
            dataGridView.DataSource = dataTable;

            // تحديد العناوين العربية للأعمدة
            dataGridView.Columns["MemberID"].HeaderText = "رقم العضو";
            dataGridView.Columns["Name"].HeaderText = "الاسم";
            dataGridView.Columns["Phone"].HeaderText = "الهاتف";
            dataGridView.Columns["Email"].HeaderText = "البريد الإلكتروني";
            dataGridView.Columns["Address"].HeaderText = "العنوان";
            dataGridView.Columns["RegistrationDate"].HeaderText = "تاريخ التسجيل";
            dataGridView.Columns["Status"].HeaderText = "الحالة";

            // تنسيق عرض الجدول
            dataGridView.RightToLeft = RightToLeft.Yes;

            // ضبط أحجام الأعمدة
            SetColumnWidth(dataGridView, "MemberID", 80);
            SetColumnWidth(dataGridView, "Name", 150);
            SetColumnWidth(dataGridView, "Phone", 100);
            SetColumnWidth(dataGridView, "Email", 150);
            SetColumnWidth(dataGridView, "Address", 200);
            SetColumnWidth(dataGridView, "RegistrationDate", 100);
            SetColumnWidth(dataGridView, "Status", 80);
        }

        // أسلوب لتهيئة جدول DataGridView للإعارات
        public static void InitializeBorrowingsDataGridView(DataGridView dataGridView, DataTable dataTable)
        {
            dataGridView.DataSource = dataTable;

            // تحديد العناوين العربية للأعمدة
            dataGridView.Columns["BorrowID"].HeaderText = "رقم الإعارة";
            dataGridView.Columns["Title"].HeaderText = "الكتاب";
            dataGridView.Columns["Name"].HeaderText = "المستعير";
            dataGridView.Columns["BorrowDate"].HeaderText = "تاريخ الإعارة";
            dataGridView.Columns["DueDate"].HeaderText = "تاريخ الاستحقاق";
            dataGridView.Columns["ReturnDate"].HeaderText = "تاريخ الإرجاع";
            dataGridView.Columns["Status"].HeaderText = "الحالة";

            // تنسيق عرض الجدول
            dataGridView.RightToLeft = RightToLeft.Yes;

            // ضبط أحجام الأعمدة
            SetColumnWidth(dataGridView, "BorrowID", 80);
            SetColumnWidth(dataGridView, "Title", 200);
            SetColumnWidth(dataGridView, "Name", 150);
            SetColumnWidth(dataGridView, "BorrowDate", 120);
            SetColumnWidth(dataGridView, "DueDate", 120);
            SetColumnWidth(dataGridView, "ReturnDate", 120);
            SetColumnWidth(dataGridView, "Status", 80);
        }

        // أسلوب مساعد لتعيين عرض العمود إذا كان موجوداً
        private static void SetColumnWidth(DataGridView dataGridView, string columnName, int width)
        {
            if (dataGridView.Columns[columnName] != null)
            {
                dataGridView.Columns[columnName].Width = width;
            }
        }

        // أسلوب لتنسيق الخلايا بألوان مختلفة حسب القيمة
        public static void ApplyCellFormatting(DataGridView dataGridView, string columnName, Func<object, bool> condition, Color backColor)
        {
            dataGridView.CellFormatting += (sender, e) =>
            {
                if (dataGridView.Columns[e.ColumnIndex].Name == columnName && e.RowIndex >= 0)
                {
                    var cellValue = dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                    if (cellValue != null && condition(cellValue))
                    {
                        e.CellStyle.BackColor = backColor;
                    }
                }
            };
        }

        // أسلوب لتعبئة مربع القائمة المنسدلة بخيارات البحث
        public static void InitializeSearchComboBox(ComboBox comboBox)
        {
            comboBox.Items.Clear();
            comboBox.Items.Add("العنوان");
            comboBox.Items.Add("المؤلف");
            comboBox.Items.Add("التصنيف");
            comboBox.Items.Add("ISBN");
            comboBox.SelectedIndex = 0;
        }

        // أسلوب لتعبئة مربع القائمة المنسدلة بحالات العضوية
        public static void InitializeMemberStatusComboBox(ComboBox comboBox)
        {
            comboBox.Items.Clear();
            comboBox.Items.Add("نشط");
            comboBox.Items.Add("موقوف");
            comboBox.Items.Add("منتهي");
            comboBox.SelectedIndex = 0;
        }

        // أسلوب لإعداد شكل الأزرار
        public static void SetupButton(Button button, Color backColor, string text)
        {
            button.BackColor = backColor;
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Text = text;
        }

        // أسلوب لإظهار رسالة تأكيد وانتظار نتيجة
        public static bool ShowConfirmationMessage(string message, string title = "تأكيد")
        {
            DialogResult result = MessageBox.Show(
                message,
                title,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            return result == DialogResult.Yes;
        }

        // أسلوب لإظهار رسالة نجاح
        public static void ShowSuccessMessage(string message, string title = "تم بنجاح")
        {
            MessageBox.Show(
                message,
                title,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        // أسلوب لإظهار رسالة خطأ
        public static void ShowErrorMessage(string message, string title = "خطأ")
        {
            MessageBox.Show(
                message,
                title,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }

        // أسلوب لإظهار رسالة تحذير
        public static void ShowWarningMessage(string message, string title = "تحذير")
        {
            MessageBox.Show(
                message,
                title,
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
        }

        // أسلوب لتنسيق DataGridView بالكامل
        public static void FormatDataGridView(DataGridView dataGridView)
        {
            dataGridView.BorderStyle = BorderStyle.None;
            dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(238, 239, 249);
            dataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(208, 226, 255);
            dataGridView.DefaultCellStyle.SelectionForeColor = Color.Black;
            dataGridView.BackgroundColor = Color.White;

            dataGridView.EnableHeadersVisualStyles = false;
            dataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 122, 204);
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10F, FontStyle.Bold);
        }

        // أسلوب للتحقق من العناصر المحددة في DataGridView
        public static bool IsRowSelected(DataGridView dataGridView, out DataGridViewRow selectedRow)
        {
            selectedRow = null;

            if (dataGridView.SelectedRows.Count > 0)
            {
                selectedRow = dataGridView.SelectedRows[0];
                return true;
            }

            return false;
        }

        // أسلوب لتهيئة مجموعة من TextBoxes للتحقق من المدخلات
        public static void SetupRequiredTextBoxes(params TextBox[] textBoxes)
        {
            foreach (var textBox in textBoxes)
            {
                textBox.TextChanged += (sender, e) =>
                {
                    if (string.IsNullOrWhiteSpace(textBox.Text))
                    {
                        textBox.BackColor = Color.MistyRose;
                    }
                    else
                    {
                        textBox.BackColor = SystemColors.Window;
                    }
                };
            }
        }

        // تعيين الفلتر للوثائق فقط
        public static void SetDocumentFilter(OpenFileDialog dialog)
        {
            dialog.Filter = "مستندات Word (*.docx)|*.docx|ملفات PDF (*.pdf)|*.pdf|جميع الملفات (*.*)|*.*";
            dialog.Title = "فتح وثيقة";
        }

        // تعيين الفلتر للصور فقط
        public static void SetImageFilter(OpenFileDialog dialog)
        {
            dialog.Filter = "ملفات الصور (*.png;*.jpg;*.jpeg;*.bmp;*.gif)|*.png;*.jpg;*.jpeg;*.bmp;*.gif|جميع الملفات (*.*)|*.*";
            dialog.Title = "فتح صورة";
        }
    }
}