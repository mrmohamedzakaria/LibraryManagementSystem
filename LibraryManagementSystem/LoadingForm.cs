using System;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;

namespace LibraryManagementSystem
{
    public partial class LoadingForm : Form
    {
        private int progressValue = 0;
        private System.Windows.Forms.Timer progressTimer;

        public LoadingForm()
        {
            InitializeComponent();

            // إعداد المؤقت لتحديث شريط التقدم
            progressTimer = new System.Windows.Forms.Timer();
            progressTimer.Interval = 30;
            progressTimer.Tick += ProgressTimer_Tick;
        }

        private void LoadingForm_Load(object sender, EventArgs e)
        {
            // عرض اسم التطبيق والإصدار
            lblAppName.Text = AppConfig.AppName;
            lblVersion.Text = $"الإصدار {AppConfig.AppVersion}";

            // بدء مؤقت شريط التقدم
            progressValue = 0;
            progressBar1.Value = 0;
            progressTimer.Start();

            // عرض رسالة التحميل
            lblStatus.Text = "جاري تهيئة النظام...";
        }

        private void ProgressTimer_Tick(object sender, EventArgs e)
        {
            // تحديث قيمة شريط التقدم
            progressValue += 1;
            progressBar1.Value = Math.Min(progressValue, 100);

            // تحديث رسالة الحالة حسب مرحلة التحميل
            switch (progressValue)
            {
                case 20:
                    lblStatus.Text = "جاري التحقق من قاعدة البيانات...";
                    break;
                case 40:
                    lblStatus.Text = "جاري تحميل البيانات...";
                    break;
                case 60:
                    lblStatus.Text = "جاري تهيئة الواجهة...";
                    break;
                case 80:
                    lblStatus.Text = "جاري إعداد النظام...";
                    break;
                case 100:
                    // إيقاف المؤقت وإغلاق نموذج التحميل
                    progressTimer.Stop();
                    lblStatus.Text = "اكتمل التحميل!";

                    // الانتظار لحظة لإظهار رسالة اكتمال التحميل
                    Thread.Sleep(500);

                    // إغلاق نموذج التحميل
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    break;
            }
        }
    }
}