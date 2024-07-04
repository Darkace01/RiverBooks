using FastEndpoints;
using FluentValidation;

namespace RiverBooks.Books.Endpoints;

public record CreateBookRequest(Guid? Id, decimal Price, string Title, string Author);

public class CreateBookRequestValidator: Validator<CreateBookRequest>
{
  public CreateBookRequestValidator()
  {
    RuleFor(x => x.Id)
      .NotEqual(Guid.Empty)
      .WithMessage("A book id is required.");
    
    RuleFor(x => x.Price)
      .GreaterThanOrEqualTo(0)
      .WithMessage("Book prices may not be negative.");
    
    RuleFor(x => x.Title)
      .NotNull()
      .NotEmpty()
      .WithMessage("A book title is required.");
    
    RuleFor(x => x.Author)
      .NotNull()
      .NotEmpty()
      .WithMessage("A book author is required.");
  }
}
