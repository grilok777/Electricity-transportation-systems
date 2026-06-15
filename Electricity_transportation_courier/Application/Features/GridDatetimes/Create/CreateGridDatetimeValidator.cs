using FluentValidation;

namespace Application.Features.GridDatetimes
{
    public class CreateGridDatetimeValidator : AbstractValidator<CreateGridDatetimeCommand>
    {
        public CreateGridDatetimeValidator()
        {
            RuleFor(x => x.AvailablePower).GreaterThanOrEqualTo(0).WithMessage("Доступна потужність не може бути від'ємною.");
            RuleFor(x => x.RequiredPower).GreaterThanOrEqualTo(0).WithMessage("Необхідна потужність не може бути від'ємною.");
            RuleFor(x => x.ForecastDate).NotEmpty();
            RuleFor(x => x.ForecastTimestamp).NotEmpty();
        }
    }
}