namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    //using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);

            //string ageRestrictionString = Console.ReadLine();
            //int notReleasedYear = int.Parse(Console.ReadLine());
            //string categories = Console.ReadLine();
            //string dateBefore = Console.ReadLine();
            //string nameEndsWith = Console.ReadLine();
            //string titleContains = Console.ReadLine();
            //string authorsNameStartsWith = Console.ReadLine();
            //int longerThan = int.Parse(Console.ReadLine());

            //string result = GetMostRecentBooks(db);
            int result = RemoveBooks(db);

            Console.WriteLine(result);
        }

        //Problem 1
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            StringBuilder sb = new StringBuilder();

            AgeRestriction ageRestriction = Enum.Parse<AgeRestriction>(command, true);

            string[] bookTitles = context
                .Books
                .Where(b => b.AgeRestriction == ageRestriction)
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToArray();

            foreach (var title in bookTitles)
            {
                sb.AppendLine(title);
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 2
        public static string GetGoldenBooks(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            string[] goldenBooksTitles = context.Books
                .Where(b => b.EditionType == EditionType.Gold &&
                            b.Copies < 5000)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            foreach (var title in goldenBooksTitles)
            {
                sb.AppendLine(title);
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 3
        public static string GetBooksByPrice(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var titlesAndPrices = context.Books
                .Where(b => b.Price > 40)
                .OrderByDescending(b => b.Price)
                .Select(b => new
                {
                    TitleBook = b.Title,
                    PriceBook = b.Price
                })
                .ToArray();

            foreach (var kvp in titlesAndPrices)
            {
                sb.AppendLine($"{kvp.TitleBook} - ${kvp.PriceBook:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 4
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            StringBuilder sb = new StringBuilder();

            var notReleased = context.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            foreach (var title in notReleased)
            {
                sb.AppendLine(title);
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 5 !!!
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder(); 

            List<string> categories = input.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            List<string> booksTitle = new List<string>();

            foreach (var category in categories)
            {
                var currCategory = context.Categories
                    .Where(c => c.Name.ToLower() == category.ToLower())
                    .SelectMany(c => c.CategoryBooks
                        .Select(cb => cb.Book))
                    .Select(c => c.Title)
                    .ToArray();

                booksTitle.AddRange(currCategory);
            }

            foreach (var title in booksTitle.OrderBy(t => t))
            {
                sb.AppendLine(title);
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 6
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            StringBuilder sb = new StringBuilder();

            DateTime dateTime = DateTime.ParseExact(date, "dd-MM-yyyy", null);

            var releasedBooksBefore = context.Books
                .Where(b => b.ReleaseDate < dateTime)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => new
                {
                    Title = b.Title,
                    EditionType = b.EditionType,
                    Price = b.Price
                })
                .ToArray();

            foreach (var book in releasedBooksBefore)
            {
                sb.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 7
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();

            var autorsEndingwith = context.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .Select(a => a.FirstName + " " + a.LastName)
                .ToArray();

            foreach (var names in autorsEndingwith.OrderBy(n => n))
            {
                sb.AppendLine(names);
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 8
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();

            var titleContains = context.Books
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToArray();

            foreach (var title in titleContains)
            {
                sb.AppendLine(title);
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 9 !!!!!
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();

            var booksAndAuthors = context.Authors
                .Where(a => a.LastName.ToLower().StartsWith(input.ToLower()))
                .Select(a => new
                {
                    AuthorFullName = a.FirstName + " " + a.LastName,
                    BooksTitle = a.Books
                        .OrderBy(b => b.BookId)
                        .Select(b => b.Title)
                })
                .ToArray();

            foreach (var autors in booksAndAuthors)
            {
                foreach (var title in autors.BooksTitle)
                {
                    sb.AppendLine($"{title} ({autors.AuthorFullName})");
                }
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 10
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var booksCount = context.Books
                .Where(b => b.Title.Length > lengthCheck)
                .ToList()
                .Count;

            return booksCount;
        }

        //Problem 11
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var countOfCopies = context.Authors
                .OrderByDescending(a => a.Books.Sum(b => b.Copies))
                .Select(a => new
                {
                    AuthorsFullName = a.FirstName + " " + a.LastName,
                    CountOfCopies = a.Books.Sum(b => b.Copies)
                })
                .ToArray();

            foreach (var author in countOfCopies)
            {
                sb.AppendLine($"{author.AuthorsFullName} - {author.CountOfCopies}");
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 12
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var profitByCategory = context.Categories
                .OrderByDescending(c => c.CategoryBooks.Sum(b => b.Book.Price * b.Book.Copies))
                .Select(c => new 
                { 
                    Category = c.Name,
                    TotalPrice = c.CategoryBooks.Sum(b => b.Book.Price * b.Book.Copies)
                })
                .ToArray();

            foreach (var category in profitByCategory)
            {
                sb.AppendLine($"{category.Category} ${category.TotalPrice:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 13 !!!!
        public static string GetMostRecentBooks(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var mostRecentBooks = context.Categories
                .OrderBy(c => c.Name)
                .Select(c => new
                {
                    CategoryName = c.Name,
                    Books = c.CategoryBooks
                        .OrderByDescending(b => b.Book.ReleaseDate)
                        .Take(3)
                        .Select(b => new
                        {
                            Title = b.Book.Title,
                            ReleaseDate = b.Book.ReleaseDate.Value.Year
                        })
                })
                .ToArray();

            foreach (var category in mostRecentBooks)
            {
                sb.AppendLine($"--{category.CategoryName}");

                foreach (var book in category.Books)
                {
                    sb.AppendLine($"{book.Title} ({book.ReleaseDate})");
                }
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 14
        public static void IncreasePrices(BookShopContext context)
        {
            var priceToIncrease = context.Books
                .Where(b => b.ReleaseDate.Value.Year < 2010)
                .ToArray();

            foreach (var book in priceToIncrease)
            {
                book.Price += 5;
            }

            context.SaveChanges();
        }

        //Problem 15 !!!!
        public static int RemoveBooks(BookShopContext context)
        {
            var categoryRemovedBooks = context.BooksCategories
                .Where(cb => cb.Book.Copies < 4200)
                .ToArray();

            context.BooksCategories.RemoveRange(categoryRemovedBooks);

            context.SaveChanges();

            var removedBooks = context.Books
                .Where(b => b.Copies < 4200)
                .ToArray();

            context.Books.RemoveRange(removedBooks);

            int countOtRemovedBooks = context.SaveChanges();
            return countOtRemovedBooks;
        }
    }
}
