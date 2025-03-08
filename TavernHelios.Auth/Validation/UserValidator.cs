using FluentValidation;
using TavernHelios.Auth.Data.Models;

namespace TavernHelios.Auth.Validation
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(user => user.FullName)
                .NotEmpty().WithMessage("Полное имя не может быть пустым.")
                .MaximumLength(100).WithMessage("Полное имя не должно превышать 100 символов.");

            RuleFor(user => user.Login)
                .NotEmpty().WithMessage("Логин не может быть пустым.")
                .MaximumLength(50).WithMessage("Логин не должен превышать 50 символов.");

            RuleFor(user => user.PasswordHash)
                .NotEmpty().WithMessage("Хэш пароля не может быть пустым.")
                .MaximumLength(255).WithMessage("Хэш пароля не должен превышать 255 символов.");
        }
    }
}
