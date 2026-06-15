using FluentValidation;

namespace Application.Features.AvailablePowers
{
    public class UpdateAvailablePowerValidator : AbstractValidator<UpdateAvailablePowerCommand>
    {
        public UpdateAvailablePowerValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.AvailablePowerPlants)
                .NotNull().WithMessage("Список електростанцій не може бути null.");
        }
    }
}