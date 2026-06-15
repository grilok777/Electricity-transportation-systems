using FluentValidation;

namespace Application.Features.SubstationLines
{
    public class CreateSubstationValidator : AbstractValidator<CreateSubstationLineCommand>
    {
        public CreateSubstationValidator()
        {
            RuleFor(x => x.SubstationId).GreaterThan(0).WithMessage("ID підстанції є обов'язковим.");
            RuleFor(x => x.LineName).NotEmpty().MaximumLength(150).WithMessage("Назва лінії не може бути порожньою або довшою за 150 символів.");
            RuleFor(x => x.BaseLoadKw).GreaterThanOrEqualTo(0).WithMessage("Базове навантаження не може бути від'ємним.");
        }
    }
}