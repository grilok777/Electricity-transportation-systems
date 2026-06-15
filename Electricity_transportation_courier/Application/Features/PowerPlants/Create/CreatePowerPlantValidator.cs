using FluentValidation;

namespace Application.Features.PowerPlants
{
    public class CreatePowerPlantValidator : AbstractValidator<CreatePowerPlantCommand>
    {
        public CreatePowerPlantValidator()
        {
            RuleFor(x => x.DealId).GreaterThan(0);
            RuleFor(x => x.SubstationLineId).GreaterThan(0);
            RuleFor(x => x.Type).IsInEnum().WithMessage("Некоректний тип електростанції.");
            RuleFor(x => x.MaxCapacityKw).GreaterThan(0).WithMessage("Потужність має бути більшою за нуль.");
            RuleFor(x => x.Location).NotEmpty().MaximumLength(200);
            RuleFor(x => x.DateCommissioning).NotEmpty();
        }
    }
}