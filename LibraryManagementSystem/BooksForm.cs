using System;
using System.Data;
using System.Windows.Forms;

namespace LibraryManagementSystem
{
    public partial class BooksForm : Form
    {
        private int selectedBookId = 0;

        public BooksForm()
        {
            InitializeComponent();
        }

        private void BooksForm_Load(object sender, EventArgs e)
        {
            // تحديث جدول الكتب عند تحميل النموذج
            RefreshBooksGrid();
            ClearFields();

            // إعداد خيارات البحث
            cmbSearchBy.Items.Add("العنوان");
            cmbSearchBy.Items.Add("المؤلف");
            cmbSearchBy.Items.Add("التصنيف");
            cmbSearchBy.Items.Add("ISBN");
            cmbSearchBy.SelectedIndex = 0;
        }
        private void BooksForm_Shown(object sender, EventArgs e)
        {
            // إعداد خيارات البحث
            cmbSearchBy.Items.Clear();
            cmbSearchBy.Items.Add("العنوان");
            cmbSearchBy.Items.Add("المؤلف");
            cmbSearchBy.Items.Add("التصنيف");
            cmbSearchBy.Items.Add("ISBN");
            cmbSearchBy.SelectedIndex = 0;
        }
        private void RefreshBooksGrid()
        {
            // عرض جميع الكتب في الجدول
            DataTable booksTable = DatabaseManager.GetAllBooks();
            dataGridViewBooks.DataSource = booksTable;

            // تحديد العناوين العربية للأعمدة
            dataGridViewBooks.Columns["BookID"].HeaderText = "رقم الكتاب";
            dataGridViewBooks.Columns["Title"].HeaderText = "العنوان";
            dataGridViewBooks.Columns["Author"].HeaderText = "المؤلف";
            dataGridViewBooks.Columns["Year"].HeaderText = "سنة النشر";
            dataGridViewBooks.Columns["Category"].HeaderText = "التصنيف";
            dataGridViewBooks.Columns["AvailableCopies"].HeaderText = "النسخ المتاحة";
            dataGridViewBooks.Columns["TotalCopies"].HeaderText = "إجمالي النسخ";
            dataGridViewBooks.Columns["ISBN"].HeaderText = "ISBN";

            // تنسيق عرض الجدول
            dataGridViewBooks.RightToLeft = RightToLeft.Yes;
            dataGridViewBooks.Columns["BookID"].Width = 80;
            dataGridViewBooks.Columns["Title"].Width = 200;
            dataGridViewBooks.Columns["Author"].Width = 150;
            dataGridViewBooks.Columns["Year"].Width = 80;
            dataGridViewBooks.Columns["Category"].Width = 100;
            dataGridViewBooks.Columns["AvailableCopies"].Width = 100;
            dataGridViewBooks.Columns["TotalCopies"].Width = 100;
            dataGridViewBooks.Columns["ISBN"].Width = 100;

            // تحديث عدد الكتب
            lblBooksCount.Text = $"عدد الكتب: {booksTable.Rows.Count}";
        }

        private void ClearFields()
        {
            // مسح الحقول
            txtTitle.Text = "";
            txtAuthor.Text = "";
            txtYear.Text = DateTime.Now.Year.ToString();
            txtCategory.Text = "";
            txtCopies.Text = "1";
            txtISBN.Text = "";

            // إعادة تعيين رقم الكتاب المحدد
            selectedBookId = 0;
            btnAdd.Text = "إضافة";
            btnDelete.Enabled = false;
        }

        private bool ValidateInput()
        {
            // التحقق من صحة البيانات المدخلة
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("يجب إدخال عنوان الكتاب", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTitle.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtAuthor.Text))
            {
                MessageBox.Show("يجب إدخال اسم المؤلف", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtAuthor.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtYear.Text) || !int.TryParse(txtYear.Text, out int year) || year < 0 || year > DateTime.Now.Year)
            {
                MessageBox.Show("يجب إدخال سنة نشر صحيحة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtYear.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtCopies.Text) || !int.TryParse(txtCopies.Text, out int copies) || copies <= 0)
            {
                MessageBox.Show("يجب إدخال عدد نسخ صحيح", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCopies.Focus();
                return false;
            }

            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            // تحويل البيانات المدخلة
            string title = txtTitle.Text.Trim();
            string author = txtAuthor.Text.Trim();
            int year = int.Parse(txtYear.Text);
            string category = txtCategory.Text.Trim();
            int copies = int.Parse(txtCopies.Text);
            string isbn = txtISBN.Text.Trim();

            bool success;

            if (selectedBookId == 0)
            {
                // إضافة كتاب جديد
                success = DatabaseManager.AddBook(title, author, year, category, copies, isbn);
                if (success)
                {
                    MessageBox.Show("تمت إضافة الكتاب بنجاح", "تأكيد", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                // تحديث كتاب موجود
                success = DatabaseManager.UpdateBook(selectedBookId, title, author, year, category, copies, isbn);
                if (success)
                {
                    MessageBox.Show("تم تحديث بيانات الكتاب بنجاح", "تأكيد", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            if (success)
            {
                // تحديث الجدول ومسح الحقول
                RefreshBooksGrid();
                ClearFields();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedBookId == 0)
            {
                MessageBox.Show("يرجى تحديد كتاب للحذف", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("هل أنت متأكد من حذف هذا الكتاب؟", "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                bool success = DatabaseManager.DeleteBook(selectedBookId);
                if (success)
                {
                    MessageBox.Show("تم حذف الكتاب بنجاح", "تأكيد", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshBooksGrid();
                    ClearFields();
                }
            }
        }

        private void dataGridViewBooks_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewBooks.Rows[e.RowIndex];

                // عرض بيانات الكتاب المحدد في الحقول
                selectedBookId = Convert.ToInt32(row.Cells["BookID"].Value);
                txtTitle.Text = row.Cells["Title"].Value.ToString();
                txtAuthor.Text = row.Cells["Author"].Value.ToString();
                txtYear.Text = row.Cells["Year"].Value.ToString();
                txtCategory.Text = row.Cells["Category"].Value.ToString();
                txtCopies.Text = row.Cells["TotalCopies"].Value.ToString();
                txtISBN.Text = row.Cells["ISBN"].Value.ToString();

                // تغيير زر الإضافة إلى تحديث
                btnAdd.Text = "تحديث";
                btnDelete.Enabled = true;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // البحث في الكتب أثناء الكتابة
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                RefreshBooksGrid();
            }
            else
            {
                string searchText = txtSearch.Text.Trim();
                string searchBy = cmbSearchBy.SelectedItem?.ToString() ?? "العنوان";

                DataTable searchResults = DatabaseManager.SearchBooks(searchText, searchBy);
                dataGridViewBooks.DataSource = searchResults;

                // تحديث عدد الكتب
                lblBooksCount.Text = $"عدد الكتب: {searchResults.Rows.Count}";
            }
        }
    }
}