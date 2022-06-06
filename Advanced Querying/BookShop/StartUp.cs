namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);
            // var command = Console.ReadLine();
            //Console.WriteLine(GetBooksByAgeRestriction(db, command)); //Task 01
            //Console.WriteLine(GetGoldenBooks(db));   //Task 02
            //Console.WriteLine(GetBooksByPrice(db));  //Task 03
            //Console.WriteLine(GetBooksNotReleasedIn(db, int.Parse(command)));    //Task 04
            //Console.WriteLine(GetBooksByCategory(db, command));    //Task 05
            //Console.WriteLine(GetBooksReleasedBefore(db, command));    //Task 06
            //Console.WriteLine(GetAuthorNamesEndingIn(db, command));    //Task 07
            //Console.WriteLine(GetBookTitlesContaining(db, command));    //Task 08
            //Console.WriteLine(GetBooksByAuthor(db, command));    //Task 09
            //Console.WriteLine(CountBooks(db, int.Parse(command)));    //Task 10
            //Console.WriteLine(CountCopiesByAuthor(db));    //Task 11
            //Console.WriteLine(GetTotalProfitByCategory(db));    //Task 12
            //Console.WriteLine(GetMostRecentBooks(db));    //Task 13
            //IncreasePrices(db);    //Task 14
            Console.WriteLine(RemoveBooks(db));    //Task 15
        }
        public static int RemoveBooks(BookShopContext context)
        {
            var books = context.Books
              .Where(x => x.Copies < 4200)
              .ToList();

            context.Books.RemoveRange(books);

            context.SaveChanges();

            return books.Count();
        }
        public static void IncreasePrices(BookShopContext context)
        {

            var books = context.Books
                .Where(x => x.ReleaseDate.Value.Year < 2010)
                .ToList();

            foreach (var book in books)
            {
                book.Price += 5;
            }

            context.SaveChanges();
        }
        public static string GetMostRecentBooks(BookShopContext context)
        {
            var sb = new StringBuilder();

            var categories = context.Categories
                .Select(x => new
                {
                    Name = x.Name,
                    Books = x.CategoryBooks
                    .Select(x => new
                    {
                        ReleaseDate = x.Book.ReleaseDate,
                        BookTitle = x.Book.Title
                    })
                    .OrderByDescending(x => x.ReleaseDate)
                    .Take(3)
                    .ToList()
                })
                .OrderBy(x => x.Name)
                .ToList();

            foreach (var category in categories)
            {
                sb.AppendLine($"--{category.Name}");
                foreach (var book in category.Books)
                {
                    sb.AppendLine($"{book.BookTitle} ({book.ReleaseDate.Value.Year})");
                }
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var categories = context.Categories
                .Select(x => new
                {
                    Name = x.Name,
                    Profit = x.CategoryBooks.Sum(c => c.Book.Price * c.Book.Copies)
                })
                .OrderByDescending(x => x.Profit)
                .ToList();

            return String.Join(Environment.NewLine, categories.Select(x => $"{x.Name} ${x.Profit:f2}"));
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authors = context.Authors
                .Select(x => new
                {
                    FullName = x.FirstName + ' ' + x.LastName,
                    Copies = x.Books.Sum(x => x.Copies)
                })
                .OrderByDescending(x => x.Copies);

            return String.Join(Environment.NewLine, authors.Select(x => $"{x.FullName} - {x.Copies}"));
        }
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var books = context.Books
                .Count(x => x.Title.Length > lengthCheck);

            return books;
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var books = context.Books
               .Include(x => x.Author)
               .Where(x => x.Author.LastName.ToLower().StartsWith(input.ToLower()))
               .Select(x => new
               {
                   Title = x.Title,
                   Author = x.Author.FirstName + ' ' + x.Author.LastName,
                   Id = x.BookId
               })
               .OrderBy(x => x.Id)
               .ToList();

            return String.Join(Environment.NewLine, books.Select(x => $"{x.Title} ({x.Author})"));
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(x => x.Title.ToLower().Contains(input.ToLower()))
                .Select(x => x.Title)
                .OrderBy(x => x)
                .ToList();

            return String.Join(Environment.NewLine, books);
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context.Authors
                .Where(x => x.FirstName.EndsWith(input))
                .Select(x => new
                {
                    FullName = x.FirstName + ' ' + x.LastName
                })
                .OrderBy(x => x.FullName)
                .ToList();

            return String.Join(Environment.NewLine, authors.Select(x => x.FullName));
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var dargetDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var books = context.Books
                .Where(x => x.ReleaseDate.Value < dargetDate)
                .Select(x => new
                {
                    Title = x.Title,
                    Price = x.Price,
                    Edition = x.EditionType,
                    ReleaseDate = x.ReleaseDate.Value
                })
                .OrderByDescending(x => x.ReleaseDate)
                .ToList();

            return String.Join(Environment.NewLine, books.Select(x => $"{x.Title} - {x.Edition} - ${x.Price:f2}"));
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var categories = input.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.ToLower())
                .ToList();

            var books = context.Books
                .Where(x => x.BookCategories.Any(category => categories.Contains(category.Category.Name.ToLower())))
                .Select(x => x.Title)
                .OrderBy(x => x)
                .ToList();

            return String.Join(Environment.NewLine, books);
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context.Books
                .Where(x => x.ReleaseDate.Value.Year != year)
                .Select(x => new
                {
                    Id = x.BookId,
                    Title = x.Title
                })
                .OrderBy(x => x.Id)
                .ToList();

            return String.Join(Environment.NewLine, books.Select(x => x.Title));
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            var sb = new StringBuilder();

            var books = context.Books
                .Where(x => x.Price > 40)
                .Select(x => new
                {
                    Price = x.Price,
                    Title = x.Title
                })
                .OrderByDescending(x => x.Price)
                .ToList();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(x => x.Copies < 5000 && x.EditionType == EditionType.Gold)
                .Select(x => new
                {
                    Id = x.BookId,
                    Title = x.Title
                })
                .OrderBy(x => x.Id)
                .ToList();

            return String.Join(Environment.NewLine, books.Select(x => x.Title));
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var ageRestriction = Enum.Parse<AgeRestriction>(command, true);

            var books = context.Books
                .Where(x => x.AgeRestriction == ageRestriction)
                .Select(x => x.Title)
                .OrderBy(x => x)
                .ToArray();

            return String.Join(Environment.NewLine, books);
        }
    }
}
