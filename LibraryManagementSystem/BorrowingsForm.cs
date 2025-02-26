using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SQLite;

namespace LibraryManagementSystem
{
    public partial class BorrowingsForm : Form
    {
        private int selectedBorrowId = 0;

        public BorrowingsForm()
        {
            InitializeComponent();
        }

        private void BorrowingsForm_Load(object sender, EventArgs e)
        {
            // تحديث جدول الإعارات
            RefreshBorrowingsGrid();

            // تحميل قوائم الكتب والأعضاء
            LoadBooks();
            LoadMembers();

            // تعيين تاريخ الإرجاع الافتراضي (بعد 14 يوم)
            dateTimePicker1.Value = DateTime.Now.AddDays(14);
        }

        private void RefreshBorrowingsGrid()
        {
            // عرض جميع الإعارات في الجدول
            DataTable borrowingsTable = DatabaseManager.GetAllBorrowings();
            dataGridViewBorrowings.DataSource = borrowingsTable;

            // تحديد العناوين العربية للأعمدة
            dataGridViewBorrowings.Columns["BorrowID"].HeaderText = "رقم الإعارة";
            dataGridViewBorrowings.Columns["Title"].HeaderText = "الكتاب";
            dataGridViewBorrowings.Columns["Name"].HeaderText = "المستعير";
            dataGridViewBorrowings.Columns["BorrowDate"].HeaderText = "تاريخ الإعارة";
            dataGridViewBorrowings.Columns["DueDate"].HeaderText = "تاريخ الاستحقاق";
            dataGridViewBorrowings.Columns["ReturnDate"].HeaderText = "تاريخ الإرجاع";
            dataGridViewBorrowings.Columns["Status"].HeaderText = "الحالة";

            // تنسيق عرض الجدول
            dataGridViewBorrowings.RightToLeft = RightToLeft.Yes;
            dataGridViewBorrowings.Columns["BorrowID"].Width = 80;
            dataGridViewBorrowings.Columns["Title"].Width = 200;
            dataGridViewBorrowings.Columns["Name"].Width = 150;
            dataGridViewBorrowings.Columns["BorrowDate"].Width = 120;
            dataGridViewBorrowings.Columns["DueDate"].Width = 120;
            dataGridViewBorrowings.Columns["ReturnDate"].Width = 120;
            dataGridViewBorrowings.Columns["Status"].Width = 80;

            // تحديث عدد الإعارات
            lblBorrowingsCount.Text = $"عدد الإعارات: {borrowingsTable.Rows.Count}";
        }

        private void LoadBooks()
        {
            // تحميل قائمة الكتب المتاحة للإعارة
            using (SQLiteConnection connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                string query = "SELECT BookID, Title FROM Books WHERE AvailableCopies > 0";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    DataTable dataTable = new DataTable();
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }

                    // إعداد قائمة الكتب
                    cmbBooks.DisplayMember = "Title";
                    cmbBooks.ValueMember = "BookID";
                    cmbBooks.DataSource = dataTable;
                }
            }
        }

        private void LoadMembers()
        {
            // تحميل قائمة الأعضاء النشطين
            using (SQLiteConnection connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                string query = "SELECT MemberID, Name FROM Members WHERE Status = 'نشط'";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    DataTable dataTable = new DataTable();
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }

                    // إعداد قائمة الأعضاء
                    cmbMembers.DisplayMember = "Name";
                    cmbMembers.ValueMember = "MemberID";
                    cmbMembers.DataSource = dataTable;
                }
            }
        }

        private void btnAddBorrowing_Click(object sender, EventArgs e)
        {
            if (cmbBooks.SelectedValue == null || cmbMembers.SelectedValue == null)
            {
                MessageBox.Show("يرجى اختيار الكتاب والعضو", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int bookId = Convert.ToInt32(cmbBooks.SelectedValue);
            int memberId = Convert.ToInt32(cmbMembers.SelectedValue);
            DateTime dueDate = dateTimePicker1.Value;

            bool success = DatabaseManager.BorrowBook(bookId, memberId, dueDate);
            if (success)
            {
                MessageBox.Show("تمت إضافة الإعارة بنجاح", "تأكيد", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // تحديث الجدول وإعادة تحميل الكتب (لأن الكتب المتاحة تغيرت)
                RefreshBorrowingsGrid();
                LoadBooks();
            }
        }

        private void btnReturnBook_Click(object sender, EventArgs e)
        {
            if (selectedBorrowId == 0)
            {
                MessageBox.Show("يرجى تحديد إعارة للإرجاع", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("هل تريد إرجاع هذا الكتاب؟", "تأكيد الإرجاع", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                bool success = DatabaseManager.ReturnBook(selectedBorrowId);
                if (success)
                {
                    MessageBox.Show("تم إرجاع الكتاب بنجاح", "تأكيد", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // تحديث الجدول وإعادة تحميل الكتب
                    RefreshBorrowingsGrid();
                    LoadBooks();
                    selectedBorrowId = 0;
                    btnReturnBook.Enabled = false;
                }
            }
        }

        private void dataGridViewBorrowings_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewBorrowings.Rows[e.RowIndex];

                // تحديد معرف الإعارة المحدد
                selectedBorrowId = Convert.ToInt32(row.Cells["BorrowID"].Value);

                // تمكين زر الإرجاع فقط للكتب التي لم يتم إرجاعها بعد
                string status = row.Cells["Status"].Value.ToString();
                object returnDateValue = row.Cells["ReturnDate"].Value;
                bool isReturned = returnDateValue != DBNull.Value && !string.IsNullOrEmpty(returnDateValue.ToString());

                btnReturnBook.Enabled = !isReturned && status == "معار";
            }
        }
    }
}