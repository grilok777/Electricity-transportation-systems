using FluentValidation;

namespace Application.Features.Forecasts
{
    public class UpdateForecastStatusValidator : AbstractValidator<UpdateForecastStatusCommand>
    {
        public UpdateForecastStatusValidator()
        {
            RuleFor(x => x.ForecastId).GreaterThan(0);
            RuleFor(x => x.NewStatus).IsInEnum().WithMessage("Недопустимий статус.");
        }
    }
}