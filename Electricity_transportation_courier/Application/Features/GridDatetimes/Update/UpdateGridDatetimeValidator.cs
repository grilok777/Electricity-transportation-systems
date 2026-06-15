using FluentValidation;

namespace Application.Features.GridDatetimes
{
    public class UpdateGridDatetimeValidator : AbstractValidator<UpdateGridDatetimeCommand>
    {
        public UpdateGridDatetimeValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.AvailablePower).GreaterThanOrEqualTo(0);
            RuleFor(x => x.RequiredPower).GreaterThanOrEqualTo(0);
        }
    }
}