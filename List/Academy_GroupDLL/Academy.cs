using System;
using System.Collections.Generic;
using System.IO;
using StudentLibrary;

namespace AcademyGroupLibrary
{
    public class AcademyGroup
    {
        private readonly List<Student> _students;

        public AcademyGroup()
        {
            _students = new List<Student>();
        }

        public void AddStudent(Student student)
        {
            _students.Add(student);
            Console.WriteLine($"Студент {student.Name} {student.Surname} успешно добавлен.");
        }

        public void RemoveStudent(string surname)
        {
            var student = _students.Find(s => s.Surname.Equals(surname, StringComparison.OrdinalIgnoreCase));
            if (student != null)
            {
                _students.Remove(student);
                Console.WriteLine($"Студент с фамилией {surname} удалён.");
            }
            else
            {
                Console.WriteLine($"Студент с фамилией {surname} не найден.");
            }
        }

        public void EditStudent(string surname, Student updatedStudent)
        {
            var index = _students.FindIndex(s => s.Surname.Equals(surname, StringComparison.OrdinalIgnoreCase));
            if (index != -1)
            {
                _students[index] = updatedStudent;
                Console.WriteLine($"Информация о студенте {surname} обновлена.");
            }
            else
            {
                Console.WriteLine($"Студент с фамилией {surname} не найден.");
            }
        }

        public void PrintAll()
        {
            if (_students.Count == 0)
            {
                Console.WriteLine("Группа пуста.");
                return;
            }

            Console.WriteLine("Список студентов:");
            foreach (var student in _students)
            {
                student.Print();
            }
        }

        public void SortStudents(int criterion)
        {
            switch (criterion)
            {
                case 1:
                    _students.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase));
                    Console.WriteLine("Сортировка по имени завершена.");
                    break;
                case 2:
                    _students.Sort((a, b) => string.Compare(a.Surname, b.Surname, StringComparison.OrdinalIgnoreCase));
                    Console.WriteLine("Сортировка по фамилии завершена.");
                    break;
                case 3:
                    _students.Sort((a, b) => a.Age.CompareTo(b.Age));
                    Console.WriteLine("Сортировка по возрасту завершена.");
                    break;
                default:
                    Console.WriteLine("Некорректный критерий сортировки.");
                    return;
            }
            PrintAll();
        }

        public void SearchStudent(int criterion, string value)
        {
            IEnumerable<Student> result = criterion switch
            {
                1 => _students.FindAll(s => s.Name.Equals(value.Trim(), StringComparison.OrdinalIgnoreCase)),
                2 => _students.FindAll(s => s.Surname.Equals(value.Trim(), StringComparison.OrdinalIgnoreCase)),
                3 => _students.FindAll(s => s.Phone.Equals(value.Trim(), StringComparison.OrdinalIgnoreCase)),
                4 => _students.FindAll(s => s.Age.ToString().Equals(value.Trim())),
                5 => _students.FindAll(s => s.Average.ToString().Equals(value.Trim())),
                6 => _students.FindAll(s => s.GroupNumber.ToString().Equals(value.Trim())),
                _ => null
            };

            if (result == null || !result.Any())
            {
                Console.WriteLine("Студенты по заданному критерию не найдены.");
                return;
            }

            Console.WriteLine("Результаты поиска:");
            foreach (var student in result)
            {
                student.Print();
            }
        }

        public void SaveToFile(string fileName = "GroupData.txt")
        {
            try
            {
                using (var writer = new StreamWriter(fileName, false))
                {
                    foreach (var student in _students)
                    {
                        writer.WriteLine(student.ToString());
                    }
                }
                Console.WriteLine("Данные успешно сохранены.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении: {ex.Message}");
            }
        }

        public void LoadFromFile(string fileName = "GroupData.txt")
        {
            if (!File.Exists(fileName))
            {
                Console.WriteLine("Файл данных не найден.");
                return;
            }

            try
            {
                _students.Clear();
                using (var reader = new StreamReader(fileName))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var studentData = line.Split(';');
                        if (studentData.Length == 6 &&
                            int.TryParse(studentData[2], out int age) &&
                            double.TryParse(studentData[4], out double average) &&
                            int.TryParse(studentData[5], out int groupNumber))
                        {
                            _students.Add(new Student(studentData[0], studentData[1], age, studentData[3], average, groupNumber));
                        }
                    }
                }
                Console.WriteLine("Данные успешно загружены.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке данных: {ex.Message}");
            }
        }
    }
}
