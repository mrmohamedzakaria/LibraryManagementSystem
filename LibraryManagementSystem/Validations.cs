// Validations.cs - فئة موسعة للتحقق من المدخلات
using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Drawing;

namespace LibraryManagementSystem
{
    public static class Validations
    {
        // التحقق من صحة البريد الإلكتروني
        public static bool IsValidEmail(string email, TextBox textBox = null)
        {
            if (string.IsNullOrWhiteSpace(email))
                return true; // البريد الإلكتروني اختياري

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            bool isValid = Regex.IsMatch(email, pattern);

            // تغيير لون الخلفية حسب صحة المدخلات
            if (textBox != null)
            {
                textBox.BackColor = isValid ? SystemColors.Window : Color.MistyRose;
            }

            return isValid;
        }

        // التحقق من صحة رقم الهاتف
        public static bool IsValidPhone(string phone, TextBox textBox = null)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return true; // رقم الهاتف اختياري

            // تنسيق الهاتف المقبول: أرقام فقط (على الأقل 8 أرقام) مع احتمال وجود '+' في البداية
            string pattern = @"^\+?\d{8,15}$";
            bool isValid = Regex.IsMatch(phone, pattern);

            // تغيير لون الخلفية حسب صحة المدخلات
            if (textBox != null)
            {
                textBox.BackColor = isValid ? SystemColors.Window : Color.MistyRose;
            }

            return isValid;
        }

        // التحقق من صحة السنة
        public static bool IsValidYear(string yearStr, TextBox textBox = null)
        {
            if (!int.TryParse(yearStr, out int year))
            {
                if (textBox != null)
                {
                    textBox.BackColor = Color.MistyRose;
                }
                return false;
            }

            // السنة يجب أن تكون أكبر من 0 وأقل من أو تساوي السنة الحالية
            bool isValid = year > 0 && year <= DateTime.Now.Year;

            // تغيير لون الخلفية حسب صحة المدخلات
            if (textBox != null)
            {
                textBox.BackColor = isValid ? SystemColors.Window : Color.MistyRose;
            }

            return isValid;
        }

        // التحقق من صحة رقم ISBN
        public static bool IsValidISBN(string isbn, TextBox textBox = null)
        {
            if (string.IsNullOrWhiteSpace(isbn))
                return true; // ISBN اختياري

            // ISBN-10 أو ISBN-13 أو نمط مخصص مثل ARB-001
            string pattern = @"^(?:\d{10}|\d{13}|[A-Z]{3}-\d{3})$";
            bool isValid = Regex.IsMatch(isbn, pattern);

            // تغيير لون الخلفية حسب صحة المدخلات
            if (textBox != null)
            {
                textBox.BackColor = isValid ? SystemColors.Window : Color.MistyRose;
            }

            return isValid;
        }

        // التحقق من وجود نص غير فارغ
        public static bool IsNotEmpty(string text, string fieldName, TextBox? textBox = null)
        {
            bool isValid = !string.IsNullOrWhiteSpace(text);

            // تغيير لون الخلفية حسب صحة المدخلات
            if (textBox != null)
            {
                textBox.BackColor = isValid ? SystemColors.Window : Color.MistyRose;
            }

            if (!isValid && fieldName != null)
            {
                MessageBox.Show($"يجب إدخال {fieldName}", "خطأ التحقق", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return isValid;
        }

        // التحقق من صحة قيمة رقمية
        public static bool IsValidNumber(string numStr, string fieldName, int minValue = 0, TextBox textBox = null)
        {
            bool isValid = int.TryParse(numStr, out int value) && value >= minValue;

            // تغيير لون الخلفية حسب صحة المدخلات
            if (textBox != null)
            {
                textBox.BackColor = isValid ? SystemColors.Window : Color.MistyRose;
            }

            if (!isValid && fieldName != null)
            {
                MessageBox.Show($"يجب إدخال قيمة صحيحة لـ {fieldName} (أكبر من أو تساوي {minValue})",
                               "خطأ التحقق", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return isValid;
        }

        // التحقق من صحة التاريخ
        public static bool IsValidDate(DateTime date, string fieldName, bool allowPastDates = true, DateTimePicker picker = null)
        {
            bool isValid = true;

            if (!allowPastDates && date.Date < DateTime.Now.Date)
            {
                isValid = false;

                if (fieldName != null)
                {
                    MessageBox.Show($"يجب أن يكون {fieldName} تاريخاً مستقبلياً",
                                   "خطأ التحقق", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // تغيير لون الخلفية حسب صحة المدخلات (إذا كان ذلك ممكناً مع DateTimePicker)
            if (picker != null && !isValid)
            {
                picker.CalendarTitleBackColor = Color.MistyRose;
            }

            return isValid;
        }

        // التحقق من الحقول في وقت واحد
        public static bool ValidateForm(params (bool isValid, string errorMessage)[] validations)
        {
            foreach (var validation in validations)
            {
                if (!validation.isValid && !string.IsNullOrEmpty(validation.errorMessage))
                {
                    MessageBox.Show(validation.errorMessage, "خطأ التحقق", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }

        // إضافة مراقبي أحداث للتحقق الفوري من المدخلات
        public static void AttachEmailValidator(TextBox textBox)
        {
            textBox.TextChanged += (sender, e) => IsValidEmail(textBox.Text, textBox);
            textBox.Leave += (sender, e) => IsValidEmail(textBox.Text, textBox);
        }

        public static void AttachPhoneValidator(TextBox textBox)
        {
            textBox.TextChanged += (sender, e) => IsValidPhone(textBox.Text, textBox);
            textBox.Leave += (sender, e) => IsValidPhone(textBox.Text, textBox);
        }

        public static void AttachYearValidator(TextBox textBox)
        {
            textBox.TextChanged += (sender, e) => IsValidYear(textBox.Text, textBox);
            textBox.Leave += (sender, e) => IsValidYear(textBox.Text, textBox);
        }

        public static void AttachRequiredValidator(TextBox textBox, string fieldName = null)
        {
            textBox.TextChanged += (sender, e) =>
            {
                if (!string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.BackColor = SystemColors.Window;
                }
            };

            textBox.Leave += (sender, e) =>
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.BackColor = Color.MistyRose;

                    // إظهار رسالة الخطأ عند مغادرة الحقل فقط إذا كان مطلوباً والمستخدم لم يدخل شيئاً
                    if (fieldName != null)
                    {
                        // تعليق إظهار الرسالة هنا لتجنب الإزعاج - سيتم التحقق عند النقر على زر الحفظ
                        // MessageBox.Show($"يجب إدخال {fieldName}", "خطأ التحقق", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            };
        }

        public static void AttachNumberValidator(TextBox textBox, int minValue = 0)
        {
            textBox.TextChanged += (sender, e) =>
            {
                if (int.TryParse(textBox.Text, out int value) && value >= minValue)
                {
                    textBox.BackColor = SystemColors.Window;
                }
                else
                {
                    textBox.BackColor = Color.MistyRose;
                }
            };
        }

        public static void AttachISBNValidator(TextBox textBox)
        {
            textBox.TextChanged += (sender, e) => IsValidISBN(textBox.Text, textBox);
            textBox.Leave += (sender, e) => IsValidISBN(textBox.Text, textBox);
        }

        // التحقق من النصوص مع التلميح
        public static void AttachTextValidator(TextBox textBox, Func<string, bool> validationFunc, string watermarkText)
        {
            // إضافة تلميح للمستخدم
            TextBoxWatermark.SetWatermark(textBox, watermarkText);

            // إضافة التحقق
            textBox.TextChanged += (sender, e) =>
            {
                bool isValid = validationFunc(textBox.Text);
                textBox.BackColor = isValid ? SystemColors.Window : Color.MistyRose;
            };
        }
    }

    // فئة مساعدة لإضافة تلميح في حقول النص
    public static class TextBoxWatermark
    {
        private const int EM_SETCUEBANNER = 0x1501;

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string lParam);

        public static void SetWatermark(TextBox textBox, string watermarkText)
        {
            SendMessage(textBox.Handle, EM_SETCUEBANNER, 0, watermarkText);
        }
    }
}