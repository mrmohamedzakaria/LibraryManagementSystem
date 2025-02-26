using ArabicLibraryManagementSystem;

namespace LibraryManagementSystem
{
    internal static class Program
    {
        /// <summary>
        /// ‰ﬁÿ… »œ«Ì… «· ÿ»Ìﬁ «·—∆Ì”Ì…
        /// </summary>
        [STAThread]
        private static void Main()
        {
            try
            {
                //  ÂÌ∆… ≈⁄œ«œ«  «· ÿ»Ìﬁ
                ApplicationConfiguration.Initialize();

                // ≈‰‘«¡ „Ã·œ«  «·‰Ÿ«„ «·÷—Ê—Ì…
                AppConfig.EnsureDataDirectoryExists();

                //  ”ÃÌ· »œ¡  ‘€Ì· «· ÿ»Ìﬁ
                AppConfig.LogToFile(AppConfig.LogLevel.Info, "»œ¡  ‘€Ì· «· ÿ»Ìﬁ: " + AppConfig.GetSystemInfo());

                //  Õ„Ì· «·ÀÌ„ «·„Õ›ÊŸ
                ThemeManager.LoadSavedTheme();

                // ⁄—÷ ‰„Ê–Ã «· Õ„Ì·
                using (var loadingForm = new LoadingForm())
                {
                    var result = loadingForm.ShowDialog();

                    if (result != DialogResult.OK)
                    {
                        // ≈–« ›‘· «· Õ„Ì·
                        AppConfig.LogToFile(AppConfig.LogLevel.Error, "›‘· ›Ì  ÂÌ∆… «· ÿ»Ìﬁ „‰ Œ·«· ‰„Ê–Ã «· Õ„Ì·");
                        MessageBox.Show(@"ÕœÀ Œÿ√ √À‰«¡ »œ¡ «· ÿ»Ìﬁ. Ì—ÃÏ «·„Õ«Ê·… „—… √Œ—Ï.", @"Œÿ√",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                //  ÂÌ∆… ﬁ«⁄œ… «·»Ì«‰« 
                DatabaseManager.InitializeDatabase();

                //  ÂÌ∆… ‰Ÿ«„ «·‰”Œ «·«Õ Ì«ÿÌ «· ·ﬁ«∆Ì
                AppConfig.InitializeAutoBackup();

                //  ‰ŸÌ› „·›«  «·”Ã· Ê«·‰”Œ «·ﬁœÌ„…
                AppConfig.CleanupOldLogFiles();
                AppConfig.CleanupOldBackups();

                //  ‘€Ì· «·‰„Ê–Ã «·—∆Ì”Ì
                Application.Run(new MainForm());

                //  ”ÃÌ· ≈€·«ﬁ «· ÿ»Ìﬁ
                AppConfig.LogToFile(AppConfig.LogLevel.Info, " „ ≈€·«ﬁ «· ÿ»Ìﬁ »‰Ã«Õ");
            }
            catch (Exception ex)
            {
                // „⁄«·Ã… «·√Œÿ«¡ €Ì— «·„ Êﬁ⁄…
                try
                {
                    AppConfig.LogToFile(AppConfig.LogLevel.Fatal, $"Œÿ√ €Ì— „ Êﬁ⁄: {ex.Message}\n{ex.StackTrace}");
                }
                catch
                {
                    //  Ã«Â· √Œÿ«¡ «· ”ÃÌ·
                }

                MessageBox.Show($@"ÕœÀ Œÿ√ €Ì— „ Êﬁ⁄ √À‰«¡  ‘€Ì· «· ÿ»Ìﬁ:{ex.Message} ”Ì „ ≈€·«ﬁ «· ÿ»Ìﬁ «·¬‰.",
                    @"Œÿ√", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}