// هيكل قاعدة البيانات SQLite

using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;
using System.IO;

namespace LibraryManagementSystem
{
    public static class DatabaseManager
    {
        private static string dbPath = "LibraryDB.sqlite";
        private static string connectionString = $"Data Source={dbPath};Version=3;";

        public static void InitializeDatabase()
        {
            // إنشاء قاعدة البيانات إذا لم تكن موجودة
            if (!File.Exists(dbPath))
            {
                SQLiteConnection.CreateFile(dbPath);

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
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

                    using (SQLiteCommand command = new SQLiteCommand(createBooksTable, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    using (SQLiteCommand command = new SQLiteCommand(createMembersTable, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    using (SQLiteCommand command = new SQLiteCommand(createBorrowingsTable, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // إضافة بعض البيانات الافتراضية للاختبار
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

                    using (SQLiteCommand command = new SQLiteCommand(insertSampleData, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public static SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(connectionString);
        }

        // دوال التعامل مع الكتب
        public static DataTable GetAllBooks()
        {
            DataTable dataTable = new DataTable();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
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

            return dataTable;
        }

        public static bool AddBook(string title, string author, int year, string category, int copies, string isbn)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
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
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء إضافة الكتاب: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static bool UpdateBook(int bookId, string title, string author, int year, string category, int copies, string isbn)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
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
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء تحديث الكتاب: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static bool DeleteBook(int bookId)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
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
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء حذف الكتاب: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static DataTable SearchBooks(string searchText, string searchBy)
        {
            DataTable dataTable = new DataTable();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
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

            return dataTable;
        }

        // دوال التعامل مع الأعضاء
        public static DataTable GetAllMembers()
        {
            DataTable dataTable = new DataTable();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
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

            return dataTable;
        }

        public static bool AddMember(string name, string phone, string email, string address, string status)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
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
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء إضافة العضو: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static bool UpdateMember(int memberId, string name, string phone, string email, string address, string status)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
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
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء تحديث بيانات العضو: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static bool DeleteMember(int memberId)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
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
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء حذف العضو: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static DataTable SearchMembers(string searchText)
        {
            DataTable dataTable = new DataTable();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
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

            return dataTable;
        }

        // دوال التعامل مع الإعارات
        public static DataTable GetAllBorrowings()
        {
            DataTable dataTable = new DataTable();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
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

            return dataTable;
        }

        public static bool BorrowBook(int bookId, int memberId, DateTime dueDate)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
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
                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
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

        public static bool ReturnBook(int borrowId)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
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
                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
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

        // دوال الإحصائيات
        public static Dictionary<string, int> GetStatistics()
        {
            Dictionary<string, int> stats = new Dictionary<string, int>();

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // إجمالي عدد الكتب
                    string totalBooksQuery = "SELECT SUM(TotalCopies) FROM Books";
                    using (SQLiteCommand command = new SQLiteCommand(totalBooksQuery, connection))
                    {
                        stats["إجمالي الكتب"] = Convert.ToInt32(command.ExecuteScalar());
                    }

                    // الكتب المتاحة
                    string availableBooksQuery = "SELECT SUM(AvailableCopies) FROM Books";
                    using (SQLiteCommand command = new SQLiteCommand(availableBooksQuery, connection))
                    {
                        stats["الكتب المتاحة"] = Convert.ToInt32(command.ExecuteScalar());
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
            }

            return stats;
        }
    }
}