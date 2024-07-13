namespace RiverBooks.Reporting;

public record BookSalesResult(Guid BookId, string Title, int Units, decimal Sales)
{
  private BookSalesResult() : this(Guid.Empty!, default!, default!, default!) { }
}
