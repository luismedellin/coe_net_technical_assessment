using FluentValidation;
using TA_API.Core.Dtos;

namespace TA_API.Core.Validators;

public class ItemDtoValidator : AbstractValidator<ItemDto>
{
    public ItemDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Item Id must be greater than 0.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Item Quantity must be greater than 0.")
            .LessThanOrEqualTo(100)
            .WithMessage("Item Quantity must be less than or equal to 100.");
    }
}
