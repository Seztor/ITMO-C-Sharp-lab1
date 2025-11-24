using Microsoft.Data.Sqlite;

namespace itmo_lab2;

public class DatabaseManager
{
    private static DatabaseManager? instance;
    private readonly SqliteConnection connection;

    private DatabaseManager()
    {
        connection = new SqliteConnection("Data Source=courses_system.db");
        connection.Open();
        CreateTables();
    }

    public static DatabaseManager GetInstance()
    {
        if (instance == null)
        {
            instance = new DatabaseManager();
        }
        return instance;
    }

    private void CreateTables()
    {
        using var cmd1 = connection.CreateCommand();
        cmd1.CommandText = @"
            CREATE TABLE IF NOT EXISTS Teachers (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Email TEXT NOT NULL,
                Number TEXT NOT NULL
            )";
        cmd1.ExecuteNonQuery();

        using var cmd2 = connection.CreateCommand();
        cmd2.CommandText = @"
            CREATE TABLE IF NOT EXISTS Courses (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Title TEXT NOT NULL,
                Description TEXT,
                CourseType TEXT NOT NULL
            )";
        cmd2.ExecuteNonQuery();

        using var cmd3 = connection.CreateCommand();
        cmd3.CommandText = @"
            CREATE TABLE IF NOT EXISTS Students (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Email TEXT NOT NULL,
                Number TEXT NOT NULL
            )";
        cmd3.ExecuteNonQuery();

        using var cmd4 = connection.CreateCommand();
        cmd4.CommandText = @"
            CREATE TABLE IF NOT EXISTS CourseTeachers (
                CourseId INTEGER,
                TeacherId INTEGER,
                PRIMARY KEY(CourseId, TeacherId),
                FOREIGN KEY(CourseId) REFERENCES Courses(Id),
                FOREIGN KEY(TeacherId) REFERENCES Teachers(Id)
            )";
        cmd4.ExecuteNonQuery();

        using var cmd5 = connection.CreateCommand();
        cmd5.CommandText = @"
            CREATE TABLE IF NOT EXISTS CourseStudents (
                CourseId INTEGER,
                StudentId INTEGER,
                PRIMARY KEY(CourseId, StudentId),
                FOREIGN KEY(CourseId) REFERENCES Courses(Id),
                FOREIGN KEY(StudentId) REFERENCES Students(Id)
            )";
        cmd5.ExecuteNonQuery();
    }

    public void AddCourse(Course course)
    {
        using var cmd = connection.CreateCommand();
        cmd.CommandText = "INSERT INTO Courses (Title, Description, CourseType) VALUES (@title, @desc, @type)";
        cmd.Parameters.AddWithValue("@title", course.Title ?? "");
        cmd.Parameters.AddWithValue("@desc", course.Description ?? "");
        cmd.Parameters.AddWithValue("@type", course.CourseType ?? "");
        cmd.ExecuteNonQuery();
    }

    public void AddTeacher(Teacher teacher)
    {
        using var cmd = connection.CreateCommand();
        cmd.CommandText = "INSERT INTO Teachers (Name, Email, Number) VALUES (@name, @email, @number)";
        cmd.Parameters.AddWithValue("@name", teacher.Name ?? "");
        cmd.Parameters.AddWithValue("@email", teacher.Email ?? "");
        cmd.Parameters.AddWithValue("@number", teacher.Number ?? "");
        cmd.ExecuteNonQuery();
    }

    public void AddStudent(Student student)
    {
        using var cmd = connection.CreateCommand();
        cmd.CommandText = "INSERT INTO Students (Name, Email, Number) VALUES (@name, @email, @number)";
        cmd.Parameters.AddWithValue("@name", student.Name ?? "");
        cmd.Parameters.AddWithValue("@email", student.Email ?? "");
        cmd.Parameters.AddWithValue("@number", student.Number ?? "");
        cmd.ExecuteNonQuery();
    }

    public void DeleteCourse(int courseId)
    {
        using var cmd1 = connection.CreateCommand();
        cmd1.CommandText = "DELETE FROM CourseStudents WHERE CourseId = @id";
        cmd1.Parameters.AddWithValue("@id", courseId);
        cmd1.ExecuteNonQuery();

        using var cmd2 = connection.CreateCommand();
        cmd2.CommandText = "DELETE FROM CourseTeachers WHERE CourseId = @id";
        cmd2.Parameters.AddWithValue("@id", courseId);
        cmd2.ExecuteNonQuery();

        using var cmd3 = connection.CreateCommand();
        cmd3.CommandText = "DELETE FROM Courses WHERE Id = @id";
        cmd3.Parameters.AddWithValue("@id", courseId);
        cmd3.ExecuteNonQuery();
    }

    public void DeleteTeacher(int teacherId)
    {
        using var cmd1 = connection.CreateCommand();
        cmd1.CommandText = "DELETE FROM CourseTeachers WHERE TeacherId = @id";
        cmd1.Parameters.AddWithValue("@id", teacherId);
        cmd1.ExecuteNonQuery();

        using var cmd2 = connection.CreateCommand();
        cmd2.CommandText = "DELETE FROM Teachers WHERE Id = @id";
        cmd2.Parameters.AddWithValue("@id", teacherId);
        cmd2.ExecuteNonQuery();
    }

        public void DeleteStudent(int studentId)
    {
        using var cmd1 = connection.CreateCommand();
        cmd1.CommandText = "DELETE FROM CourseStudents WHERE StudentId = @id";
        cmd1.Parameters.AddWithValue("@id", studentId);
        cmd1.ExecuteNonQuery();

        using var cmd2 = connection.CreateCommand();
        cmd2.CommandText = "DELETE FROM Students WHERE Id = @id";
        cmd2.Parameters.AddWithValue("@id", studentId);
        cmd2.ExecuteNonQuery();
    }

    public void AddTeacherToCourse(int courseId, int teacherId)
    {
        using var cmd = connection.CreateCommand();
        cmd.CommandText = "INSERT INTO CourseTeachers (CourseId, TeacherId) VALUES (@courseId, @teacherId)";
        cmd.Parameters.AddWithValue("@courseId", courseId);
        cmd.Parameters.AddWithValue("@teacherId", teacherId);
        cmd.ExecuteNonQuery();
    }

    public void AddStudentToCourse(int courseId, int studentId)
    {
        using var cmd = connection.CreateCommand();
        cmd.CommandText = "INSERT INTO CourseStudents (CourseId, StudentId) VALUES (@courseId, @studentId)";
        cmd.Parameters.AddWithValue("@courseId", courseId);
        cmd.Parameters.AddWithValue("@studentId", studentId);
        cmd.ExecuteNonQuery();
    }

    public List<Student> GetCourseStudents(int courseId)
    {
        var students = new List<Student>();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = @"
            SELECT Students.* FROM Students
            JOIN CourseStudents ON Students.Id = CourseStudents.StudentId
            WHERE CourseStudents.CourseId = @courseId";
        cmd.Parameters.AddWithValue("@courseId", courseId);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            students.Add(new Student
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Email = reader.GetString(2),
                Number = reader.GetString(3)
            });
        }
        return students;
    }

    public List<Teacher> GetCourseTeachers(int courseId)
    {
        var teachers = new List<Teacher>();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = @"
            SELECT Teachers.* FROM Teachers
            JOIN CourseTeachers ON Teachers.Id = CourseTeachers.TeacherId
            WHERE CourseTeachers.CourseId = @courseId";
        cmd.Parameters.AddWithValue("@courseId", courseId);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            teachers.Add(new Teacher
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Email = reader.GetString(2),
                Number = reader.GetString(3)
            });
        }
        return teachers;
    }

    public List<Course> GetAllCourses()
    {
        var courses = new List<Course>();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT * FROM Courses";

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            courses.Add(new Course
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Description = reader.GetString(2),
                CourseType = reader.GetString(3)
            });
        }
        return courses;
    }

    public List<Course> GetTeacherCourses(int teacherId)
    {
        var courses = new List<Course>();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = @"
            SELECT Courses.* FROM Courses
            JOIN CourseTeachers ON Courses.Id = CourseTeachers.CourseId
            WHERE CourseTeachers.TeacherId = @teacherId";
        cmd.Parameters.AddWithValue("@teacherId", teacherId);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            courses.Add(new Course
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Description = reader.GetString(2),
                CourseType = reader.GetString(3)
            });
        }
        return courses;
    }

    public List<Teacher> GetAllTeachers()
    {
        var teachers = new List<Teacher>();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT * FROM Teachers";

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            teachers.Add(new Teacher
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Email = reader.GetString(2),
                Number = reader.GetString(3)
            });
        }
        return teachers;
    }

    public List<Student> GetAllStudents()
    {
        var students = new List<Student>();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT * FROM Students";

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            students.Add(new Student
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Email = reader.GetString(2),
                Number = reader.GetString(3)
            });
        }
        return students;
    }
}