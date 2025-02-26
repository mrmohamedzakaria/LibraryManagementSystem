using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Collections.Generic;

namespace LibraryManagementSystem
{
    public partial class ReportsForm : Form
    {
        public ReportsForm()
        {
            InitializeComponent();
        }

        private void ReportsForm_Load(object sender, EventArgs e)
        {
            // تحميل بيانات التقارير
            LoadStatistics();

            // تحميل قائمة التقارير
            LoadReportsList();

            // تحديد التقرير الافتراضي
            cmbReportType.SelectedIndex = 0;
        }

        private void LoadStatistics()
        {
            // عرض الإحصائيات العامة
            var stats = DatabaseManager.GetStatistics();

            lblTotalBooks.Text = stats["إجمالي الكتب"].ToString();
            lblAvailableBooks.Text = stats["الكتب المتاحة"].ToString();
            lblBorrowedBooks.Text = stats["الكتب المعارة"].ToString();
            lblTotalMembers.Text = stats["إجمالي الأعضاء"].ToString();
            lblActiveMembers.Text = stats["الأعضاء النشطين"].ToString();
            lblTodayBorrowings.Text = stats["إعارات اليوم"].ToString();
        }

        private void LoadReportsList()
        {
            // تحميل قائمة التقارير المتاحة
            cmbReportType.Items.Add("أكثر الكتب إعارة");
            cmbReportType.Items.Add("أكثر الأعضاء استعارة");
            cmbReportType.Items.Add("الكتب المتأخرة");
            cmbReportType.Items.Add("الكتب حسب التصنيف");
            cmbReportType.Items.Add("الإعارات اليومية");
        }

        private void btnShowReport_Click(object sender, EventArgs e)
        {
            if (cmbReportType.SelectedIndex == -1)
            {
                MessageBox.Show("يرجى اختيار نوع التقرير", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // عرض التقرير المطلوب
            int reportIndex = cmbReportType.SelectedIndex;
            LoadReportData(reportIndex);
        }

        private void LoadReportData(int reportIndex)
        {
            DataTable reportData = new DataTable();
            string reportTitle = "";

            using (SQLiteConnection connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                string query = "";

                switch (reportIndex)
                {
                    case 0: // أكثر الكتب إعارة
                        reportTitle = "تقرير أكثر الكتب إعارة";
                        query = @"
                        SELECT b.Title, COUNT(br.BorrowID) AS BorrowCount
                        FROM Books b
                        JOIN Borrowings br ON b.BookID = br.BookID
                        GROUP BY b.BookID
                        ORDER BY BorrowCount DESC";
                        break;

                    case 1: // أكثر الأعضاء استعارة
                        reportTitle = "تقرير أكثر الأعضاء استعارة";
                        query = @"
                        SELECT m.Name, COUNT(br.BorrowID) AS BorrowCount
                        FROM Members m
                        JOIN Borrowings br ON m.MemberID = br.MemberID
                        GROUP BY m.MemberID
                        ORDER BY BorrowCount DESC";
                        break;

                    case 2: // الكتب المتأخرة
                        reportTitle = "تقرير الكتب المتأخرة";
                        query = @"
                        SELECT b.Title, m.Name, br.BorrowDate, br.DueDate, 
                               JULIANDAY('now') - JULIANDAY(br.DueDate) AS OverdueDays
                        FROM Borrowings br
                        JOIN Books b ON br.BookID = b.BookID
                        JOIN Members m ON br.MemberID = m.MemberID
                        WHERE br.ReturnDate IS NULL AND br.DueDate < date('now')
                        ORDER BY OverdueDays DESC";
                        break;

                    case 3: // الكتب حسب التصنيف
                        reportTitle = "تقرير الكتب حسب التصنيف";
                        query = @"
                        SELECT Category, COUNT(*) AS BookCount, SUM(TotalCopies) AS TotalCopies
                        FROM Books
                        GROUP BY Category
                        ORDER BY BookCount DESC";
                        break;

                    case 4: // الإعارات اليومية
                        reportTitle = "تقرير الإعارات اليومية";
                        query = @"
                        SELECT BorrowDate, COUNT(*) AS BorrowCount
                        FROM Borrowings
                        GROUP BY BorrowDate
                        ORDER BY BorrowDate DESC";
                        break;
                }

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                    {
                        adapter.Fill(reportData);
                    }
                }
            }

            // عرض عنوان التقرير
            lblReportTitle.Text = reportTitle;

            // عرض بيانات التقرير في الجدول
            dataGridViewReport.DataSource = reportData;

            // تنسيق عرض الجدول
            dataGridViewReport.RightToLeft = RightToLeft.Yes;

            // تنسيق الأعمدة حسب نوع التقرير
            switch (reportIndex)
            {
                case 0: // أكثر الكتب إعارة
                    dataGridViewReport.Columns["Title"].HeaderText = "عنوان الكتاب";
                    dataGridViewReport.Columns["BorrowCount"].HeaderText = "عدد مرات الإعارة";
                    break;

                case 1: // أكثر الأعضاء استعارة
                    dataGridViewReport.Columns["Name"].HeaderText = "اسم العضو";
                    dataGridViewReport.Columns["BorrowCount"].HeaderText = "عدد مرات الاستعارة";
                    break;

                case 2: // الكتب المتأخرة
                    dataGridViewReport.Columns["Title"].HeaderText = "عنوان الكتاب";
                    dataGridViewReport.Columns["Name"].HeaderText = "اسم المستعير";
                    dataGridViewReport.Columns["BorrowDate"].HeaderText = "تاريخ الإعارة";
                    dataGridViewReport.Columns["DueDate"].HeaderText = "تاريخ الاستحقاق";
                    dataGridViewReport.Columns["OverdueDays"].HeaderText = "أيام التأخير";
                    break;

                case 3: // الكتب حسب التصنيف
                    dataGridViewReport.Columns["Category"].HeaderText = "التصنيف";
                    dataGridViewReport.Columns["BookCount"].HeaderText = "عدد الكتب";
                    dataGridViewReport.Columns["TotalCopies"].HeaderText = "إجمالي النسخ";
                    break;

                case 4: // الإعارات اليومية
                    dataGridViewReport.Columns["BorrowDate"].HeaderText = "التاريخ";
                    dataGridViewReport.Columns["BorrowCount"].HeaderText = "عدد الإعارات";
                    break;
            }

            // ضبط أحجام الأعمدة
            foreach (DataGridViewColumn column in dataGridViewReport.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            // عرض عدد السجلات
            lblReportCount.Text = $"عدد السجلات: {reportData.Rows.Count}";
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            MessageBox.Show("سيتم تنفيذ خاصية الطباعة في الإصدار القادم", "معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}