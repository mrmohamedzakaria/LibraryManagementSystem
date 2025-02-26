// DatabaseManager.cs - إدارة التعامل مع قاعدة البيانات
using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;

namespace LibraryManagementSystem
{
    public static class DatabaseManager
    {
        private static string connectionString = $"Data Source={AppConfig.DatabasePath};Version=3;";

        // الحصول على اتصال جديد بقاعدة البيانات
        public static SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(connectionString);
        }

        // تهيئة قاعدة البيانات
        public static void InitializeDatabase()
        {
            try
            {
                // التأكد من وجود المجلدات اللازمة
                AppConfig.EnsureDataDirectoryExists();

                // إنشاء قاعدة البيانات إذا لم تكن موجودة
                if (!AppConfig.CheckDatabaseExists())
                {
                    SQLiteConnection.CreateFile(AppConfig.DatabasePath);
                    CreateDatabaseSchema();
                    InsertSampleData();
                    AppConfig.LogToFile(AppConfig.LogLevel.Info, "تم إنشاء قاعدة البيانات بنجاح");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ أثناء تهيئة قاعدة البيانات: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AppConfig.LogToFile(AppConfig.LogLevel.Fatal, $"فشل تهيئة قاعدة البيانات: {ex.Message}");
            }
        }

        // إنشاء هيكل قاعدة البيانات
        private static void CreateDatabaseSchema()
        {
            using (SQLiteConnection connection = GetConnection())
            {
                connection.Open();

                // إنشاء جدول الكتب
                string createBooksTable = @"
                CREATE TABLE Books (
                    BookID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title NVARCHAR(150) NOT NULL,
                    Author NVARCHAR(100) NOT NULL,
                    Year INTEGER,
                    Category NVARCHAR(50),
                    AvailableCopies INTEGER,
                    TotalCopies INTEGER,
                    ISBN NVARCHAR(20) UNIQUE
                )";

                // إنشاء جدول الأعضاء
                string createMembersTable = @"
                CREATE TABLE Members (
                    MemberID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name NVARCHAR(100) NOT NULL,
                    Phone NVARCHAR(20),
                    Email NVARCHAR(100),
                    Address NVARCHAR(200),
                    RegistrationDate DATE,
                    Status NVARCHAR(20)
                )";

                // إنشاء جدول الإعارات
                string createBorrowingsTable = @"
                CREATE TABLE Borrowings (
                    BorrowID INTEGER PRIMARY KEY AUTOINCREMENT,
                    BookID INTEGER,
                    MemberID INTEGER,
                    BorrowDate DATE,
                    DueDate DATE,
                    ReturnDate DATE,
                    Status NVARCHAR(20),
                    FOREIGN KEY (BookID) REFERENCES Books(BookID),
                    FOREIGN KEY (MemberID) REFERENCES Members(MemberID)
                )";

                // تنفيذ استعلامات إنشاء الجداول
                using (SQLiteTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        ExecuteNonQuery(connection, transaction, createBooksTable);
                        ExecuteNonQuery(connection, transaction, createMembersTable);
                        ExecuteNonQuery(connection, transaction, createBorrowingsTable);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception($"فشل إنشاء هيكل قاعدة البيانات: {ex.Message}", ex);
                    }
                }
            }
        }

        // إضافة بيانات توضيحية للاختبار
        private static void InsertSampleData()
        {
            using (SQLiteConnection connection = GetConnection())
            {
                connection.Open();

                string insertSampleData = @"
                INSERT INTO Books (Title, Author, Year, Category, AvailableCopies, TotalCopies, ISBN)
                VALUES 
                    ('ألف ليلة وليلة', 'تراث شعبي', 1850, 'أدب', 5, 5, 'ARB-001'),
                    ('مقدمة ابن خلدون', 'ابن خلدون', 1377, 'فلسفة', 3, 3, 'ARB-002'),
                    ('البخلاء', 'الجاحظ', 869, 'أدب', 2, 2, 'ARB-003');

                INSERT INTO Members (Name, Phone, Email, Address, RegistrationDate, Status)
                VALUES 
                    ('أحمد محمود', '0123456789', 'ahmed@example.com', 'القاهرة، مصر', '2023-01-15', 'نشط'),
                    ('سارة علي', '0123456788', 'sara@example.com', 'دمشق، سوريا', '2023-02-20', 'نشط');
                ";

                using (SQLiteTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        ExecuteNonQuery(connection, transaction, insertSampleData);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception($"فشل إدخال البيانات التوضيحية: {ex.Message}", ex);
                    }
                }
            }
        }

        // تنفيذ استعلام بدون إرجاع نتائج
        private static int ExecuteNonQuery(SQLiteConnection connection, SQLiteTransaction transaction, string query, Dictionary<string, object> parameters = null!)
        {
            using SQLiteCommand command = new SQLiteCommand(query, connection, transaction);
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value);
                }
            }
            return command.ExecuteNonQuery();
        }

        #region دوال التعامل مع الكتب

        // الحصول على جميع الكتب
        public static DataTable GetAllBooks()
        {
            DataTable dataTable = new DataTable();

            try
            {
                using (SQLiteConnection connection = GetConnection())
                {
                    connection.Open();
                    string query = "SELECT * FROM Books";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AppConfig.LogToFile(AppConfig.LogLevel.Error, $"خطأ في استعلام الكتب: {ex.Message}");
            }

            return dataTable;
        }

        // إضافة كتاب جديد
        public static bool AddBook(string title, string author, int year, string category, int copies, string isbn)
        {
            try
            {
                using (SQLiteConnection connection = GetConnection())
                {
                    connection.Open();
                    string query = @"
                    INSERT INTO Books (Title, Author, Year, Category, AvailableCopies, TotalCopies, ISBN)
                    VALUES (@Title, @Author, @Year, @Category, @Copies, @Copies, @ISBN)";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Title", title);
                        command.Parameters.AddWithValue("@Author", author);
                        command.Parameters.AddWithValue("@Year", year);
                        command.Parameters.AddWithValue("@Category", category);
                        command.Parameters.AddWithValue("@Copies", copies);
                        command.Parameters.AddWithValue("@ISBN", isbn);

                        int result = command.ExecuteNonQuery();
                        AppConfig.LogToFile(AppConfig.LogLevel.Info, $"تمت إضافة كتاب جديد: {title}");
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء إضافة الكتاب: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AppConfig.LogToFile(AppConfig.LogLevel.Error, $"فشلت إضافة كتاب: {ex.Message}");
                return false;
            }
        }

        // تحديث بيانات كتاب
        public static bool UpdateBook(int bookId, string title, string author, int year, string category, int copies, string isbn)
        {
            try
            {
                using (SQLiteConnection connection = GetConnection())
                {
                    connection.Open();
                    string query = @"
                    UPDATE Books 
                    SET Title = @Title, Author = @Author, Year = @Year, Category = @Category, 
                        TotalCopies = @Copies, AvailableCopies = @Copies - (TotalCopies - AvailableCopies), ISBN = @ISBN
                    WHERE BookID = @BookID";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@BookID", bookId);
                        command.Parameters.AddWithValue("@Title", title);
                        command.Parameters.AddWithValue("@Author", author);
                        command.Parameters.AddWithValue("@Year", year);
                        command.Parameters.AddWithValue("@Category", category);
                        command.Parameters.AddWithValue("@Copies", copies);
                        command.Parameters.AddWithValue("@ISBN", isbn);

                        int result = command.ExecuteNonQuery();
                        AppConfig.LogToFile(AppConfig.LogLevel.Info, $"تم تحديث كتاب: (ID: {bookId}) {title}");
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء تحديث الكتاب: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AppConfig.LogToFile(AppConfig.LogLevel.Error, $"فشل تحديث كتاب: {ex.Message}");
                return false;
            }
        }

        // حذف كتاب
        public static bool DeleteBook(int bookId)
        {
            try
            {
                using (SQLiteConnection connection = GetConnection())
                {
                    connection.Open();
                    // تحقق أولاً إذا كان الكتاب معار
                    string checkQuery = @"
                    SELECT COUNT(*) FROM Borrowings 
                    WHERE BookID = @BookID AND (ReturnDate IS NULL OR Status = 'معار')";

                    using (SQLiteCommand checkCommand = new SQLiteCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@BookID", bookId);
                        int borrowedCount = Convert.ToInt32(checkCommand.ExecuteScalar());

                        if (borrowedCount > 0)
                        {
                            MessageBox.Show("لا يمكن حذف هذا الكتاب لأنه معار حالياً", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                    }

                    // حذف الكتاب إذا لم يكن معاراً
                    string deleteQuery = "DELETE FROM Books WHERE BookID = @BookID";

                    using (SQLiteCommand command = new SQLiteCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@BookID", bookId);
                        int result = command.ExecuteNonQuery();
                        AppConfig.LogToFile(AppConfig.LogLevel.Info, $"تم حذف كتاب: (ID: {bookId})");
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء حذف الكتاب: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AppConfig.LogToFile(AppConfig.LogLevel.Error, $"فشل حذف كتاب: {ex.Message}");
                return false;
            }
        }

        // البحث عن الكتب
        public static DataTable SearchBooks(string searchText, string searchBy)
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (SQLiteConnection connection = GetConnection())
                {
                    connection.Open();
                    string query = "";

                    switch (searchBy)
                    {
                        case "العنوان":
                            query = "SELECT * FROM Books WHERE Title LIKE @SearchText";
                            break;
                        case "المؤلف":
                            query = "SELECT * FROM Books WHERE Author LIKE @SearchText";
                            break;
                        case "التصنيف":
                            query = "SELECT * FROM Books WHERE Category LIKE @SearchText";
                            break;
                        case "ISBN":
                            query = "SELECT * FROM Books WHERE ISBN LIKE @SearchText";
                            break;
                        default:
                            query = "SELECT * FROM Books WHERE Title LIKE @SearchText OR Author LIKE @SearchText OR Category LIKE @SearchText OR ISBN LIKE @SearchText";
                            break;
                    }

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SearchText", "%" + searchText + "%");
                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AppConfig.LogToFile(AppConfig.LogLevel.Error, $"خطأ في البحث عن الكتب: {ex.Message}");
            }

            return dataTable;
        }
        #endregion

        #region دوال التعامل مع الأعضاء

        // الحصول على جميع الأعضاء
        public static DataTable GetAllMembers()
        {
            DataTable dataTable = new DataTable();

            try
            {
                using (SQLiteConnection connection = GetConnection())
                {
                    connection.Open();
                    string query = "SELECT * FROM Members";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AppConfig.LogToFile(AppConfig.LogLevel.Error, $"خطأ في استعلام الأعضاء: {ex.Message}");
            }

            return dataTable;
        }

        // إضافة عضو جديد
        public static bool AddMember(string name, string phone, string email, string address, string status)
        {
            try
            {
                using (SQLiteConnection connection = GetConnection())
                {
                    connection.Open();
                    string query = @"
                    INSERT INTO Members (Name, Phone, Email, Address, RegistrationDate, Status)
                    VALUES (@Name, @Phone, @Email, @Address, @RegDate, @Status)";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@Phone", phone);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Address", address);
                        command.Parameters.AddWithValue("@RegDate", DateTime.Now.ToString("yyyy-MM-dd"));
                        command.Parameters.AddWithValue("@Status", status);

                        int result = command.ExecuteNonQuery();
                        AppConfig.LogToFile(AppConfig.LogLevel.Info, $"تمت إضافة عضو جديد: {name}");
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء إضافة العضو: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AppConfig.LogToFile(AppConfig.LogLevel.Error, $"فشلت إضافة عضو: {ex.Message}");
                return false;
            }
        }

        // تحديث بيانات عضو
        public static bool UpdateMember(int memberId, string name, string phone, string email, string address, string status)
        {
            try
            {
                using (SQLiteConnection connection = GetConnection())
                {
                    connection.Open();
                    string query = @"
                    UPDATE Members 
                    SET Name = @Name, Phone = @Phone, Email = @Email, Address = @Address, Status = @Status
                    WHERE MemberID = @MemberID";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@MemberID", memberId);
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@Phone", phone);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Address", address);
                        command.Parameters.AddWithValue("@Status", status);

                        int result = command.ExecuteNonQuery();
                        AppConfig.LogToFile(AppConfig.LogLevel.Info, $"تم تحديث عضو: (ID: {memberId}) {name}");
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء تحديث بيانات العضو: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AppConfig.LogToFile(AppConfig.LogLevel.Error, $"فشل تحديث عضو: {ex.Message}");
                return false;
            }
        }

        // حذف عضو
        public static bool DeleteMember(int memberId)
        {
            try
            {
                using (SQLiteConnection connection = GetConnection())
                {
                    connection.Open();
                    // تحقق أولاً إذا كان العضو لديه كتب معارة
                    string checkQuery = @"
                    SELECT COUNT(*) FROM Borrowings 
                    WHERE MemberID = @MemberID AND (ReturnDate IS NULL OR Status = 'معار')";

                    using (SQLiteCommand checkCommand = new SQLiteCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@MemberID", memberId);
                        int borrowedCount = Convert.ToInt32(checkCommand.ExecuteScalar());

                        if (borrowedCount > 0)
                        {
                            MessageBox.Show("لا يمكن حذف هذا العضو لأن لديه كتب معارة", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                    }

                    // حذف العضو إذا لم يكن لديه كتب معارة
                    string deleteQuery = "DELETE FROM Members WHERE MemberID = @MemberID";

                    using (SQLiteCommand command = new SQLiteCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@MemberID", memberId);
                        int result = command.ExecuteNonQuery();
                        AppConfig.LogToFile(AppConfig.LogLevel.Info, $"تم حذف عضو: (ID: {memberId})");
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء حذف العضو: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AppConfig.LogToFile(AppConfig.LogLevel.Error, $"فشل حذف عضو: {ex.Message}");
                return false;
            }
        }

        // البحث عن الأعضاء
        public static DataTable SearchMembers(string searchText)
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (SQLiteConnection connection = GetConnection())
                {
                    connection.Open();
                    string query = "SELECT * FROM Members WHERE Name LIKE @SearchText OR Phone LIKE @SearchText OR Email LIKE @SearchText";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SearchText", "%" + searchText + "%");
                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AppConfig.LogToFile(AppConfig.LogLevel.Error, $"خطأ في البحث عن الأعضاء: {ex.Message}");
            }

            return dataTable;
        }
        #endregion

        #region دوال التعامل مع الإعارات

        // الحصول على جميع الإعارات
        public static DataTable GetAllBorrowings()
        {
            DataTable dataTable = new DataTable();

            try
            {
                using (SQLiteConnection connection = GetConnection())
                {
                    connection.Open();
                    string query = @"
                        SELECT b.BorrowID, bk.Title, m.Name, b.BorrowDate, b.DueDate, b.ReturnDate, b.Status
                        FROM Borrowings b
                        JOIN Books bk ON b.BookID = bk.BookID
                        JOIN Members m ON b.MemberID = m.MemberID
                        ORDER BY b.BorrowDate DESC";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AppConfig.LogToFile(AppConfig.LogLevel.Error, $"خطأ في استعلام الإعارات: {ex.Message}");
            }

            return dataTable;
        }

        // إعارة كتاب
        public static bool BorrowBook(int bookId, int memberId, DateTime dueDate)
        {
            try
            {
                using (SQLiteConnection connection = GetConnection())
                {
                    connection.Open();

                    // تحقق من توفر الكتاب
                    string checkQuery = "SELECT AvailableCopies FROM Books WHERE BookID = @BookID";
                    using (SQLiteCommand checkCommand = new SQLiteCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@BookID", bookId);
                        int availableCopies = Convert.ToInt32(checkCommand.ExecuteScalar());

                        if (availableCopies <= 0)
                        {
                            MessageBox.Show("هذا الكتاب غير متوفر حالياً للإعارة", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                    }

                    // إضافة سجل الإعارة
                    using (SQLiteTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // إضافة سجل الإعارة
                            string borrowQuery = @"
                            INSERT INTO Borrowings (BookID, MemberID, BorrowDate, DueDate, Status)
                            VALUES (@BookID, @MemberID, @BorrowDate, @DueDate, 'معار')";

                            using (SQLiteCommand borrowCommand = new SQLiteCommand(borrowQuery, connection, transaction))
                            {
                                borrowCommand.Parameters.AddWithValue("@BookID", bookId);
                                borrowCommand.Parameters.AddWithValue("@MemberID", memberId);
                                borrowCommand.Parameters.AddWithValue("@BorrowDate", DateTime.Now.ToString("yyyy-MM-dd"));
                                borrowCommand.Parameters.AddWithValue("@DueDate", dueDate.ToString("yyyy-MM-dd"));
                                borrowCommand.ExecuteNonQuery();
                            }

                            // تحديث عدد النسخ المتاحة
                            string updateQuery = @"
                            UPDATE Books 
                            SET AvailableCopies = AvailableCopies - 1
                            WHERE BookID = @BookID";

                            using (SQLiteCommand updateCommand = new SQLiteCommand(updateQuery, connection, transaction))
                            {
                                updateCommand.Parameters.AddWithValue("@BookID", bookId);
                                updateCommand.ExecuteNonQuery();
                            }

                            transaction.Commit();
                            AppConfig.LogToFile(AppConfig.LogLevel.Info, $"تمت إعارة كتاب: (BookID: {bookId}) للعضو (MemberID: {memberId})");
                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            AppConfig.LogToFile(AppConfig.LogLevel.Error, $"فشلت إعارة كتاب: {ex.Message}");
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء إعارة الكتاب: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // إرجاع كتاب
        public static bool ReturnBook(int borrowId)
        {
            try
            {
                using (SQLiteConnection connection = GetConnection())
                {
                    connection.Open();

                    // الحصول على معرف الكتاب من سجل الإعارة
                    string getBookIdQuery = "SELECT BookID FROM Borrowings WHERE BorrowID = @BorrowID";
                    int bookId;

                    using (SQLiteCommand getBookIdCommand = new SQLiteCommand(getBookIdQuery, connection))
                    {
                        getBookIdCommand.Parameters.AddWithValue("@BorrowID", borrowId);
                        bookId = Convert.ToInt32(getBookIdCommand.ExecuteScalar());
                    }

                    // تحديث سجل الإعارة والكتاب في transaction واحدة
                    using (SQLiteTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // تحديث سجل الإعارة
                            string updateBorrowingQuery = @"
                            UPDATE Borrowings 
                            SET ReturnDate = @ReturnDate, Status = 'مسترجع'
                            WHERE BorrowID = @BorrowID";

                            using (SQLiteCommand borrowCommand = new SQLiteCommand(updateBorrowingQuery, connection, transaction))
                            {
                                borrowCommand.Parameters.AddWithValue("@BorrowID", borrowId);
                                borrowCommand.Parameters.AddWithValue("@ReturnDate", DateTime.Now.ToString("yyyy-MM-dd"));
                                borrowCommand.ExecuteNonQuery();
                            }

                            // تحديث عدد النسخ المتاحة
                            string updateBookQuery = @"
                            UPDATE Books 
                            SET AvailableCopies = AvailableCopies + 1
                            WHERE BookID = @BookID";

                            using (SQLiteCommand updateCommand = new SQLiteCommand(updateBookQuery, connection, transaction))
                            {
                                updateCommand.Parameters.AddWithValue("@BookID", bookId);
                                updateCommand.ExecuteNonQuery();
                            }

                            transaction.Commit();
                            AppConfig.LogToFile(AppConfig.LogLevel.Info, $"تم إرجاع كتاب: (BorrowID: {borrowId})");
                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            AppConfig.LogToFile(AppConfig.LogLevel.Error, $"فشل إرجاع كتاب: {ex.Message}");
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء إرجاع الكتاب: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        #endregion

        #region دوال الإحصائيات

        // الحصول على إحصائيات النظام
        public static Dictionary<string, int> GetStatistics()
        {
            Dictionary<string, int> stats = new Dictionary<string, int>();

            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();

                    // إجمالي عدد الكتب
                    string totalBooksQuery = "SELECT SUM(TotalCopies) FROM Books";
                    using (SQLiteCommand command = new SQLiteCommand(totalBooksQuery, connection))
                    {
                        object result = command.ExecuteScalar();
                        stats["إجمالي الكتب"] = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                    }

                    // الكتب المتاحة
                    string availableBooksQuery = "SELECT SUM(AvailableCopies) FROM Books";
                    using (SQLiteCommand command = new SQLiteCommand(availableBooksQuery, connection))
                    {
                        object result = command.ExecuteScalar();
                        stats["الكتب المتاحة"] = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                    }

                    // الكتب المعارة
                    string borrowedBooksQuery = "SELECT COUNT(*) FROM Borrowings WHERE ReturnDate IS NULL";
                    using (SQLiteCommand command = new SQLiteCommand(borrowedBooksQuery, connection))
                    {
                        stats["الكتب المعارة"] = Convert.ToInt32(command.ExecuteScalar());
                    }

                    // إجمالي الأعضاء
                    string totalMembersQuery = "SELECT COUNT(*) FROM Members";
                    using (SQLiteCommand command = new SQLiteCommand(totalMembersQuery, connection))
                    {
                        stats["إجمالي الأعضاء"] = Convert.ToInt32(command.ExecuteScalar());
                    }

                    // الأعضاء النشطين
                    string activeMembersQuery = "SELECT COUNT(*) FROM Members WHERE Status = 'نشط'";
                    using (SQLiteCommand command = new SQLiteCommand(activeMembersQuery, connection))
                    {
                        stats["الأعضاء النشطين"] = Convert.ToInt32(command.ExecuteScalar());
                    }

                    // عدد الإعارات اليوم
                    string todayBorrowingsQuery = "SELECT COUNT(*) FROM Borrowings WHERE BorrowDate = @Today";
                    using (SQLiteCommand command = new SQLiteCommand(todayBorrowingsQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Today", DateTime.Now.ToString("yyyy-MM-dd"));
                        stats["إعارات اليوم"] = Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء استرجاع الإحصائيات: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AppConfig.LogToFile(AppConfig.LogLevel.Error, $"فشل استرجاع الإحصائيات: {ex.Message}");
            }

            return stats;
        }

        // الحصول على إحصائيات موسعة
        public static DataTable GetDetailedStatistics()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("الفئة", typeof(string));
            dataTable.Columns.Add("القيمة", typeof(int));
            dataTable.Columns.Add("النسبة", typeof(string));

            try
            {
                Dictionary<string, int> stats = GetStatistics();
                int totalBooks = stats["إجمالي الكتب"];
                int totalMembers = stats["إجمالي الأعضاء"];

                foreach (var stat in stats)
                {
                    string percentage = "";
                    if (stat.Key.Contains("الكتب"))
                    {
                        if (totalBooks > 0)
                            percentage = $"{(stat.Value * 100.0 / totalBooks):0.00}%";
                    }
                    else if (stat.Key.Contains("الأعضاء"))
                    {
                        if (totalMembers > 0)
                            percentage = $"{(stat.Value * 100.0 / totalMembers):0.00}%";
                    }

                    dataTable.Rows.Add(stat.Key, stat.Value, percentage);
                }

                // إضافة إحصائيات إضافية
                using (var connection = GetConnection())
                {
                    connection.Open();

                    // توزيع الكتب حسب التصنيف
                    string categoryStatsQuery = @"
                    SELECT Category, COUNT(*) as Count
                    FROM Books
                    GROUP BY Category
                    ORDER BY Count DESC";

                    using (SQLiteCommand command = new SQLiteCommand(categoryStatsQuery, connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string category = reader["Category"].ToString();
                                int count = Convert.ToInt32(reader["Count"]);
                                string percentage = totalBooks > 0 ? $"{(count * 100.0 / totalBooks):0.00}%" : "0%";
                                dataTable.Rows.Add($"تصنيف: {category}", count, percentage);
                            }
                        }
                    }

                    // الكتب المتأخرة
                    string overdueStatsQuery = @"
                    SELECT COUNT(*) as Count
                    FROM Borrowings
                    WHERE ReturnDate IS NULL AND DueDate < @Today";

                    using (SQLiteCommand command = new SQLiteCommand(overdueStatsQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Today", DateTime.Now.ToString("yyyy-MM-dd"));
                        int overdueCount = Convert.ToInt32(command.ExecuteScalar());
                        string percentage = stats["الكتب المعارة"] > 0 ? $"{(overdueCount * 100.0 / stats["الكتب المعارة"]):0.00}%" : "0%";
                        dataTable.Rows.Add("الكتب المتأخرة", overdueCount, percentage);
                    }
                }
            }
            catch (Exception ex)
            {
                AppConfig.LogToFile(AppConfig.LogLevel.Error, $"فشل استرجاع الإحصائيات المفصلة: {ex.Message}");
            }

            return dataTable;
        }
        #endregion

        #region دوال التصدير والاستيراد

        // تصدير قاعدة البيانات كاملة
        public static bool ExportFullDatabase(string filePath)
        {
            Dictionary<string, DataTable> dataTables = new Dictionary<string, DataTable>();

            try
            {
                // استخراج بيانات جميع الجداول
                dataTables["Books"] = GetAllBooks();
                dataTables["Members"] = GetAllMembers();
                dataTables["Borrowings"] = GetAllBorrowings();

                // تصدير البيانات
                bool success = ExportImportManager.ExportToSystemBackup(dataTables, filePath);

                if (success)
                    AppConfig.LogToFile(AppConfig.LogLevel.Info, $"تم تصدير قاعدة البيانات بنجاح إلى: {filePath}");
                else
                    AppConfig.LogToFile(AppConfig.LogLevel.Error, "فشل تصدير قاعدة البيانات");

                return success;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ أثناء تصدير قاعدة البيانات: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AppConfig.LogToFile(AppConfig.LogLevel.Error, $"فشل تصدير قاعدة البيانات: {ex.Message}");
                return false;
            }
        }

        // استيراد قاعدة البيانات كاملة
        public static bool ImportFullDatabase(string filePath)
        {
            try
            {
                AppConfig.LogToFile(AppConfig.LogLevel.Info, $"بدء استيراد قاعدة البيانات من: {filePath}");

                // استيراد البيانات
                Dictionary<string, DataTable> dataTables = ExportImportManager.ImportFromSystemBackup(filePath);

                if (dataTables == null || dataTables.Count == 0)
                {
                    AppConfig.LogToFile(AppConfig.LogLevel.Error, "فشل استيراد قاعدة البيانات: ملف فارغ أو غير صالح");
                    return false;
                }

                using (SQLiteConnection connection = GetConnection())
                {
                    connection.Open();
                    AppConfig.LogToFile(AppConfig.LogLevel.Info, "بدء عملية استيراد البيانات");

                    using (SQLiteTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // حذف البيانات الحالية
                            using (SQLiteCommand command = new SQLiteCommand("DELETE FROM Borrowings", connection, transaction))
                            {
                                command.ExecuteNonQuery();
                            }

                            using (SQLiteCommand command = new SQLiteCommand("DELETE FROM Members", connection, transaction))
                            {
                                command.ExecuteNonQuery();
                            }

                            using (SQLiteCommand command = new SQLiteCommand("DELETE FROM Books", connection, transaction))
                            {
                                command.ExecuteNonQuery();
                            }

                            // إعادة ضبط تسلسل المعرفات
                            using (SQLiteCommand command = new SQLiteCommand("DELETE FROM sqlite_sequence", connection, transaction))
                            {
                                command.ExecuteNonQuery();
                            }

                            AppConfig.LogToFile(AppConfig.LogLevel.Info, "تم حذف البيانات الحالية");

                            // استيراد الكتب
                            if (dataTables.ContainsKey("Books"))
                            {
                                ImportBooksFromDataTable(dataTables["Books"], connection, transaction);
                                AppConfig.LogToFile(AppConfig.LogLevel.Info, $"تم استيراد {dataTables["Books"].Rows.Count} من الكتب");
                            }

                            // استيراد الأعضاء
                            if (dataTables.ContainsKey("Members"))
                            {
                                ImportMembersFromDataTable(dataTables["Members"], connection, transaction);
                                AppConfig.LogToFile(AppConfig.LogLevel.Info, $"تم استيراد {dataTables["Members"].Rows.Count} من الأعضاء");
                            }

                            // استيراد الإعارات
                            if (dataTables.ContainsKey("Borrowings"))
                            {
                                ImportBorrowingsFromDataTable(dataTables["Borrowings"], connection, transaction);
                                AppConfig.LogToFile(AppConfig.LogLevel.Info, $"تم استيراد {dataTables["Borrowings"].Rows.Count} من الإعارات");
                            }

                            transaction.Commit();
                            AppConfig.LogToFile(AppConfig.LogLevel.Info, "تم الانتهاء من استيراد قاعدة البيانات بنجاح");
                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            AppConfig.LogToFile(AppConfig.LogLevel.Error, $"فشل استيراد قاعدة البيانات: {ex.Message}");
                            throw new Exception($"حدث خطأ أثناء استيراد البيانات: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ أثناء استيراد البيانات: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AppConfig.LogToFile(AppConfig.LogLevel.Error, $"فشل استيراد قاعدة البيانات: {ex.Message}");
                return false;
            }
        }

        // استيراد الكتب من جدول بيانات
        private static void ImportBooksFromDataTable(DataTable dataTable, SQLiteConnection connection, SQLiteTransaction transaction)
        {
            string insertQuery = @"
                INSERT INTO Books (BookID, Title, Author, Year, Category, AvailableCopies, TotalCopies, ISBN)
                VALUES (@BookID, @Title, @Author, @Year, @Category, @AvailableCopies, @TotalCopies, @ISBN)";

            using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection, transaction))
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@BookID", row["BookID"]);
                    command.Parameters.AddWithValue("@Title", row["Title"]);
                    command.Parameters.AddWithValue("@Author", row["Author"]);
                    command.Parameters.AddWithValue("@Year", row["Year"]);
                    command.Parameters.AddWithValue("@Category", row["Category"]);
                    command.Parameters.AddWithValue("@AvailableCopies", row["AvailableCopies"]);
                    command.Parameters.AddWithValue("@TotalCopies", row["TotalCopies"]);
                    command.Parameters.AddWithValue("@ISBN", row["ISBN"]);

                    command.ExecuteNonQuery();
                }
            }
        }

        // استيراد الأعضاء من جدول بيانات
        private static void ImportMembersFromDataTable(DataTable dataTable, SQLiteConnection connection, SQLiteTransaction transaction)
        {
            string insertQuery = @"
                INSERT INTO Members (MemberID, Name, Phone, Email, Address, RegistrationDate, Status)
                VALUES (@MemberID, @Name, @Phone, @Email, @Address, @RegistrationDate, @Status)";

            using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection, transaction))
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@MemberID", row["MemberID"]);
                    command.Parameters.AddWithValue("@Name", row["Name"]);
                    command.Parameters.AddWithValue("@Phone", row["Phone"]);
                    command.Parameters.AddWithValue("@Email", row["Email"]);
                    command.Parameters.AddWithValue("@Address", row["Address"]);
                    command.Parameters.AddWithValue("@RegistrationDate", row["RegistrationDate"]);
                    command.Parameters.AddWithValue("@Status", row["Status"]);

                    command.ExecuteNonQuery();
                }
            }
        }

        // استيراد الإعارات من جدول بيانات
        private static void ImportBorrowingsFromDataTable(DataTable dataTable, SQLiteConnection connection, SQLiteTransaction transaction)
        {
            string insertQuery = @"
                INSERT INTO Borrowings (BorrowID, BookID, MemberID, BorrowDate, DueDate, ReturnDate, Status)
                VALUES (@BorrowID, @BookID, @MemberID, @BorrowDate, @DueDate, @ReturnDate, @Status)";

            using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection, transaction))
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@BorrowID", row["BorrowID"]);
                    command.Parameters.AddWithValue("@BookID", row["BookID"]);
                    command.Parameters.AddWithValue("@MemberID", row["MemberID"]);
                    command.Parameters.AddWithValue("@BorrowDate", row["BorrowDate"]);
                    command.Parameters.AddWithValue("@DueDate", row["DueDate"]);
                    command.Parameters.AddWithValue("@ReturnDate", DBNull.Value);

                    if (row["ReturnDate"] != DBNull.Value && !string.IsNullOrEmpty(row["ReturnDate"].ToString()))
                    {
                        command.Parameters["@ReturnDate"] = row["ReturnDate"];
                    }

                    command.Parameters.AddWithValue("@Status", row["Status"]);

                    command.ExecuteNonQuery();
                }
            }
        }

        // تصدير الكتب إلى CSV
        public static bool ExportBooksToCSV(string filePath)
        {
            try
            {
                DataTable booksTable = GetAllBooks();
                bool success = ExportImportManager.ExportToCSV(booksTable, filePath);

                if (success)
                    AppConfig.LogToFile(AppConfig.LogLevel.Info, $"تم تصدير بيانات الكتب بنجاح إلى: {filePath}");
                else
                    AppConfig.LogToFile(AppConfig.LogLevel.Error, "فشل تصدير بيانات الكتب");

                return success;
            }
            catch (Exception ex)
            {
                AppConfig.LogToFile(AppConfig.LogLevel.Error, $"فشل تصدير بيانات الكتب: {ex.Message}");
                return false;
            }
        }

        // تصدير الأعضاء إلى CSV
        public static bool ExportMembersToCSV(string filePath)
        {
            try
            {
                DataTable membersTable = GetAllMembers();
                bool success = ExportImportManager.ExportToCSV(membersTable, filePath);

                if (success)
                    AppConfig.LogToFile(AppConfig.LogLevel.Info, $"تم تصدير بيانات الأعضاء بنجاح إلى: {filePath}");
                else
                    AppConfig.LogToFile(AppConfig.LogLevel.Error, "فشل تصدير بيانات الأعضاء");

                return success;
            }
            catch (Exception ex)
            {
                AppConfig.LogToFile(AppConfig.LogLevel.Error, $"فشل تصدير بيانات الأعضاء: {ex.Message}");
                return false;
            }
        }

        // تصدير الإعارات إلى CSV
        public static bool ExportBorrowingsToCSV(string filePath)
        {
            try
            {
                DataTable borrowingsTable = GetAllBorrowings();
                bool success = ExportImportManager.ExportToCSV(borrowingsTable, filePath);

                if (success)
                    AppConfig.LogToFile(AppConfig.LogLevel.Info, $"تم تصدير بيانات الإعارات بنجاح إلى: {filePath}");
                else
                    AppConfig.LogToFile(AppConfig.LogLevel.Error, "فشل تصدير بيانات الإعارات");

                return success;
            }
            catch (Exception ex)
            {
                AppConfig.LogToFile(AppConfig.LogLevel.Error, $"فشل تصدير بيانات الإعارات: {ex.Message}");
                return false;
            }
        }

        // تصدير أي جدول بيانات إلى CSV
        public static bool ExportDataTableToCSV(DataTable dataTable, string filePath)
        {
            try
            {
                bool success = ExportImportManager.ExportToCSV(dataTable, filePath);

                if (success)
                    AppConfig.LogToFile(AppConfig.LogLevel.Info, $"تم تصدير البيانات بنجاح إلى: {filePath}");
                else
                    AppConfig.LogToFile(AppConfig.LogLevel.Error, "فشل تصدير البيانات");

                return success;
            }
            catch (Exception ex)
            {
                AppConfig.LogToFile(AppConfig.LogLevel.Error, $"فشل تصدير البيانات: {ex.Message}");
                return false;
            }
        }
        #endregion
    }
}