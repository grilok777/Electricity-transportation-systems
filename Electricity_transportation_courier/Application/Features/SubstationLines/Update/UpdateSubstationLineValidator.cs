using FluentValidation;

namespace Application.Features.SubstationLines
{
    public class UpdateSubstationLineValidator : AbstractValidator<UpdateSubstationLineCommand>
    {
        public UpdateSubstationLineValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.LineName).NotEmpty().MaximumLength(150);
            RuleFor(x => x.BaseLoadKw).GreaterThanOrEqualTo(0);
        }
    }
}