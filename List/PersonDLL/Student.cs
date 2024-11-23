namespace PersonLibrary
{
    public class Student
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int Age { get; set; }

        public Student() { }

        public Student(string name, string surname, string phone, int age)
        {
            Name = name;
            Surname = surname;
            Phone = phone;
            Age = age;
        }

        public virtual void Print()
        {
            Console.WriteLine("Информация о человеке:");
            Console.WriteLine($"Имя: {Name}");
            Console.WriteLine($"Фамилия: {Surname}");
            Console.WriteLine($"Телефон: {Phone}");
            Console.WriteLine($"Возраст: {Age}");
        }
    }
}
