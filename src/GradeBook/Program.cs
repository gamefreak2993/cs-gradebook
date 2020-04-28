using System;

namespace GradeBook
{
    class Program
    {
        static void Main(string[] args)
        {
            var book = CreateBookByUserInput.GetBook(args);
            var statistics = book.GetStatistics();

            if (!double.IsNaN(statistics.Average))
            {
                Console.WriteLine($"=== {book.Name} ===");
                Book.WriteStatistics(statistics);
            }
        }
    }
}
