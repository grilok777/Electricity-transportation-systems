using FluentValidation;

namespace Application.Features.OwnerDeals
{
    public class UpdateOwnerDealValidator : AbstractValidator<UpdateOwnerDealCommand>
    {
        public UpdateOwnerDealValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.OwnerName).NotEmpty().MaximumLength(150);
            RuleFor(x => x.NumberPhone).NotEmpty().Matches(@"^\+380\d{9}$");
            RuleFor(x => x.PlaceLocation).NotEmpty();
            RuleFor(x => x.CompetionDeal)
                .GreaterThanOrEqualTo(x => x.ConclusionDeal)
                .When(x => x.CompetionDeal.HasValue);
        }
    }
}