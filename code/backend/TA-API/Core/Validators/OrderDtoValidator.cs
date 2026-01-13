using FluentValidation;
using TA_API.Core.Dtos;

namespace TA_API.Core.Validators;

public class OrderDtoValidator : AbstractValidator<OrderDto>
{
    public OrderDtoValidator()
    {
        RuleFor(o => o.CustomerId)
            .GreaterThan(0)
            .WithMessage("CustomerId must be greater than 0.");

        RuleFor(o => o.Items)
            .NotEmpty()
            .WithMessage("Order must contain at least one item.");

        RuleForEach(o => o.Items).SetValidator(new ItemDtoValidator());
    }
}
