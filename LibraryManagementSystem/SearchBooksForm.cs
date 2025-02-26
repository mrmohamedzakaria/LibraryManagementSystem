using System;
using System.Data;
using System.Windows.Forms;

namespace LibraryManagementSystem
{
    public partial class SearchBooksForm : Form
    {
        public SearchBooksForm()
        {
            InitializeComponent();
        }

        private void SearchBooksForm_Load(object sender, EventArgs e)
        {
            // إعداد خيارات البحث
            cmbSearchBy.Items.Add("العنوان");
            cmbSearchBy.Items.Add("المؤلف");
            cmbSearchBy.Items.Add("التصنيف");
            cmbSearchBy.Items.Add("ISBN");
            cmbSearchBy.SelectedIndex = 0;

            // عرض جميع الكتب افتراضياً
            RefreshBooksGrid();
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

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            SearchBooks();
        }

        private void cmbSearchBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchBooks();
        }

        private void SearchBooks()
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

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            cmbSearchBy.SelectedIndex = 0;
            RefreshBooksGrid();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            MessageBox.Show("سيتم تنفيذ خاصية الطباعة في الإصدار القادم", "معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}