using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RiverBooks.Books;
internal class BookConfiguration : IEntityTypeConfiguration<Book>
{
  internal static readonly Guid Book1Guid = new("A89F6CD7-3D3D-4D3D-8D3D-3D3D3D3D3D3D");
  internal static readonly Guid Book2Guid = new("B89F6CD7-3D3D-4D3D-8D3D-3D3D3D3D3D3D");
  internal static readonly Guid Book3Guid = new("C89F6CD7-3D3D-4D3D-8D3D-3D3D3D3D3D3D");
  public void Configure(EntityTypeBuilder<Book> builder)
  {
    builder.Property(b => b.Title)
      .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH)
      .IsRequired();

    builder.Property(b => b.Author)
      .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH)
      .IsRequired();

    builder.HasData(GetSampleBookData());
  }

  private static IEnumerable<Book> GetSampleBookData()
  {
    var tolkien = "J.R.R. Tolkien";
    yield return new Book(Book1Guid, "The Fellowship of the Ring", tolkien, 10.99m);
    yield return new Book(Book2Guid, "The Two Towers", tolkien, 11.99m);
    yield return new Book(Book3Guid, "The Return of the King", tolkien, 9.99m);
  }
}
