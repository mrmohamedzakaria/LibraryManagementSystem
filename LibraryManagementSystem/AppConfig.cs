// AppConfig.cs - إعدادات التطبيق المركزية
using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using System.Timers;
using Timer = System.Windows.Forms.Timer;

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
        public static Color PrimaryColor = Color.FromArgb(0, 122, 204);
        public static Color SecondaryColor = Color.FromArgb(46, 204, 113);
        public static Color DangerColor = Color.FromArgb(231, 76, 60);
        public static Color WarningColor = Color.FromArgb(230, 126, 34);
        public static Color InfoColor = Color.FromArgb(52, 152, 219);

        // مسار سجلات التطبيق
        private static string LogDirectory = Path.Combine(Application.StartupPath, "Logs");

        // مؤقت النسخ الاحتياطي التلقائي
        private static System.Timers.Timer _autoBackupTimer;

        // إعدادات سجلات النظام
        public static bool EnableLogging = true;
        public static LogLevel CurrentLogLevel = LogLevel.Info;

        // مستويات تفصيل السجلات
        public enum LogLevel
        {
            Debug,
            Info,
            Warning,
            Error,
            Fatal
        }

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

            // إنشاء مجلد السجلات
            if (!Directory.Exists(LogDirectory))
            {
                Directory.CreateDirectory(LogDirectory);
            }
        }

        // احصائيات النظام
        public static string GetSystemInfo()
        {
            return $"نظام إدارة المكتبة - الإصدار {AppVersion}\n" +
                   $"تاريخ التشغيل: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}\n" +
                   $"مسار قاعدة البيانات: {DatabasePath}";
        }

        // تهيئة نظام النسخ الاحتياطي التلقائي
        public static void InitializeAutoBackup()
        {
            if (_autoBackupTimer != null)
            {
                _autoBackupTimer.Stop();
                _autoBackupTimer.Dispose();
            }

            _autoBackupTimer = new System.Timers.Timer();
            _autoBackupTimer.Elapsed += AutoBackup_Elapsed;

            // قراءة إعدادات النسخ الاحتياطي
            bool autoBackup = Properties.Settings.Default.AutoBackup;

            if (autoBackup)
            {
                // حساب الوقت المتبقي للنسخ الاحتياطي التالي
                DateTime lastBackup = Properties.Settings.Default.LastBackupDate;
                int backupDays = Properties.Settings.Default.BackupDays;
                DateTime nextBackup = lastBackup.AddDays(backupDays);

                // تعيين وقت معين للتنفيذ إذا كان محدداً
                DateTime today = DateTime.Today;
                TimeSpan backupTime = Properties.Settings.Default.BackupTime;
                DateTime scheduledBackup = today.Add(backupTime);

                // إذا كان الوقت المجدول قد مر اليوم، نضيفه غداً
                if (scheduledBackup < DateTime.Now)
                {
                    scheduledBackup = scheduledBackup.AddDays(1);
                }

                // استخدام الموعد الأقرب بين المجدول والمحسوب بناءً على آخر نسخة
                DateTime backupDateTime = (nextBackup < scheduledBackup) ? nextBackup : scheduledBackup;

                if (DateTime.Now >= nextBackup)
                {
                    // يجب عمل نسخة احتياطية فوراً
                    PerformAutoBackup();
                }
                else
                {
                    // تعيين المؤقت للنسخ الاحتياطي التالي
                    TimeSpan timeToNextBackup = backupDateTime - DateTime.Now;
                    _autoBackupTimer.Interval = timeToNextBackup.TotalMilliseconds;
                    _autoBackupTimer.Start();

                    LogToFile(LogLevel.Info, $"تم جدولة النسخ الاحتياطي التالي في {backupDateTime}");
                }
            }
        }

        // إعداد النسخ الاحتياطي التلقائي
        public static void SetupAutoBackup(int days, TimeSpan time)
        {
            Properties.Settings.Default.AutoBackup = true;
            Properties.Settings.Default.BackupDays = days;
            Properties.Settings.Default.BackupTime = time;
            Properties.Settings.Default.Save();

            InitializeAutoBackup();

            LogToFile(LogLevel.Info, $"تم تفعيل النسخ الاحتياطي التلقائي كل {days} يوم في الساعة {time}");
        }

        // إيقاف النسخ الاحتياطي التلقائي
        public static void DisableAutoBackup()
        {
            if (_autoBackupTimer != null)
            {
                _autoBackupTimer.Stop();
            }

            Properties.Settings.Default.AutoBackup = false;
            Properties.Settings.Default.Save();

            LogToFile(LogLevel.Info, "تم إيقاف النسخ الاحتياطي التلقائي");
        }

        // حدث مؤقت النسخ الاحتياطي
        private static void AutoBackup_Elapsed(object sender, ElapsedEventArgs e)
        {
            PerformAutoBackup();
        }

        // تنفيذ النسخ الاحتياطي التلقائي
        public static void PerformAutoBackup()
        {
            try
            {
                string backupPath = Properties.Settings.Default.BackupPath;

                // التأكد من وجود المجلد
                if (!Directory.Exists(backupPath))
                {
                    Directory.CreateDirectory(backupPath);
                }

                // إنشاء اسم ملف النسخ الاحتياطي
                string backupFileName = $"AutoBackup_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.lbk";
                string backupFilePath = Path.Combine(backupPath, backupFileName);

                // إنشاء النسخة الاحتياطية
                bool success = DatabaseManager.ExportFullDatabase(backupFilePath);

                if (success)
                {
                    // تحديث تاريخ آخر نسخ احتياطي
                    Properties.Settings.Default.LastBackupDate = DateTime.Now;
                    Properties.Settings.Default.Save();

                    // إعادة تعيين المؤقت للنسخ الاحتياطي التالي
                    int backupDays = Properties.Settings.Default.BackupDays;
                    DateTime nextBackup = DateTime.Now.AddDays(backupDays);

                    // تعيين وقت معين للتنفيذ إذا كان محدداً
                    TimeSpan backupTime = Properties.Settings.Default.BackupTime;
                    DateTime today = DateTime.Today.AddDays(backupDays);
                    DateTime scheduledBackup = today.Add(backupTime);

                    TimeSpan timeToNextBackup = scheduledBackup - DateTime.Now;
                    _autoBackupTimer.Interval = timeToNextBackup.TotalMilliseconds;
                    _autoBackupTimer.Start();

                    LogToFile(LogLevel.Info, $"تم إنشاء نسخة احتياطية تلقائية بنجاح: {backupFilePath}");
                    LogToFile(LogLevel.Info, $"تم جدولة النسخ الاحتياطي التالي في {scheduledBackup}");

                    // اختياري: إعلام المستخدم
                    ShowNotification("تم إنشاء نسخة احتياطية تلقائية بنجاح");
                }
                else
                {
                    LogToFile(LogLevel.Error, "فشل إنشاء النسخة الاحتياطية التلقائية");
                }
            }
            catch (Exception ex)
            {
                // تسجيل الخطأ
                LogToFile(LogLevel.Error, $"حدث خطأ أثناء النسخ الاحتياطي التلقائي: {ex.Message}");
            }
        }

        // عرض إشعار للمستخدم
        private static void ShowNotification(string message)
        {
            // يمكن استخدام NotifyIcon أو MessageBox حسب الحاجة
            if (Application.OpenForms.Count > 0)
            {
                Form mainForm = Application.OpenForms[0];
                mainForm.Invoke(new Action(() =>
                {
                    NotifyIcon notifyIcon = new NotifyIcon();
                    notifyIcon.Icon = SystemIcons.Information;
                    notifyIcon.Visible = true;
                    notifyIcon.BalloonTipTitle = AppName;
                    notifyIcon.BalloonTipText = message;
                    notifyIcon.ShowBalloonTip(3000);

                    // إزالة الأيقونة بعد ظهور الإشعار
                    Timer timer = new Timer();
                    timer.Interval = 5000;
                    timer.Tick += (s, e) =>
                    {
                        notifyIcon.Dispose();
                        timer.Stop();
                        timer.Dispose();
                    };
                    timer.Start();
                }));
            }
        }

        // تسجيل رسالة في ملف السجل
        public static void LogToFile(LogLevel level, string message)
        {
            if (!EnableLogging || level < CurrentLogLevel)
                return;

            try
            {
                if (!Directory.Exists(LogDirectory))
                {
                    Directory.CreateDirectory(LogDirectory);
                }

                string logFile = Path.Combine(LogDirectory, $"Log_{DateTime.Now:yyyy-MM-dd}.txt");
                string logLevelStr = level.ToString().ToUpper();

                using (StreamWriter writer = new StreamWriter(logFile, true))
                {
                    writer.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{logLevelStr}] {message}");
                }
            }
            catch
            {
                // تجاهل أخطاء التسجيل
            }
        }

        // الحصول على مسار أحدث ملف سجل
        public static string GetLatestLogFilePath()
        {
            try
            {
                if (!Directory.Exists(LogDirectory))
                    return string.Empty;

                string logFile = Path.Combine(LogDirectory, $"Log_{DateTime.Now:yyyy-MM-dd}.txt");
                if (File.Exists(logFile))
                    return logFile;

                // البحث عن أحدث ملف
                var logFiles = Directory.GetFiles(LogDirectory, "Log_*.txt");
                if (logFiles.Length > 0)
                {
                    Array.Sort(logFiles);
                    return logFiles[logFiles.Length - 1];
                }

                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        // إزالة ملفات السجل القديمة
        public static void CleanupOldLogFiles(int daysToKeep = 30)
        {
            try
            {
                if (!Directory.Exists(LogDirectory))
                    return;

                var logFiles = Directory.GetFiles(LogDirectory, "Log_*.txt");
                DateTime cutoffDate = DateTime.Now.AddDays(-daysToKeep);

                foreach (var file in logFiles)
                {
                    FileInfo fileInfo = new FileInfo(file);
                    if (fileInfo.CreationTime < cutoffDate)
                    {
                        fileInfo.Delete();
                    }
                }
            }
            catch
            {
                // تجاهل أخطاء التنظيف
            }
        }

        // إزالة ملفات النسخ الاحتياطي القديمة
        public static void CleanupOldBackups(int backupsToKeep = 10)
        {
            try
            {
                string backupPath = Properties.Settings.Default.BackupPath;

                if (!Directory.Exists(backupPath))
                    return;

                var backupFiles = Directory.GetFiles(backupPath, "*.lbk");

                if (backupFiles.Length <= backupsToKeep)
                    return;

                // ترتيب الملفات حسب تاريخ الإنشاء (الأقدم أولاً)
                Array.Sort(backupFiles, (a, b) => File.GetCreationTime(a).CompareTo(File.GetCreationTime(b)));

                // حذف الملفات الأقدم
                int filesToDelete = backupFiles.Length - backupsToKeep;
                for (int i = 0; i < filesToDelete; i++)
                {
                    File.Delete(backupFiles[i]);
                    LogToFile(LogLevel.Info, $"تم حذف نسخة احتياطية قديمة: {backupFiles[i]}");
                }
            }
            catch (Exception ex)
            {
                LogToFile(LogLevel.Error, $"حدث خطأ أثناء تنظيف النسخ الاحتياطية القديمة: {ex.Message}");
            }
        }
    }
}