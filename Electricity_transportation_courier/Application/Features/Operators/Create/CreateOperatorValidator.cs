using FluentValidation;

namespace Application.Features.Operators.Create
{
    public class CreateOperatorValidator : AbstractValidator<CreateOperatorCommand>
    {
        public CreateOperatorValidator()
        {
            RuleFor(x => x.Username).NotEmpty().MinimumLength(3);
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6).WithMessage("Пароль має містити мінімум 6 символів.");
            RuleFor(x => x.AccessLevel).NotEmpty();
        }
    }
}