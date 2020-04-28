using System;

namespace GradeBook
{
    class CreateBookByUserInput
    {
        static void OnGradeAdded(object sender, EventArgs e) {
            Console.WriteLine("A grade was added.");
        }
        static public IBook GetBook(string[] args)
        {
            IBook book = new DiskBook(GetBookName(args));
            book.GradeAdded += OnGradeAdded;

            return GetBookWithGrades(book);
        }

        static string GetBookName(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("What is the name of the gradebook? (Optional)");

                return Console.ReadLine();
            }
            else
            {
                return String.Empty;
            }
        }

        static IBook GetBookWithGrades(IBook book)
        {
            var input = String.Empty;
            double grade;

            while (true)
            {
                Console.WriteLine("What grade would you like to add? (Press \"Q\" to stop adding grades.)");

                input = Console.ReadLine();

                if (input.ToUpper() == "Q")
                {
                    Console.WriteLine("Quitting...");

                    break;
                }

                try
                {
                    grade = double.Parse(input);
                    book.AddGrade(grade);
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return book;
        }
    }
}