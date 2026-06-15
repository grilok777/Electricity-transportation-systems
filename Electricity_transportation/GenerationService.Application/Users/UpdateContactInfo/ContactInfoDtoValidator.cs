using FluentValidation;

namespace Application.Users
{
    public class ContactInfoDtoValidator : AbstractValidator<ContactInfoDto>
    {
        public ContactInfoDtoValidator()
        {
            RuleFor(x => x.NumberPhone)
                .NotEmpty().WithMessage("Номер телефону є обов'язковим.")
                .Matches(@"^\+380\d{9}$").WithMessage("Формат має бути +380XXXXXXXXX");

            RuleFor(x => x.PlaceLocation)
                .NotEmpty().WithMessage("Локація є обов'язковою.")
                .MaximumLength(200).WithMessage("Локація не може перевищувати 200 символів.");
        }
    }
}
