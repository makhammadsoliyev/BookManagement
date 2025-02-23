namespace BookManagement.BusinessLogic.Models.Books;

public abstract class BookBaseModel
{
    public string Title { get; set; }

    public int PublicationYear { get; set; }

    public string AuthorName { get; set; }
}
