using FluentValidation;

namespace RiverBooks.Users.UseCases.Cart.AddItem;

public class AddItemToCartCommandValidator : AbstractValidator<AddItemToCartCommand>
{
  public AddItemToCartCommandValidator()
  {
    RuleFor(x => x.BookId).NotEmpty()
                          .WithMessage("Not a valid Book id.");
    RuleFor(x => x.Quantity).GreaterThan(0)
                            .WithMessage("Quantity must be greater than 0.");
    RuleFor(x => x.EmailAddress).NotEmpty()
                                .EmailAddress()
                                .WithMessage("Not a valid email address.");
  }
}
