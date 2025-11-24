// DTOModelsTests.cs
using Xunit;

namespace itmo_lab2.Tests
{
    public class DTOModelsTests
    {
        [Fact]
        public void Course_ShouldSetPropertiesCorrectly()
        {
            // Arrange & Act
            var course = new Course
            {
                Id = 1,
                Title = "Computer Science",
                Description = "Programming fundamentals",
                CourseType = "Technical"
            };

            // Assert
            Assert.Equal(1, course.Id);
            Assert.Equal("Computer Science", course.Title);
            Assert.Equal("Programming fundamentals", course.Description);
            Assert.Equal("Technical", course.CourseType);
        }

        [Fact]
        public void Teacher_ShouldSetPropertiesCorrectly()
        {
            // Arrange & Act
            var teacher = new Teacher
            {
                Id = 1,
                Name = "Professor Johnson",
                Email = "johnson@university.edu",
                Number = "1234567890"
            };

            // Assert
            Assert.Equal(1, teacher.Id);
            Assert.Equal("Professor Johnson", teacher.Name);
            Assert.Equal("johnson@university.edu", teacher.Email);
            Assert.Equal("1234567890", teacher.Number);
        }

        [Fact]
        public void Student_ShouldSetPropertiesCorrectly()
        {
            // Arrange & Act
            var student = new Student
            {
                Id = 1,
                Name = "Alice Brown",
                Email = "alice.brown@student.edu",
                Number = "0987654321"
            };

            // Assert
            Assert.Equal(1, student.Id);
            Assert.Equal("Alice Brown", student.Name);
            Assert.Equal("alice.brown@student.edu", student.Email);
            Assert.Equal("0987654321", student.Number);
        }

        [Fact]
        public void CourseStudent_ShouldEstablishRelationship()
        {
            // Arrange & Act
            var courseStudent = new CourseStudent
            {
                CourseId = 5,
                StudentId = 10
            };

            // Assert
            Assert.Equal(5, courseStudent.CourseId);
            Assert.Equal(10, courseStudent.StudentId);
        }

        [Fact]
        public void CourseTeacher_ShouldEstablishRelationship()
        {
            // Arrange & Act
            var courseTeacher = new CourseTeacher
            {
                CourseId = 3,
                TeacherId = 7
            };

            // Assert
            Assert.Equal(3, courseTeacher.CourseId);
            Assert.Equal(7, courseTeacher.TeacherId);
        }

        [Fact]
        public void Course_DefaultValues_ShouldBeNull()
        {
            // Arrange & Act
            var course = new Course();

            // Assert
            Assert.Equal(0, course.Id);
            Assert.Null(course.Title);
            Assert.Null(course.Description);
            Assert.Null(course.CourseType);
        }

        [Fact]
        public void Teacher_DefaultValues_ShouldBeNull()
        {
            // Arrange & Act
            var teacher = new Teacher();

            // Assert
            Assert.Equal(0, teacher.Id);
            Assert.Null(teacher.Name);
            Assert.Null(teacher.Email);
            Assert.Null(teacher.Number);
        }

        [Fact]
        public void Student_DefaultValues_ShouldBeNull()
        {
            // Arrange & Act
            var student = new Student();

            // Assert
            Assert.Equal(0, student.Id);
            Assert.Null(student.Name);
            Assert.Null(student.Email);
            Assert.Null(student.Number);
        }
    }
}