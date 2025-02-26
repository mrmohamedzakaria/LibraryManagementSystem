// SettingsForm.cs
using System;
using System.Drawing;
using System.Windows.Forms;

namespace LibraryManagementSystem
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            // تحميل الثيمات المتاحة
            cmbTheme.Items.Clear();
            foreach (string themeName in ThemeManager.GetAvailableThemes())
            {
                cmbTheme.Items.Add(themeName);
            }

            // تحديد الثيم الحالي
            cmbTheme.SelectedIndex = (int)ThemeManager.CurrentTheme;

            // تحميل إعدادات الإعارة
            numBorrowDays.Value = AppConfig.DefaultBorrowDays;

            // تحميل إعدادات النظام
            txtAppName.Text = AppConfig.AppName;
            chkRightToLeft.Checked = Properties.Settings.Default.RightToLeft;

            // تحميل إعدادات عرض الجداول
            chkColorCoding.Checked = Properties.Settings.Default.UseColorCoding;

            // تطبيق الثيم على النموذج
            ThemeManager.ApplyTheme(this);

            // تظليل الثيم المحدد في واجهة المستخدم
            ShowThemePreview();
        }

        private void ShowThemePreview()
        {
            // تحديث لوحة معاينة الثيم
            var colors = ThemeManager.GetCurrentThemeColors();

            panelPrimary.BackColor = colors.Primary;
            panelSecondary.BackColor = colors.Secondary;
            panelDanger.BackColor = colors.Danger;
            panelWarning.BackColor = colors.Warning;
            panelInfo.BackColor = colors.Info;

            panelBackground.BackColor = colors.Background;
            lblThemePreview.ForeColor = colors.Text;
            lblThemePreview.BackColor = colors.Background;
        }

        private void cmbTheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            // تغيير الثيم مؤقتاً للمعاينة
            ThemeManager.ChangeThemeByName(cmbTheme.SelectedItem.ToString());

            // تحديث معاينة الثيم
            ShowThemePreview();

            // تطبيق الثيم على النموذج
            ThemeManager.ApplyTheme(this);
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            try
            {
                // حفظ إعدادات الثيم
                ThemeManager.ChangeThemeByName(cmbTheme.SelectedItem.ToString());

                // حفظ إعدادات الإعارة
                AppConfig.DefaultBorrowDays = (int)numBorrowDays.Value;

                // حفظ إعدادات النظام
                AppConfig.AppName = txtAppName.Text;
                Properties.Settings.Default.RightToLeft = chkRightToLeft.Checked;

                // حفظ إعدادات عرض الجداول
                Properties.Settings.Default.UseColorCoding = chkColorCoding.Checked;

                // حفظ الإعدادات في ملف الإعدادات
                Properties.Settings.Default.Save();

                // إظهار رسالة تأكيد
                UIHelper.ShowSuccessMessage("تم حفظ الإعدادات بنجاح");

                // إعلام النموذج الرئيسي بحاجته للتحديث
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                UIHelper.ShowErrorMessage($"حدث خطأ أثناء حفظ الإعدادات: {ex.Message}");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // إلغاء التغييرات وإعادة تحميل الثيم السابق
            int savedTheme = Properties.Settings.Default.Theme;
            if (Enum.IsDefined(typeof(ThemeType), savedTheme))
            {
                ThemeManager.ChangeTheme((ThemeType)savedTheme);
            }

            // إغلاق النموذج بدون حفظ
            DialogResult = DialogResult.Cancel;
        }

        private void btnResetDefaults_Click(object sender, EventArgs e)
        {
            // التحقق من تأكيد المستخدم
            bool confirmed = UIHelper.ShowConfirmationMessage(
                "هل أنت متأكد من إعادة جميع الإعدادات إلى الافتراضية؟",
                "تأكيد إعادة الضبط"
            );

            if (confirmed)
            {
                // إعادة الإعدادات إلى الافتراضية
                cmbTheme.SelectedIndex = 0; // الثيم الفاتح
                numBorrowDays.Value = 14; // 14 يوم كمدة افتراضية للإعارة
                txtAppName.Text = "نظام إدارة المكتبة";
                chkRightToLeft.Checked = true;
                chkColorCoding.Checked = true;

                // تطبيق التغييرات
                cmbTheme_SelectedIndexChanged(sender, e);
            }
        }

        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            try
            {
                // اختبار الاتصال بقاعدة البيانات
                using (var connection = DatabaseManager.GetConnection())
                {
                    connection.Open();
                    UIHelper.ShowSuccessMessage("تم الاتصال بقاعدة البيانات بنجاح");
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowErrorMessage($"فشل الاتصال بقاعدة البيانات: {ex.Message}");
            }
        }
    }
}