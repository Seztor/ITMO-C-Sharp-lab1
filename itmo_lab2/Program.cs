namespace itmo_lab2;

class Program
{
    private static DatabaseManager db = DatabaseManager.GetInstance();

    static void Main()
    {
        while (true)
        {
            ShowMainMenu();
            var choice = Console.ReadLine();

            if (choice == "1")
            {
                ManageCourses();
            }
            else if (choice == "2")
            {
                ManageTeachers();
            }
            else if (choice == "3")
            {
                ManageStudents();
            }
            else if (choice == "0")
            {
                Console.WriteLine("Выход");
                return;
            }
        }
    }

    static void ShowMainMenu()
    {
        Console.Clear();
        Console.WriteLine("Главное меню:");
        Console.WriteLine("1. Управление курсами");
        Console.WriteLine("2. Управление преподавателями");
        Console.WriteLine("3. Управление студентами");
        Console.WriteLine("0. Выход");
        Console.Write("Выберите пункт меню: ");
    }

    static void ManageCourses()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Управление курсами:");
            Console.WriteLine("1. Добавить курс");
            Console.WriteLine("2. Удалить курс");
            Console.WriteLine("3. Показать все курсы");
            Console.WriteLine("4. Список студентов на курсе");
            Console.WriteLine("5. Список учителей ведущих курс");
            Console.WriteLine("0. Назад");
            Console.Write("Выберите пункт меню: ");

            var choice = Console.ReadLine();

            if (choice == "1")
            {
                AddCourse();
            }
            else if (choice == "2")
            {
                DeleteCourse();
            }
            else if (choice == "3")
            {
                ShowAllCourses();
            }
            else if (choice == "4")
            {
                ShowCourseStudents();
            }
            else if (choice == "5")
            {
                ShowCourseTeachers();
            }
            else if (choice == "0")
            {
                return;
            }

        }
    }

    static void ManageTeachers()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Управление преподавателями:");
            Console.WriteLine("1. Добавить преподавателя");
            Console.WriteLine("2. Найти курсы преподавателя");
            Console.WriteLine("3. Назначить преподавателя на курс");
            Console.WriteLine("0. Назад");
            Console.Write("Выберите пункт меню: ");

            var choice = Console.ReadLine();

            if (choice == "1")
            {
                AddTeacher();
            }
            else if (choice == "2")
            {
                ShowTeacherCourses();
            }
            else if (choice == "3")
            {
                AssignTeacherToCourse();
            }
            else if (choice == "0")
            {
                return;
            }


        }
    }

    static void ManageStudents()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Управление студентами:");
            Console.WriteLine("1. Добавить студента");
            Console.WriteLine("2. Добавить студента на курс");
            Console.WriteLine("0. Назад");
            Console.Write("Выберите пункт меню: ");

            var choice = Console.ReadLine();

            if (choice == "1")
            {
                AddStudent();
            }
            else if (choice == "2")
            {
                AssignStudentToCourse();
            }
            else if (choice == "0")
            {
                return;
            }

        }
    }

    static void AddCourse()
    {
        Console.WriteLine("\nДобавление курса...");
        Console.Write("Название курса: ");
        var title = Console.ReadLine();
        Console.Write("Описание курса: ");
        var description = Console.ReadLine();
        Console.Write("Тип курса: ");
        var courseType = Console.ReadLine();

        var course = new Course
        {
            Title = title,
            Description = description,
            CourseType = courseType
        };

        db.AddCourse(course);

        Console.WriteLine($"Курс {title} добавлен!");

        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();

    }

    static void AddTeacher()
    {
        Console.WriteLine("\nДобавление преподавателя...");

        Console.Write("Имя преподавателя: ");
        string name = Console.ReadLine();

        Console.Write("Email: ");
        string email = Console.ReadLine();

        Console.Write("Номер телефона: ");
        string number = Console.ReadLine();

        var teacher = new Teacher
        {
            Name = name,
            Email = email,
            Number = number
        };

        db.AddTeacher(teacher);
        Console.WriteLine($"Преподаватель {name} добавлен!");

        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }


    static void AddStudent()
    {
        Console.WriteLine("Добавление студента...");

        Console.Write("Имя студента: ");
        string name = Console.ReadLine();

        Console.Write("Email: ");
        string email = Console.ReadLine();

        Console.Write("Номер телефона: ");
        string number = Console.ReadLine();

        var student = new Student
        {
            Name = name,
            Email = email,
            Number = number
        };

        db.AddStudent(student);
        Console.WriteLine($"Студент {name} добавлен!");

        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }


    static int SelectCourse()
    {
        var courses = db.GetAllCourses();
        if (courses.Count == 0)
        {
            Console.WriteLine("Нет доступных курсов");
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
            return -1;
        }

        Console.WriteLine("\n Выберите курс:");
        for (int i = 0; i < courses.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {courses[i].Title}");
        }

        Console.Write("Введите порядковый номер курса: ");
        if (!int.TryParse(Console.ReadLine(), out int choice) ||
         choice < 1 ||
          choice > courses.Count)
        {
            Console.WriteLine("Неверный номер!");
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
            return -1;
        }

        return courses[choice - 1].Id;

    }
    static int SelectTeacher()
    {
        var teachers = db.GetAllTeachers();
        if (teachers.Count == 0)
        {
            Console.WriteLine("Нет доступных преподавателей");
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
            return -1;
        }

        Console.WriteLine("\nВыберите преподавателя:");
        for (int i = 0; i < teachers.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {teachers[i].Name} (ID: {teachers[i].Id})");
        }

        Console.Write("Введите порядковый номер преподавателя: ");
        if (!int.TryParse(Console.ReadLine(), out int choice) ||
         choice < 1 ||
          choice > teachers.Count)
        {
            Console.WriteLine("Неверный номер!");
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
            return -1;
        }

        return teachers[choice - 1].Id;
    }

    static int SelectStudent()
    {
        var students = db.GetAllStudents();
        if (students.Count == 0)
        {
            Console.WriteLine("Нет доступных студентов");
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
            return -1;
        }

        Console.WriteLine("\n Выберите студента:");
        for (int i = 0; i < students.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {students[i].Name}");
        }

        Console.Write("Введите порядковый номер студента: ");
        if (!int.TryParse(Console.ReadLine(), out int choice) ||
         choice < 1 ||
          choice > students.Count)
        {
            Console.WriteLine("Неверный номер!");
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
            return -1;
        }

        return students[choice - 1].Id;
    }

    static void DeleteCourse()
    {
        Console.WriteLine("\nУдаление курса...");

        int courseId = SelectCourse();
        if (courseId == -1) return;

        db.DeleteCourse(courseId);
        Console.WriteLine($"Курс с id {courseId} удален!");

        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }

    static void AssignTeacherToCourse()
    {
        Console.WriteLine("\nНазначение преподавателя на курс...");

        int courseId = SelectCourse();
        if (courseId == -1) return;

        int teacherId = SelectTeacher();
        if (teacherId == -1) return;

        db.AddTeacherToCourse(courseId, teacherId);
        Console.WriteLine("Преподаватель назначен на курс!");

        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }

    static void AssignStudentToCourse()
    {
        Console.WriteLine("\nЗапись студента на курс...");

        int courseId = SelectCourse();
        if (courseId == -1) return;

        int studentId = SelectStudent();
        if (studentId == -1) return;

        db.AddStudentToCourse(courseId, studentId);
        Console.WriteLine("Студент записан на курс!");

        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }

    static void ShowCourseStudents()
    {
        Console.WriteLine("\nСтуденты курса...");

        int courseId = SelectCourse();
        if (courseId == -1) return;

        var students = db.GetCourseStudents(courseId);

        if (students.Count == 0)
        {
            Console.WriteLine("На курсе нет студентов");
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("Студенты курса:");
        foreach (var student in students)
        {
            Console.WriteLine($"{student.Name}");
            Console.WriteLine($"Email: {student.Email}");
            Console.WriteLine($"Телефон: {student.Number}\n");
        }

        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }

    static void ShowCourseTeachers()
    {
        Console.WriteLine("\nПреподаватели курса...");

        int courseId = SelectCourse();
        if (courseId == -1) return;

        var teachers = db.GetCourseTeachers(courseId);

        if (teachers.Count == 0)
        {
            Console.WriteLine("На курсе нет преподавателей");
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("Преподаватели курса:");
        foreach (var teacher in teachers)
        {
            Console.WriteLine($"{teacher.Name}");
            Console.WriteLine($"Email: {teacher.Email}");
            Console.WriteLine($"Телефон: {teacher.Number}\n");
        }

        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }

    static void ShowTeacherCourses()
    {
        Console.WriteLine("\nКурсы преподавателя...");

        int teacherId = SelectTeacher();
        if (teacherId == -1) return;

        var courses = db.GetTeacherCourses(teacherId);

        if (courses.Count == 0)
        {
            Console.WriteLine("Преподаватель не ведет курсы");
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine($"Курсы преподавателя {teacherId}:");
        foreach (var course in courses)
        {
            Console.WriteLine($"{course.Title}");
            Console.WriteLine($"Тип: {course.CourseType}");
            Console.WriteLine($"Описание: {course.Description}\n");
        }

        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }

    static void ShowAllCourses()
    {
        Console.WriteLine("\nСписок курсов:");

        var courses = db.GetAllCourses();

        if (courses.Count == 0)
        {
            Console.WriteLine("Курсов нет");
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
            return;
        }

        foreach (var course in courses)
        {
            Console.WriteLine($"{course.Title} (ID: {course.Id})");
            Console.WriteLine($"Тип: {course.CourseType}");
            Console.WriteLine($"Описание: {course.Description}\n");
        }

        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }

}