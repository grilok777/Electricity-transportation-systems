using FluentValidation;

namespace Application.Features.AvailablePowers  
{
    public class CreateAvailablePowerValidator : AbstractValidator<CreateAvailablePowerCommand>
    {
        public CreateAvailablePowerValidator()
        {
            RuleFor(x => x.SubstationLineId).GreaterThan(0);
            RuleFor(x => x.DatetimeId).GreaterThan(0);
            RuleFor(x => x.AvailablePowerPlants)
                .NotNull().WithMessage("Список електростанцій не може бути null.");
        }
    }
}