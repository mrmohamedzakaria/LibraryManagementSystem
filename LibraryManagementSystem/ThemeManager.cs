// ThemeManager.cs - إدارة ثيمات التطبيق
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace LibraryManagementSystem
{
    public enum ThemeType
    {
        Light,
        Dark,
        Blue,
        Green,
        Purple
    }

    public static class ThemeManager
    {
        // ألوان الثيمات
        private static readonly Dictionary<ThemeType, (Color Primary, Color Secondary, Color Danger, Color Warning, Color Info, Color Text, Color Background)> ThemeColors =
            new Dictionary<ThemeType, (Color Primary, Color Secondary, Color Danger, Color Warning, Color Info, Color Text, Color Background)>
            {
                // ثيم فاتح
                { ThemeType.Light, (
                    Color.FromArgb(0, 122, 204),    // Primary
                    Color.FromArgb(46, 204, 113),   // Secondary
                    Color.FromArgb(231, 76, 60),    // Danger
                    Color.FromArgb(230, 126, 34),   // Warning
                    Color.FromArgb(52, 152, 219),   // Info
                    Color.FromArgb(51, 51, 51),     // Text
                    Color.FromArgb(240, 240, 240)   // Background
                )},
                
                // ثيم داكن
                { ThemeType.Dark, (
                    Color.FromArgb(22, 160, 255),   // Primary
                    Color.FromArgb(46, 204, 113),   // Secondary
                    Color.FromArgb(231, 76, 60),    // Danger
                    Color.FromArgb(230, 126, 34),   // Warning
                    Color.FromArgb(52, 152, 219),   // Info
                    Color.FromArgb(240, 240, 240),  // Text
                    Color.FromArgb(50, 50, 50)      // Background
                )},
                
                // ثيم أزرق
                { ThemeType.Blue, (
                    Color.FromArgb(24, 100, 171),   // Primary
                    Color.FromArgb(2, 119, 189),    // Secondary
                    Color.FromArgb(211, 47, 47),    // Danger
                    Color.FromArgb(245, 124, 0),    // Warning
                    Color.FromArgb(21, 101, 192),   // Info
                    Color.FromArgb(33, 33, 33),     // Text
                    Color.FromArgb(227, 242, 253)   // Background
                )},
                
                // ثيم أخضر
                { ThemeType.Green, (
                    Color.FromArgb(27, 94, 32),     // Primary
                    Color.FromArgb(76, 175, 80),    // Secondary
                    Color.FromArgb(198, 40, 40),    // Danger
                    Color.FromArgb(239, 108, 0),    // Warning
                    Color.FromArgb(2, 119, 189),    // Info
                    Color.FromArgb(33, 33, 33),     // Text
                    Color.FromArgb(232, 245, 233)   // Background
                )},
                
                // ثيم بنفسجي
                { ThemeType.Purple, (
                    Color.FromArgb(123, 31, 162),   // Primary
                    Color.FromArgb(156, 39, 176),   // Secondary
                    Color.FromArgb(211, 47, 47),    // Danger
                    Color.FromArgb(255, 143, 0),    // Warning
                    Color.FromArgb(26, 35, 126),    // Info
                    Color.FromArgb(33, 33, 33),     // Text
                    Color.FromArgb(237, 231, 246)   // Background
                )}
            };

        private static ThemeType _currentTheme = ThemeType.Light;

        // الحصول على الثيم الحالي
        public static ThemeType CurrentTheme => _currentTheme;

        // تغيير الثيم
        public static void ChangeTheme(ThemeType theme)
        {
            _currentTheme = theme;

            // تحديث إعدادات الألوان في AppConfig
            var colors = ThemeColors[theme];
            AppConfig.PrimaryColor = colors.Primary;
            AppConfig.SecondaryColor = colors.Secondary;
            AppConfig.DangerColor = colors.Danger;
            AppConfig.WarningColor = colors.Warning;
            AppConfig.InfoColor = colors.Info;

            // حفظ الثيم في الإعدادات
            Properties.Settings.Default.Theme = (int)theme;
            Properties.Settings.Default.Save();
        }

        // تطبيق الثيم على نموذج
        public static void ApplyTheme(Form form)
        {
            var colors = ThemeColors[_currentTheme];

            form.BackColor = colors.Background;
            form.ForeColor = colors.Text;

            // تطبيق الثيم على جميع عناصر التحكم
            ApplyThemeToControls(form.Controls, colors);
        }

        // تطبيق الثيم على عناصر التحكم
        private static void ApplyThemeToControls(Control.ControlCollection controls, (Color Primary, Color Secondary, Color Danger, Color Warning, Color Info, Color Text, Color Background) colors)
        {
            foreach (Control control in controls)
            {
                // تطبيق ألوان العناصر حسب نوعها
                if (control is Panel panel)
                {
                    if (panel.Name.Contains("panel1") || panel.Name.Contains("headerPanel"))
                    {
                        panel.BackColor = colors.Primary;
                        panel.ForeColor = Color.White;
                    }
                    else
                    {
                        panel.BackColor = colors.Background;
                        panel.ForeColor = colors.Text;
                    }
                }
                else if (control is Button button)
                {
                    // تحديد لون الزر حسب اسمه أو نصه
                    if (button.Name.Contains("Delete") || button.Text.Contains("حذف"))
                    {
                        button.BackColor = colors.Danger;
                    }
                    else if (button.Name.Contains("Add") || button.Text.Contains("إضافة"))
                    {
                        button.BackColor = colors.Secondary;
                    }
                    else if (button.Name.Contains("Clear") || button.Text.Contains("مسح"))
                    {
                        button.BackColor = colors.Warning;
                    }
                    else if (button.Name.Contains("Print") || button.Text.Contains("طباعة"))
                    {
                        button.BackColor = colors.Info;
                    }
                    else if (button.Name.Contains("Exit") || button.Text.Contains("خروج"))
                    {
                        button.BackColor = colors.Danger;
                    }
                    else
                    {
                        button.BackColor = colors.Primary;
                    }

                    button.ForeColor = Color.White;
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderSize = 0;
                }
                else if (control is DataGridView dataGridView)
                {
                    dataGridView.BackgroundColor = colors.Background;
                    dataGridView.ForeColor = colors.Text;
                    dataGridView.GridColor = Color.LightGray;

                    dataGridView.EnableHeadersVisualStyles = false;
                    dataGridView.ColumnHeadersDefaultCellStyle.BackColor = colors.Primary;
                    dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

                    // تلوين الصفوف بالتناوب
                    dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(
                        (int)(colors.Background.R * 0.95),
                        (int)(colors.Background.G * 0.95),
                        (int)(colors.Background.B * 0.95)
                    );
                }
                else if (control is Label label)
                {
                    // الحفاظ على لون التسميات في اللوحات العلوية
                    if (control.Parent is Panel panel && (panel.Name.Contains("panel1") || panel.Name.Contains("headerPanel")))
                    {
                        label.ForeColor = Color.White;
                    }
                    else
                    {
                        label.ForeColor = colors.Text;
                    }
                }
                else if (control is TextBox textBox)
                {
                    textBox.BackColor = SystemColors.Window;
                    textBox.ForeColor = colors.Text;
                }
                else if (control is ComboBox comboBox)
                {
                    comboBox.BackColor = SystemColors.Window;
                    comboBox.ForeColor = colors.Text;
                }
                else if (control is GroupBox groupBox)
                {
                    groupBox.ForeColor = colors.Primary;
                }
                else if (control is CheckBox checkBox)
                {
                    checkBox.ForeColor = colors.Text;
                }
                else if (control is RadioButton radioButton)
                {
                    radioButton.ForeColor = colors.Text;
                }
                else if (control is DateTimePicker dateTimePicker)
                {
                    dateTimePicker.CalendarForeColor = colors.Text;
                    dateTimePicker.CalendarMonthBackground = SystemColors.Window;
                }
                else if (control is NumericUpDown numericUpDown)
                {
                    numericUpDown.BackColor = SystemColors.Window;
                    numericUpDown.ForeColor = colors.Text;
                }
                else if (control is TabControl tabControl)
                {
                    // تنسيق التبويبات
                    tabControl.BackColor = colors.Background;
                    tabControl.ForeColor = colors.Text;
                }
                else if (control is TabPage tabPage)
                {
                    tabPage.BackColor = colors.Background;
                    tabPage.ForeColor = colors.Text;
                }
                else if (control is MenuStrip menuStrip)
                {
                    // تنسيق القوائم
                    menuStrip.BackColor = colors.Primary;
                    menuStrip.ForeColor = Color.White;
                }
                else if (control is ToolStrip toolStrip)
                {
                    // تنسيق شريط الأدوات
                    toolStrip.BackColor = colors.Primary;
                    toolStrip.ForeColor = Color.White;
                }

                // تطبيق الثيم على العناصر الفرعية
                if (control.Controls.Count > 0)
                {
                    ApplyThemeToControls(control.Controls, colors);
                }
            }
        }

        // الحصول على الألوان الحالية للثيم
        public static (Color Primary, Color Secondary, Color Danger, Color Warning, Color Info, Color Text, Color Background) GetCurrentThemeColors()
        {
            return ThemeColors[_currentTheme];
        }

        // استدعاء مرة واحدة في بداية التطبيق لتحميل الثيم المحفوظ
        public static void LoadSavedTheme()
        {
            int savedTheme = Properties.Settings.Default.Theme;
            if (Enum.IsDefined(typeof(ThemeType), savedTheme))
            {
                ChangeTheme((ThemeType)savedTheme);
            }
            else
            {
                ChangeTheme(ThemeType.Light);
            }
        }

        // الحصول على قائمة بالثيمات المتاحة
        public static List<string> GetAvailableThemes()
        {
            List<string> themes = new List<string>();
            foreach (ThemeType theme in Enum.GetValues(typeof(ThemeType)))
            {
                // تحويل اسم الثيم إلى اسم عربي مناسب
                switch (theme)
                {
                    case ThemeType.Light:
                        themes.Add("فاتح");
                        break;
                    case ThemeType.Dark:
                        themes.Add("داكن");
                        break;
                    case ThemeType.Blue:
                        themes.Add("أزرق");
                        break;
                    case ThemeType.Green:
                        themes.Add("أخضر");
                        break;
                    case ThemeType.Purple:
                        themes.Add("بنفسجي");
                        break;
                }
            }

            return themes;
        }

        // تغيير الثيم حسب الاسم العربي
        public static void ChangeThemeByName(string themeName)
        {
            switch (themeName)
            {
                case "فاتح":
                    ChangeTheme(ThemeType.Light);
                    break;
                case "داكن":
                    ChangeTheme(ThemeType.Dark);
                    break;
                case "أزرق":
                    ChangeTheme(ThemeType.Blue);
                    break;
                case "أخضر":
                    ChangeTheme(ThemeType.Green);
                    break;
                case "بنفسجي":
                    ChangeTheme(ThemeType.Purple);
                    break;
            }
        }
    }
}