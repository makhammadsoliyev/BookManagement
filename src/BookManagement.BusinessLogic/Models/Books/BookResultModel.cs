namespace BookManagement.BusinessLogic.Models.Books;

public class BookResultModel : BookBaseModel
{
    public long Id { get; set; }

    public int Views { get; set; }

    public double PopularityScore { get; set; }
}