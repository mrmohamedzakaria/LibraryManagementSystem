using System;
using System.Windows.Forms;

namespace LibraryManagementSystem
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // تهيئة قاعدة البيانات عند بدء التطبيق
            DatabaseManager.InitializeDatabase();

            // عرض الإحصائيات في لوحة التحكم
            UpdateDashboardStats();
        }

        private void UpdateDashboardStats()
        {
            var stats = DatabaseManager.GetStatistics();

            lblTotalBooks.Text = stats["إجمالي الكتب"].ToString();
            lblAvailableBooks.Text = stats["الكتب المتاحة"].ToString();
            lblBorrowedBooks.Text = stats["الكتب المعارة"].ToString();
            lblTotalMembers.Text = stats["إجمالي الأعضاء"].ToString();
            lblActiveMembers.Text = stats["الأعضاء النشطين"].ToString();
            lblTodayBorrowings.Text = stats["إعارات اليوم"].ToString();
        }

        private void btnManageBooks_Click(object sender, EventArgs e)
        {
            BooksForm booksForm = new BooksForm();
            booksForm.ShowDialog();

            // تحديث الإحصائيات بعد إغلاق نموذج إدارة الكتب
            UpdateDashboardStats();
        }

        private void btnManageMembers_Click(object sender, EventArgs e)
        {
            MembersForm membersForm = new MembersForm();
            membersForm.ShowDialog();

            // تحديث الإحصائيات بعد إغلاق نموذج إدارة الأعضاء
            UpdateDashboardStats();
        }

        private void btnManageBorrowings_Click(object sender, EventArgs e)
        {
            BorrowingsForm borrowingsForm = new BorrowingsForm();
            borrowingsForm.ShowDialog();

            // تحديث الإحصائيات بعد إغلاق نموذج إدارة الإعارات
            UpdateDashboardStats();
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            ReportsForm reportsForm = new ReportsForm();
            reportsForm.ShowDialog();
        }

        private void btnSearchBooks_Click(object sender, EventArgs e)
        {
            SearchBooksForm searchForm = new SearchBooksForm();
            searchForm.ShowDialog();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}