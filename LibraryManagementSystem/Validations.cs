using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace LibraryManagementSystem
{
    public static class Validations
    {
        // التحقق من صحة البريد الإلكتروني
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return true; // البريد الإلكتروني اختياري

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

        // التحقق من صحة رقم الهاتف
        public static bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return true; // رقم الهاتف اختياري

            // تنسيق الهاتف المقبول: أرقام فقط (على الأقل 8 أرقام) مع احتمال وجود '+' في البداية
            string pattern = @"^\+?\d{8,15}$";
            return Regex.IsMatch(phone, pattern);
        }

        // التحقق من صحة السنة
        public static bool IsValidYear(string yearStr)
        {
            if (!int.TryParse(yearStr, out int year))
                return false;

            // السنة يجب أن تكون أكبر من 0 وأقل من أو تساوي السنة الحالية
            return year > 0 && year <= DateTime.Now.Year;
        }

        // التحقق من صحة رقم ISBN
        public static bool IsValidISBN(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
                return true; // ISBN اختياري

            // ISBN-10 أو ISBN-13
            string pattern = @"^(?:\d{10}|\d{13})$";
            return Regex.IsMatch(isbn, pattern);
        }

        // التحقق من وجود نص غير فارغ
        public static bool IsNotEmpty(string text, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show($"يجب إدخال {fieldName}", "خطأ التحقق", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        // التحقق من صحة قيمة رقمية
        public static bool IsValidNumber(string numStr, string fieldName, int minValue = 0)
        {
            if (!int.TryParse(numStr, out int value) || value < minValue)
            {
                MessageBox.Show($"يجب إدخال قيمة صحيحة لـ {fieldName} (أكبر من أو تساوي {minValue})", "خطأ التحقق", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        // التحقق من صحة التاريخ
        public static bool IsValidDate(DateTime date, string fieldName, bool allowPastDates = true)
        {
            if (!allowPastDates && date.Date < DateTime.Now.Date)
            {
                MessageBox.Show($"يجب أن يكون {fieldName} تاريخاً مستقبلياً", "خطأ التحقق", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
    }
}