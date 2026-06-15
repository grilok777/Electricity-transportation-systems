using FluentValidation;

namespace Application.Features.PowerPlants
{
    public class UpdatePowerPlantValidator : AbstractValidator<UpdatePowerPlantCommand>
    {
        public UpdatePowerPlantValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.SubstationLineId).GreaterThan(0);
            RuleFor(x => x.MaxCapacityKw).GreaterThan(0);
            RuleFor(x => x.Status).IsInEnum();
        }
    }
}