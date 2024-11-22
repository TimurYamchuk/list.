using System;
using System.IO;
using System.Reflection;

class Program
{
    private static object groupInstance;
    private static Type groupType;
    private static Assembly studentAssembly;
    private static Type studentType;
    private static bool isRunning = true;

    static void Main()
    {
        try
        {
            InitializeAssemblies();

            while (isRunning)
            {
                ShowMainMenu();
                ProcessUserChoice();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    private static void InitializeAssemblies()
    {
        string basePath = AppDomain.CurrentDomain.BaseDirectory;
        string dllPath = Path.Combine(basePath, "DLL");

        LoadGroupAssembly(Path.Combine(dllPath, "AcademyGroupDLL.dll"));
        LoadStudentAssembly(Path.Combine(dllPath, "StudentDLL.dll"));
    }

    private static void LoadGroupAssembly(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException("Файл AcademyGroupDLL.dll не найден.");

        Assembly groupAssembly = Assembly.LoadFrom(path);
        groupType = groupAssembly.GetType("AcademyGroupDLL.AcademyGroup");

        if (groupType == null)
            throw new TypeLoadException("Класс AcademyGroup не найден в сборке.");

        groupInstance = Activator.CreateInstance(groupType);
    }

    private static void LoadStudentAssembly(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException("Файл StudentDLL.dll не найден.");

        studentAssembly = Assembly.LoadFrom(path);
        studentType = studentAssembly.GetType("StudentDLL.Student");

        if (studentType == null)
            throw new TypeLoadException("Класс Student не найден в сборке.");
    }

    private static void ShowMainMenu()
    {
        Console.WriteLine("\nГлавное меню:");
        Console.WriteLine("1. Добавить студента");
        Console.WriteLine("2. Удалить студента");
        Console.WriteLine("3. Редактировать студента");
        Console.WriteLine("4. Просмотреть список студентов");
        Console.WriteLine("5. Сохранить данные группы");
        Console.WriteLine("6. Загрузить данные группы");
        Console.WriteLine("7. Поиск студента");
        Console.WriteLine("8. Сортировать студентов");
        Console.WriteLine("0. Выйти");
        Console.Write("Выберите действие: ");
    }

    private static void ProcessUserChoice()
    {
        string choice = Console.ReadLine();
        switch (choice)
        {
            case "1": AddStudent(); break;
            case "2": RemoveStudent(); break;
            case "3": EditStudent(); break;
            case "4": InvokeGroupMethod("PrintGroup"); break;
            case "5": InvokeGroupMethod("SaveGroupData"); break;
            case "6": InvokeGroupMethod("LoadGroupData"); break;
            case "7": SearchStudent(); break;
            case "8": SortStudents(); break;
            case "0": isRunning = false; break;
            default: Console.WriteLine("Ошибка: неверный ввод."); break;
        }
    }

    private static void AddStudent()
    {
        Console.WriteLine("\nДобавление нового студента:");

        var parameters = GetStudentInput();

        try
        {
            var student = Activator.CreateInstance(studentType, parameters);
            InvokeGroupMethod("AddStudent", student);
            Console.WriteLine("Студент успешно добавлен.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при добавлении студента: {ex.Message}");
        }
    }

    private static void RemoveStudent()
    {
        Console.Write("\nВведите фамилию студента для удаления: ");
        string surname = Console.ReadLine();

        try
        {
            InvokeGroupMethod("RemoveStudent", surname);
            Console.WriteLine("Студент успешно удалён.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при удалении студента: {ex.Message}");
        }
    }

    private static void EditStudent()
    {
        Console.Write("\nВведите фамилию студента для редактирования: ");
        string targetSurname = Console.ReadLine();

        Console.WriteLine("Введите новые данные студента:");
        var updatedParameters = GetStudentInput();

        try
        {
            var updatedStudent = Activator.CreateInstance(studentType, updatedParameters);
            InvokeGroupMethod("EditStudent", targetSurname, updatedStudent);
            Console.WriteLine("Данные студента успешно обновлены.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при редактировании студента: {ex.Message}");
        }
    }

    private static void SearchStudent()
    {
        Console.WriteLine("\nПоиск студента:");
        Console.WriteLine("1. По имени");
        Console.WriteLine("2. По фамилии");
        Console.Write("Введите критерий поиска: ");

        if (int.TryParse(Console.ReadLine(), out int criterion))
        {
            Console.Write("Введите значение для поиска: ");
            string searchValue = Console.ReadLine();

            try
            {
                InvokeGroupMethod("SearchStudent", criterion, searchValue);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при поиске студента: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Ошибка: ввод должен быть числом.");
        }
    }

    private static void SortStudents()
    {
        Console.WriteLine("\nСортировка студентов:");
        Console.WriteLine("1. По имени");
        Console.WriteLine("2. По возрасту");
        Console.Write("Выберите тип сортировки: ");

        if (int.TryParse(Console.ReadLine(), out int sortType))
        {
            try
            {
                InvokeGroupMethod("SortStudents", sortType);
                Console.WriteLine("Сортировка выполнена.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сортировке студентов: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Ошибка: ввод должен быть числом.");
        }
    }

    private static object[] GetStudentInput()
    {
        Console.Write("Имя: ");
        string name = Console.ReadLine();
        Console.Write("Фамилия: ");
        string surname = Console.ReadLine();
        Console.Write("Возраст: ");
        int age = int.Parse(Console.ReadLine());
        Console.Write("Телефон: ");
        string phone = Console.ReadLine();
        Console.Write("Средний балл: ");
        double average = double.Parse(Console.ReadLine());
        Console.Write("Номер группы: ");
        int groupNumber = int.Parse(Console.ReadLine());

        return new object[] { name, surname, age, phone, average, groupNumber };
    }

    private static void InvokeGroupMethod(string methodName, params object[] parameters)
    {
        MethodInfo method = groupType.GetMethod(methodName);
        if (method == null)
            throw new MissingMethodException($"Метод {methodName} не найден в классе.");

        method.Invoke(groupInstance, parameters);
    }
}
