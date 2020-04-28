using System;
using System.Collections.Generic;
using System.IO;

namespace GradeBook
{
    public delegate void GradeAddedDelegate(object sender, EventArgs args);

    public interface IBook
    {
        void AddGrade(double grade);
        Statistics GetStatistics();
        string Name { get; }
        List<double> Grades { get; set; }
        event GradeAddedDelegate GradeAdded;
    }

    public class NamedObject
    {
        public string Name { get; set; }

        public NamedObject(string name)
        {
            Name = name;
        }
    }

    public abstract class Book : NamedObject, IBook
    {
        public Book(string name) : base(name)
        {
            if (name == String.Empty)
            {
                Name = "Unnamed";
            }
            else
            {
                Name = name;
            }
        }

        public List<double> Grades { get => this.Grades; set => this.Grades = value; }

        public abstract event GradeAddedDelegate GradeAdded;

        public abstract void AddGrade(double grade);

        public abstract Statistics GetStatistics();
        static public void WriteStatistics(Statistics statistics)
        {
            Console.WriteLine($"The lowest grade is {statistics.Low:N1}");
            Console.WriteLine($"The highest grade is {statistics.High:N1}");
            Console.WriteLine($"The average grade is {statistics.Average:N1}");
            Console.WriteLine($"The letter grade is {statistics.Letter}");
        }
    }

    public class DiskBook : Book
    {
        public DiskBook(string name) : base(name) { }

        public override event GradeAddedDelegate GradeAdded;

        public override void AddGrade(double grade)
        {
            using (var writer = File.AppendText($"{Name.ToLower().Replace(' ', '_')}.gradebook.txt"))
            {
                writer.WriteLine(grade);

                if (GradeAdded != null)
                {
                    GradeAdded(this, new EventArgs());
                }
            }
        }

        public override Statistics GetStatistics()
        {
            var result = new Statistics();

            using (var reader = File.OpenText($"{Name.ToLower().Replace(' ', '-')}.gradebook.txt"))
            {
                var line = reader.ReadLine();

                while (line != null)
                {
                    var number = double.Parse(line);

                    result.Add(number);

                    line = reader.ReadLine();
                }
            }

            return result;
        }
    }

    public class InMemoryBook : Book
    {
        public InMemoryBook(string name) : base(name)
        {
            Grades = new List<double>();
        }

        public void AddGrade(char letter)
        {
            switch (letter)
            {
                case 'A':
                    AddGrade(90);
                    break;

                case 'B':
                    AddGrade(80);
                    break;

                case 'C':
                    AddGrade(70);
                    break;

                default:
                    AddGrade(0);
                    break;
            }
        }

        public override void AddGrade(double grade)
        {
            if (grade <= 100 && grade >= 0)
            {
                Grades.Add(grade);

                if (GradeAdded != null)
                {
                    GradeAdded(this, new EventArgs());
                }
            }
            else
            {
                throw new ArgumentException($"Invalid {nameof(grade)}!");
            }
        }

        public override event GradeAddedDelegate GradeAdded;

        public override Statistics GetStatistics()
        {
            var result = new Statistics();

            for (var index = 0; index < Grades.Count; index += 1)
            {
                result.Add(Grades[index]);
            }

            return result;
        }
    }
}