using FluentValidation;

namespace Application.Features.Substations
{
    public class CreateSubstationValidator : AbstractValidator<CreateSubstationCommand>
    {
        public CreateSubstationValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Назва підстанції є обов'язковою.")
                .MaximumLength(100).WithMessage("Назва не може перевищувати 100 символів.");

            RuleFor(x => x.Location)
                .NotEmpty().WithMessage("Локація є обов'язковою.");

            RuleFor(x => x.MaxThroughputMw)
                .GreaterThan(0).WithMessage("Максимальна пропускна здатність має бути більшою за нуль.");
        }
    }
}