// ExportImportManager.cs
using System;
using System.IO;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Collections.Generic;

namespace LibraryManagementSystem
{
    public static class ExportImportManager
    {
        // تصدير جدول بيانات إلى ملف CSV
        public static bool ExportToCSV(DataTable dataTable, string filePath)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                // كتابة أسماء الأعمدة
                List<string> columnNames = new List<string>();
                foreach (DataColumn column in dataTable.Columns)
                {
                    columnNames.Add(column.ColumnName);
                }
                sb.AppendLine(string.Join(",", columnNames));

                // كتابة الصفوف
                foreach (DataRow row in dataTable.Rows)
                {
                    List<string> fields = new List<string>();
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        string field = row[column].ToString();
                        // معالجة الحقول التي تحتوي على فواصل
                        if (field.Contains(","))
                        {
                            field = $"\"{field}\"";
                        }
                        fields.Add(field);
                    }
                    sb.AppendLine(string.Join(",", fields));
                }

                // كتابة الملف
                File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ أثناء تصدير البيانات: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // تصدير جدول بيانات إلى ملف Excel (CSV بتنسيق خاص)
        public static bool ExportToExcel(DataTable dataTable, string filePath)
        {
            try
            {
                // استخدام ترميز Unicode للدعم الأفضل للغة العربية
                using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.Unicode))
                {
                    // كتابة أسماء الأعمدة
                    List<string> columnNames = new List<string>();
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        columnNames.Add(column.ColumnName);
                    }
                    writer.WriteLine(string.Join("\t", columnNames));

                    // كتابة الصفوف
                    foreach (DataRow row in dataTable.Rows)
                    {
                        List<string> fields = new List<string>();
                        foreach (DataColumn column in dataTable.Columns)
                        {
                            string field = row[column].ToString();
                            // معالجة الحقول التي تحتوي على علامات تبويب
                            if (field.Contains("\t"))
                            {
                                field = field.Replace("\t", " ");
                            }
                            fields.Add(field);
                        }
                        writer.WriteLine(string.Join("\t", fields));
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ أثناء تصدير البيانات إلى Excel: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // استيراد من ملف CSV إلى جدول بيانات
        public static DataTable ImportFromCSV(string filePath)
        {
            try
            {
                DataTable dataTable = new DataTable();

                // قراءة الملف
                string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);

                if (lines.Length == 0)
                {
                    throw new Exception("الملف فارغ");
                }

                // قراءة أسماء الأعمدة
                string[] columnNames = ParseCSVLine(lines[0]);
                foreach (string columnName in columnNames)
                {
                    dataTable.Columns.Add(columnName.Trim());
                }

                // قراءة البيانات
                for (int i = 1; i < lines.Length; i++)
                {
                    if (!string.IsNullOrWhiteSpace(lines[i]))
                    {
                        string[] fields = ParseCSVLine(lines[i]);
                        DataRow dataRow = dataTable.NewRow();

                        for (int j = 0; j < Math.Min(fields.Length, columnNames.Length); j++)
                        {
                            dataRow[j] = fields[j].Trim();
                        }

                        dataTable.Rows.Add(dataRow);
                    }
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ أثناء استيراد البيانات: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        // استيراد من ملف Excel (بامتداد .xlsx) إلى جدول بيانات
        public static DataTable ImportFromExcel(string filePath)
        {
            try
            {
                DataTable dataTable = new DataTable();

                // قراءة الملف كملف بترميز Unicode وفواصل تبويب
                string[] lines = File.ReadAllLines(filePath, Encoding.Unicode);

                if (lines.Length == 0)
                {
                    throw new Exception("الملف فارغ");
                }

                // قراءة أسماء الأعمدة
                string[] columnNames = lines[0].Split('\t');
                foreach (string columnName in columnNames)
                {
                    dataTable.Columns.Add(columnName.Trim());
                }

                // قراءة البيانات
                for (int i = 1; i < lines.Length; i++)
                {
                    if (!string.IsNullOrWhiteSpace(lines[i]))
                    {
                        string[] fields = lines[i].Split('\t');
                        DataRow dataRow = dataTable.NewRow();

                        for (int j = 0; j < Math.Min(fields.Length, columnNames.Length); j++)
                        {
                            dataRow[j] = fields[j].Trim();
                        }

                        dataTable.Rows.Add(dataRow);
                    }
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ أثناء استيراد البيانات من Excel: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        // تحليل سطر CSV مع مراعاة الاقتباسات
        private static string[] ParseCSVLine(string line)
        {
            List<string> result = new List<string>();
            int startIndex = 0;
            bool isInsideQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '"')
                {
                    isInsideQuotes = !isInsideQuotes;
                }
                else if (line[i] == ',' && !isInsideQuotes)
                {
                    result.Add(line.Substring(startIndex, i - startIndex).Replace("\"", ""));
                    startIndex = i + 1;
                }
            }

            // إضافة الحقل الأخير
            result.Add(line.Substring(startIndex).Replace("\"", ""));

            return result.ToArray();
        }

        // تصدير جدول بيانات إلى ملف النظام
        public static bool ExportToSystemBackup(Dictionary<string, DataTable> dataTables, string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    // كتابة بيانات الإصدار
                    writer.WriteLine("## نظام إدارة المكتبة - ملف نسخ احتياطي ##");
                    writer.WriteLine($"## الإصدار: {AppConfig.AppVersion} ##");
                    writer.WriteLine($"## تاريخ النسخ: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} ##");
                    writer.WriteLine();

                    // كتابة البيانات لكل جدول
                    foreach (var kvp in dataTables)
                    {
                        string tableName = kvp.Key;
                        DataTable dataTable = kvp.Value;

                        writer.WriteLine($"--- BEGIN TABLE {tableName} ---");

                        // كتابة أسماء الأعمدة
                        List<string> columnNames = new List<string>();
                        foreach (DataColumn column in dataTable.Columns)
                        {
                            columnNames.Add(column.ColumnName);
                        }
                        writer.WriteLine(string.Join("|", columnNames));

                        // كتابة الصفوف
                        foreach (DataRow row in dataTable.Rows)
                        {
                            List<string> fields = new List<string>();
                            foreach (DataColumn column in dataTable.Columns)
                            {
                                string field = row[column].ToString();
                                // معالجة الحقول التي تحتوي على فاصل عمودي
                                field = field.Replace("|", "\\|");
                                fields.Add(field);
                            }
                            writer.WriteLine(string.Join("|", fields));
                        }

                        writer.WriteLine($"--- END TABLE {tableName} ---");
                        writer.WriteLine();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ أثناء تصدير البيانات: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // استيراد بيانات من ملف النظام
        public static Dictionary<string, DataTable> ImportFromSystemBackup(string filePath)
        {
            try
            {
                Dictionary<string, DataTable> dataTables = new Dictionary<string, DataTable>();

                string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);

                string currentTable = null;
                DataTable currentDataTable = null;
                bool isHeaderRead = false;

                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i].Trim();

                    if (line.StartsWith("--- BEGIN TABLE ") && line.EndsWith(" ---"))
                    {
                        // بداية جدول جديد
                        currentTable = line.Substring("--- BEGIN TABLE ".Length, line.Length - "--- BEGIN TABLE ".Length - " ---".Length);
                        currentDataTable = new DataTable();
                        isHeaderRead = false;
                    }
                    else if (line.StartsWith("--- END TABLE ") && line.EndsWith(" ---"))
                    {
                        // نهاية الجدول
                        if (currentTable != null && currentDataTable != null)
                        {
                            dataTables[currentTable] = currentDataTable;
                        }
                        currentTable = null;
                        currentDataTable = null;
                        isHeaderRead = false;
                    }
                    else if (currentTable != null && currentDataTable != null && !string.IsNullOrWhiteSpace(line))
                    {
                        // قراءة البيانات
                        if (!isHeaderRead)
                        {
                            // قراءة أسماء الأعمدة
                            string[] columnNames = line.Split('|');
                            foreach (string columnName in columnNames)
                            {
                                currentDataTable.Columns.Add(columnName.Trim());
                            }
                            isHeaderRead = true;
                        }
                        else
                        {
                            // قراءة الصفوف
                            string[] fields = SplitByPipe(line);
                            DataRow dataRow = currentDataTable.NewRow();

                            for (int j = 0; j < Math.Min(fields.Length, currentDataTable.Columns.Count); j++)
                            {
                                string field = fields[j].Replace("\\|", "|").Trim();
                                dataRow[j] = field;
                            }

                            currentDataTable.Rows.Add(dataRow);
                        }
                    }
                }

                return dataTables;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ أثناء استيراد البيانات: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        // تقسيم النص بالفاصل العمودي مع مراعاة الهروب
        private static string[] SplitByPipe(string line)
        {
            List<string> result = new List<string>();
            StringBuilder currentField = new StringBuilder();
            bool escapeNext = false;

            for (int i = 0; i < line.Length; i++)
            {
                if (escapeNext)
                {
                    currentField.Append(line[i]);
                    escapeNext = false;
                }
                else if (line[i] == '\\')
                {
                    escapeNext = true;
                }
                else if (line[i] == '|')
                {
                    result.Add(currentField.ToString());
                    currentField.Clear();
                }
                else
                {
                    currentField.Append(line[i]);
                }
            }

            // إضافة الحقل الأخير
            result.Add(currentField.ToString());

            return result.ToArray();
        }

        // تصدير التقرير إلى ملف PDF
        public static bool ExportToPDF(DataTable dataTable, string title, string filePath)
        {
            try
            {
                // هذه وظيفة تحتاج إلى مكتبة خارجية مثل iTextSharp
                // يمكن تنفيذها عند الحاجة

                MessageBox.Show("سيتم تنفيذ خاصية التصدير إلى PDF في الإصدار القادم", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ أثناء تصدير البيانات إلى PDF: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // طباعة التقرير
        public static bool PrintReport(DataTable dataTable, string title)
        {
            try
            {
                // هذه وظيفة تحتاج إلى تنفيذ مفصل للطباعة
                // يمكن تنفيذها عند الحاجة

                MessageBox.Show("سيتم تنفيذ خاصية الطباعة في الإصدار القادم", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ أثناء طباعة التقرير: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // استيراد بيانات الكتب من ملف CSV
        public static bool ImportBooksFromCSV(string filePath)
        {
            try
            {
                DataTable booksTable = ImportFromCSV(filePath);

                if (booksTable == null || booksTable.Rows.Count == 0)
                {
                    return false;
                }

                // التحقق من وجود الأعمدة المطلوبة
                string[] requiredColumns = { "Title", "Author", "Year", "Category", "TotalCopies", "ISBN" };
                foreach (string column in requiredColumns)
                {
                    if (!booksTable.Columns.Contains(column))
                    {
                        throw new Exception($"الملف لا يحتوي على العمود المطلوب: {column}");
                    }
                }

                // استيراد الكتب
                int successCount = 0;
                int failCount = 0;

                using (SQLiteConnection connection = DatabaseManager.GetConnection())
                {
                    connection.Open();

                    foreach (DataRow row in booksTable.Rows)
                    {
                        try
                        {
                            string title = row["Title"].ToString();
                            string author = row["Author"].ToString();
                            int year = Convert.ToInt32(row["Year"]);
                            string category = row["Category"].ToString();
                            int copies = Convert.ToInt32(row["TotalCopies"]);
                            string isbn = row["ISBN"].ToString();

                            bool success = DatabaseManager.AddBook(title, author, year, category, copies, isbn);

                            if (success)
                            {
                                successCount++;
                            }
                            else
                            {
                                failCount++;
                            }
                        }
                        catch (Exception)
                        {
                            failCount++;
                        }
                    }
                }

                MessageBox.Show($"تم استيراد {successCount} كتاب بنجاح. فشل استيراد {failCount} كتاب.", "استيراد الكتب", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return successCount > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ أثناء استيراد بيانات الكتب: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // استيراد بيانات الأعضاء من ملف CSV
        public static bool ImportMembersFromCSV(string filePath)
        {
            try
            {
                DataTable membersTable = ImportFromCSV(filePath);

                if (membersTable == null || membersTable.Rows.Count == 0)
                {
                    return false;
                }

                // التحقق من وجود الأعمدة المطلوبة
                string[] requiredColumns = { "Name", "Phone", "Email", "Address", "Status" };
                foreach (string column in requiredColumns)
                {
                    if (!membersTable.Columns.Contains(column))
                    {
                        throw new Exception($"الملف لا يحتوي على العمود المطلوب: {column}");
                    }
                }

                // استيراد الأعضاء
                int successCount = 0;
                int failCount = 0;

                using (SQLiteConnection connection = DatabaseManager.GetConnection())
                {
                    connection.Open();

                    foreach (DataRow row in membersTable.Rows)
                    {
                        try
                        {
                            string name = row["Name"].ToString();
                            string phone = row["Phone"].ToString();
                            string email = row["Email"].ToString();
                            string address = row["Address"].ToString();
                            string status = row["Status"].ToString();

                            bool success = DatabaseManager.AddMember(name, phone, email, address, status);

                            if (success)
                            {
                                successCount++;
                            }
                            else
                            {
                                failCount++;
                            }
                        }
                        catch (Exception)
                        {
                            failCount++;
                        }
                    }
                }

                MessageBox.Show($"تم استيراد {successCount} عضو بنجاح. فشل استيراد {failCount} عضو.", "استيراد الأعضاء", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return successCount > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ أثناء استيراد بيانات الأعضاء: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}