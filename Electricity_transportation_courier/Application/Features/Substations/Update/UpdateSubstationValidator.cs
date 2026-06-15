using FluentValidation;

namespace Application.Features.Substations
{
    public class UpdateSubstationValidator : AbstractValidator<UpdateSubstationCommand>
    {
        public UpdateSubstationValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Location).NotEmpty();
            RuleFor(x => x.MaxThroughputMw).GreaterThan(0);
            RuleFor(x => x.Status).IsInEnum().WithMessage("Недопустимий статус підстанції.");
        }
    }
}