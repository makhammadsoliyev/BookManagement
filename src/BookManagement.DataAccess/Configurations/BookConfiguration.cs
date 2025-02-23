using BookManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookManagement.DataAccess.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(book => book.Id);

        builder.Property(book => book.Title)
            .HasMaxLength(255)
            .UseCollation("SQL_Latin1_General_CP1_CI_AS")
            .IsRequired();

        builder.Property(book => book.AuthorName)
            .HasMaxLength(255)
            .IsRequired();

        builder.HasIndex(book => book.Title)
            .IsUnique();

        builder.HasIndex(book => book.IsDeleted);

        builder.HasQueryFilter(book => !book.IsDeleted);
    }
}
