using ArabicLibraryManagementSystem;

namespace LibraryManagementSystem
{
    internal static class Program
    {
        /// <summary>
        /// ���� ����� ������� ��������
        /// </summary>
        [STAThread]
        private static void Main()
        {
            try
            {
                // ����� ������� �������
                ApplicationConfiguration.Initialize();

                // ����� ������ ������ ��������
                AppConfig.EnsureDataDirectoryExists();

                // ����� ��� ����� �������
                AppConfig.LogToFile(AppConfig.LogLevel.Info, "��� ����� �������: " + AppConfig.GetSystemInfo());

                // ����� ����� �������
                ThemeManager.LoadSavedTheme();

                // ��� ����� �������
                using (var loadingForm = new LoadingForm())
                {
                    var result = loadingForm.ShowDialog();

                    if (result != DialogResult.OK)
                    {
                        // ��� ��� �������
                        AppConfig.LogToFile(AppConfig.LogLevel.Error, "��� �� ����� ������� �� ���� ����� �������");
                        MessageBox.Show(@"��� ��� ����� ��� �������. ���� �������� ��� ����.", @"���",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // ����� ����� ��������
                DatabaseManager.InitializeDatabase();

                // ����� ���� ����� ��������� ��������
                AppConfig.InitializeAutoBackup();

                // ����� ����� ����� ������ �������
                AppConfig.CleanupOldLogFiles();
                AppConfig.CleanupOldBackups();

                // ����� ������� �������
                Application.Run(new MainForm());

                // ����� ����� �������
                AppConfig.LogToFile(AppConfig.LogLevel.Info, "�� ����� ������� �����");
            }
            catch (Exception ex)
            {
                // ������ ������� ��� ��������
                try
                {
                    AppConfig.LogToFile(AppConfig.LogLevel.Fatal, $"��� ��� �����: {ex.Message}\n{ex.StackTrace}");
                }
                catch
                {
                    // ����� ����� �������
                }

                MessageBox.Show($@"��� ��� ��� ����� ����� ����� �������:{ex.Message} ���� ����� ������� ����.",
                    @"���", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}