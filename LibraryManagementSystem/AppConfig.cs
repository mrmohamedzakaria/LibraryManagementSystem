using System;
using System.IO;
using System.Windows.Forms;

namespace LibraryManagementSystem
{
    public static class AppConfig
    {
        // إعدادات التطبيق
        public static string AppName = "نظام إدارة المكتبة";
        public static string AppVersion = "1.0.0";
        public static string DatabaseName = "LibraryDB.sqlite";
        public static string DatabasePath = Path.Combine(Application.StartupPath, DatabaseName);

        // إعدادات الإعارة
        public static int DefaultBorrowDays = 14; // المدة الافتراضية للإعارة بالأيام

        // عناوين رسائل التطبيق
        public static string MessageTitle = "نظام إدارة المكتبة";

        // الألوان المستخدمة في التطبيق
        public static System.Drawing.Color PrimaryColor = System.Drawing.Color.FromArgb(0, 122, 204);
        public static System.Drawing.Color SecondaryColor = System.Drawing.Color.FromArgb(46, 204, 113);
        public static System.Drawing.Color DangerColor = System.Drawing.Color.FromArgb(231, 76, 60);
        public static System.Drawing.Color WarningColor = System.Drawing.Color.FromArgb(230, 126, 34);
        public static System.Drawing.Color InfoColor = System.Drawing.Color.FromArgb(52, 152, 219);

        // التحقق من وجود ملف قاعدة البيانات
        public static bool CheckDatabaseExists()
        {
            return File.Exists(DatabasePath);
        }

        // إنشاء مجلد البيانات إذا لم يكن موجوداً
        public static void EnsureDataDirectoryExists()
        {
            string? dataDir = Path.GetDirectoryName(DatabasePath);
            if (!Directory.Exists(dataDir))
            {
                Directory.CreateDirectory(dataDir);
            }
        }

        // احصائيات النظام
        public static string GetSystemInfo()
        {
            return $"نظام إدارة المكتبة - الإصدار {AppVersion}\n" +
                   $"تاريخ التشغيل: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}\n" +
                   $"مسار قاعدة البيانات: {DatabasePath}";
        }
    }
}