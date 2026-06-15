using FluentValidation;

namespace Application.Features.OwnerDeals
{
    public class CreateOwnerDealValidator : AbstractValidator<CreateOwnerDealCommand>
    {
        public CreateOwnerDealValidator()
        {
            RuleFor(x => x.OwnerName)
                .NotEmpty().WithMessage("Ім'я власника обов'язкове.")
                .MaximumLength(150);

            RuleFor(x => x.NumberPhone)
                .NotEmpty()
                .Matches(@"^\+380\d{9}$").WithMessage("Номер телефону має бути у форматі +380XXXXXXXXX.");

            RuleFor(x => x.PlaceLocation).NotEmpty();
            RuleFor(x => x.ConclusionDeal).NotEmpty();

            RuleFor(x => x.CompetionDeal)
                .GreaterThanOrEqualTo(x => x.ConclusionDeal)
                .When(x => x.CompetionDeal.HasValue)
                .WithMessage("Дата завершення не може бути раніше дати укладення угоди.");
        }
    }
}