using System;
using System.Data;
using System.Windows.Forms;

namespace LibraryManagementSystem
{
    public partial class MembersForm : Form
    {
        private int _selectedMemberId = 0;

        public MembersForm()
        {
            InitializeComponent();
        }

        private void MembersForm_Load(object sender, EventArgs e)
        {
            // إعداد حالات العضوية
            cmbStatus.Items.Add("نشط");
            cmbStatus.Items.Add("موقوف");
            cmbStatus.Items.Add("منتهي");
            cmbStatus.SelectedIndex = 0;

            // تحديث جدول الأعضاء عند تحميل النموذج
            RefreshMembersGrid();
            ClearFields();
        }

        private void RefreshMembersGrid()
        {
            // عرض جميع الأعضاء في الجدول
            DataTable membersTable = DatabaseManager.GetAllMembers();
            dataGridViewMembers.DataSource = membersTable;

            // تحديد العناوين العربية للأعمدة
            dataGridViewMembers.Columns["MemberID"]!.HeaderText = "رقم العضو";
            dataGridViewMembers.Columns["Name"].HeaderText = "الاسم";
            dataGridViewMembers.Columns["Phone"].HeaderText = "الهاتف";
            dataGridViewMembers.Columns["Email"].HeaderText = "البريد الإلكتروني";
            dataGridViewMembers.Columns["Address"].HeaderText = "العنوان";
            dataGridViewMembers.Columns["RegistrationDate"].HeaderText = "تاريخ التسجيل";
            dataGridViewMembers.Columns["Status"].HeaderText = "الحالة";

            // تنسيق عرض الجدول
            dataGridViewMembers.RightToLeft = RightToLeft.Yes;
            dataGridViewMembers.Columns["MemberID"].Width = 80;
            dataGridViewMembers.Columns["Name"].Width = 150;
            dataGridViewMembers.Columns["Phone"].Width = 100;
            dataGridViewMembers.Columns["Email"].Width = 150;
            dataGridViewMembers.Columns["Address"].Width = 200;
            dataGridViewMembers.Columns["RegistrationDate"].Width = 100;
            dataGridViewMembers.Columns["Status"].Width = 80;

            // تحديث عدد الأعضاء
            lblMembersCount.Text = $"عدد الأعضاء: {membersTable.Rows.Count}";
        }

        private void ClearFields()
        {
            // مسح الحقول
            txtName.Text = "";
            txtPhone.Text = "";
            txtEmail.Text = "";
            txtAddress.Text = "";
            cmbStatus.SelectedIndex = 0;

            // إعادة تعيين رقم العضو المحدد
            _selectedMemberId = 0;
            btnAdd.Text = "إضافة";
            btnDelete.Enabled = false;
        }

        private bool ValidateInput()
        {
            // التحقق من صحة البيانات المدخلة
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("يجب إدخال اسم العضو", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("يجب إدخال رقم هاتف العضو", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPhone.Focus();
                return false;
            }

            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            // تحويل البيانات المدخلة
            string name = txtName.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string email = txtEmail.Text.Trim();
            string address = txtAddress.Text.Trim();
            string status = cmbStatus.SelectedItem.ToString();

            bool success;

            if (_selectedMemberId == 0)
            {
                // إضافة عضو جديد
                success = DatabaseManager.AddMember(name, phone, email, address, status);
                if (success)
                {
                    MessageBox.Show("تمت إضافة العضو بنجاح", "تأكيد", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                // تحديث عضو موجود
                success = DatabaseManager.UpdateMember(_selectedMemberId, name, phone, email, address, status);
                if (success)
                {
                    MessageBox.Show("تم تحديث بيانات العضو بنجاح", "تأكيد", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            if (success)
            {
                // تحديث الجدول ومسح الحقول
                RefreshMembersGrid();
                ClearFields();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedMemberId == 0)
            {
                MessageBox.Show("يرجى تحديد عضو للحذف", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("هل أنت متأكد من حذف هذا العضو؟", "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                bool success = DatabaseManager.DeleteMember(_selectedMemberId);
                if (success)
                {
                    MessageBox.Show("تم حذف العضو بنجاح", "تأكيد", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshMembersGrid();
                    ClearFields();
                }
            }
        }

        private void dataGridViewMembers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewMembers.Rows[e.RowIndex];

                // عرض بيانات العضو المحدد في الحقول
                _selectedMemberId = Convert.ToInt32(row.Cells["MemberID"].Value);
                txtName.Text = row.Cells["Name"].Value.ToString();
                txtPhone.Text = row.Cells["Phone"].Value.ToString();
                txtEmail.Text = row.Cells["Email"].Value.ToString();
                txtAddress.Text = row.Cells["Address"].Value.ToString();
                cmbStatus.SelectedItem = row.Cells["Status"].Value.ToString();

                // تغيير زر الإضافة إلى تحديث
                btnAdd.Text = "تحديث";
                btnDelete.Enabled = true;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // البحث في الأعضاء أثناء الكتابة
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                RefreshMembersGrid();
            }
            else
            {
                string searchText = txtSearch.Text.Trim();
                DataTable searchResults = DatabaseManager.SearchMembers(searchText);
                dataGridViewMembers.DataSource = searchResults;

                // تحديث عدد الأعضاء
                lblMembersCount.Text = $"عدد الأعضاء: {searchResults.Rows.Count}";
            }
        }
    }
}