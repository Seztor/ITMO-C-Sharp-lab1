// DatabaseManagerTests.cs
using Microsoft.Data.Sqlite;
using Xunit;

namespace itmo_lab2.Tests
{
    public class DatabaseManagerTests : IDisposable
    {
        private readonly DatabaseManager _db;

        public DatabaseManagerTests()
        {
            _db = DatabaseManager.GetInstance();
            ClearTestData();
        }

        public void Dispose()
        {
            ClearTestData();
        }

        private void ClearTestData()
        {
            // Очищаем все данные для изоляции тестов
            var courses = _db.GetAllCourses();
            foreach (var course in courses)
            {
                _db.DeleteCourse(course.Id);
            }

            var teachers = _db.GetAllTeachers();
            foreach (var teacher in teachers)
            {
                _db.DeleteTeacher(teacher.Id);
            }

            var students = _db.GetAllStudents();
            foreach (var student in students)
            {
                _db.DeleteStudent(student.Id);
            }
        }

        [Fact]
        public void AddCourse_ShouldAddCourseToDatabase()
        {
            // Arrange
            var course = new Course
            {
                Title = "Mathematics",
                Description = "Advanced mathematics course",
                CourseType = "Science"
            };

            // Act
            _db.AddCourse(course);
            var courses = _db.GetAllCourses();

            // Assert
            Assert.Single(courses);
            Assert.Equal("Mathematics", courses[0].Title);
            Assert.Equal("Advanced mathematics course", courses[0].Description);
            Assert.Equal("Science", courses[0].CourseType);
        }

        [Fact]
        public void AddTeacher_ShouldAddTeacherToDatabase()
        {
            // Arrange
            var teacher = new Teacher
            {
                Name = "Dr. Smith",
                Email = "smith@university.edu",
                Number = "1234567890"
            };

            // Act
            _db.AddTeacher(teacher);
            var teachers = _db.GetAllTeachers();

            // Assert
            Assert.Single(teachers);
            Assert.Equal("Dr. Smith", teachers[0].Name);
            Assert.Equal("smith@university.edu", teachers[0].Email);
            Assert.Equal("1234567890", teachers[0].Number);
        }

        [Fact]
        public void AddStudent_ShouldAddStudentToDatabase()
        {
            // Arrange
            var student = new Student
            {
                Name = "John Doe",
                Email = "john@student.edu",
                Number = "0987654321"
            };

            // Act
            _db.AddStudent(student);
            var students = _db.GetAllStudents();

            // Assert
            Assert.Single(students);
            Assert.Equal("John Doe", students[0].Name);
            Assert.Equal("john@student.edu", students[0].Email);
            Assert.Equal("0987654321", students[0].Number);
        }

        [Fact]
        public void DeleteCourse_ShouldRemoveCourseAndItsRelations()
        {
            // Arrange
            var course = new Course { Title = "Course to Delete", Description = "Description", CourseType = "Type" };
            _db.AddCourse(course);
            var courses = _db.GetAllCourses();
            var courseId = courses[0].Id;

            // Act
            _db.DeleteCourse(courseId);
            var coursesAfterDelete = _db.GetAllCourses();

            // Assert
            Assert.Empty(coursesAfterDelete);
        }

        [Fact]
        public void DeleteTeacher_ShouldRemoveTeacherFromDatabase()
        {
            // Arrange
            var teacher = new Teacher
            {
                Name = "Teacher to Delete",
                Email = "delete@example.com",
                Number = "1111111111"
            };
            _db.AddTeacher(teacher);
            var teachers = _db.GetAllTeachers();
            var teacherId = teachers[0].Id;

            // Act
            _db.DeleteTeacher(teacherId);
            var teachersAfterDelete = _db.GetAllTeachers();

            // Assert
            Assert.Empty(teachersAfterDelete);
        }

        [Fact]
        public void DeleteStudent_ShouldRemoveStudentFromDatabase()
        {
            // Arrange
            var student = new Student
            {
                Name = "Student to Delete",
                Email = "delete@example.com",
                Number = "2222222222"
            };
            _db.AddStudent(student);
            var students = _db.GetAllStudents();
            var studentId = students[0].Id;

            // Act
            _db.DeleteStudent(studentId);
            var studentsAfterDelete = _db.GetAllStudents();

            // Assert
            Assert.Empty(studentsAfterDelete);
        }

        [Fact]
        public void AddTeacherToCourse_ShouldCreateRelationship()
        {
            // Arrange
            var course = new Course { Title = "Physics", Description = "Physics course", CourseType = "Science" };
            var teacher = new Teacher { Name = "Prof. Physics", Email = "physics@example.com", Number = "3333333333" };
            
            _db.AddCourse(course);
            _db.AddTeacher(teacher);
            
            var courses = _db.GetAllCourses();
            var teachers = _db.GetAllTeachers();
            var courseId = courses[0].Id;
            var teacherId = teachers[0].Id;

            // Act
            _db.AddTeacherToCourse(courseId, teacherId);
            var courseTeachers = _db.GetCourseTeachers(courseId);

            // Assert
            Assert.Single(courseTeachers);
            Assert.Equal("Prof. Physics", courseTeachers[0].Name);
        }

        [Fact]
        public void AddStudentToCourse_ShouldCreateRelationship()
        {
            // Arrange
            var course = new Course { Title = "Chemistry", Description = "Chemistry course", CourseType = "Science" };
            var student = new Student { Name = "Chemistry Student", Email = "chem@example.com", Number = "4444444444" };
            
            _db.AddCourse(course);
            _db.AddStudent(student);
            
            var courses = _db.GetAllCourses();
            var students = _db.GetAllStudents();
            var courseId = courses[0].Id;
            var studentId = students[0].Id;

            // Act
            _db.AddStudentToCourse(courseId, studentId);
            var courseStudents = _db.GetCourseStudents(courseId);

            // Assert
            Assert.Single(courseStudents);
            Assert.Equal("Chemistry Student", courseStudents[0].Name);
        }

        [Fact]
        public void GetTeacherCourses_ShouldReturnCorrectCourses()
        {
            // Arrange
            var teacher = new Teacher { Name = "Multi-Course Teacher", Email = "multi@example.com", Number = "5555555555" };
            var course1 = new Course { Title = "Course A", Description = "Description A", CourseType = "Type A" };
            var course2 = new Course { Title = "Course B", Description = "Description B", CourseType = "Type B" };
            
            _db.AddTeacher(teacher);
            _db.AddCourse(course1);
            _db.AddCourse(course2);
            
            var teachers = _db.GetAllTeachers();
            var courses = _db.GetAllCourses();
            var teacherId = teachers[0].Id;

            _db.AddTeacherToCourse(courses[0].Id, teacherId);
            _db.AddTeacherToCourse(courses[1].Id, teacherId);

            // Act
            var teacherCourses = _db.GetTeacherCourses(teacherId);

            // Assert
            Assert.Equal(2, teacherCourses.Count);
            Assert.Contains(teacherCourses, c => c.Title == "Course A");
            Assert.Contains(teacherCourses, c => c.Title == "Course B");
        }

        [Fact]
        public void DeleteTeacher_ShouldRemoveTeacherFromCourses()
        {
            // Arrange
            var teacher = new Teacher { Name = "Teacher", Email = "teacher@test.com", Number = "6666666666" };
            var course = new Course { Title = "Test Course", Description = "Test Description", CourseType = "Test Type" };
            
            _db.AddTeacher(teacher);
            _db.AddCourse(course);
            
            var teachers = _db.GetAllTeachers();
            var courses = _db.GetAllCourses();
            var teacherId = teachers[0].Id;
            var courseId = courses[0].Id;

            _db.AddTeacherToCourse(courseId, teacherId);

            // Act
            _db.DeleteTeacher(teacherId);
            var courseTeachers = _db.GetCourseTeachers(courseId);

            // Assert
            Assert.Empty(courseTeachers);
        }

        [Fact]
        public void DeleteStudent_ShouldRemoveStudentFromCourses()
        {
            // Arrange
            var student = new Student { Name = "Student", Email = "student@test.com", Number = "7777777777" };
            var course = new Course { Title = "Test Course", Description = "Test Description", CourseType = "Test Type" };
            
            _db.AddStudent(student);
            _db.AddCourse(course);
            
            var students = _db.GetAllStudents();
            var courses = _db.GetAllCourses();
            var studentId = students[0].Id;
            var courseId = courses[0].Id;

            _db.AddStudentToCourse(courseId, studentId);

            // Act
            _db.DeleteStudent(studentId);
            var courseStudents = _db.GetCourseStudents(courseId);

            // Assert
            Assert.Empty(courseStudents);
        }

        [Fact]
        public void GetAllCourses_ShouldReturnEmptyListWhenNoCourses()
        {
            // Act
            var courses = _db.GetAllCourses();

            // Assert
            Assert.Empty(courses);
        }

        [Fact]
        public void GetAllTeachers_ShouldReturnEmptyListWhenNoTeachers()
        {
            // Act
            var teachers = _db.GetAllTeachers();

            // Assert
            Assert.Empty(teachers);
        }

        [Fact]
        public void GetAllStudents_ShouldReturnEmptyListWhenNoStudents()
        {
            // Act
            var students = _db.GetAllStudents();

            // Assert
            Assert.Empty(students);
        }

        [Fact]
        public void DatabaseManager_ShouldBeSingleton()
        {
            // Arrange & Act
            var instance1 = DatabaseManager.GetInstance();
            var instance2 = DatabaseManager.GetInstance();

            // Assert
            Assert.Same(instance1, instance2);
        }
    }
}