using BookManagement.Domain.Entities;
using System.Linq.Expressions;

namespace BookManagement.DataAccess.Infrastructure.Calculators;

public static class BookPopularityCalculator
{
    public static Expression<Func<Book, double>> GetCalculateExpressiom(int currentYear)
    {
        return (Book book) => book.Views * 0.5 + (currentYear - book.PublicationYear) * 2;
    }

    public static double Calculate(int views, int publicationYear, int currentYear)
    {
        return views * 0.5 + (currentYear - publicationYear) * 2;
    }
}
