using FluentValidation;
using TavernHelios.Auth.Data.Models;

namespace TavernHelios.Auth.Validation
{
    public class RoleValidator : AbstractValidator<Role>
    {
        public RoleValidator()
        {
            RuleFor(role => role.Name)
                .NotEmpty().WithMessage("Название роли не может быть пустым.")
                .MaximumLength(50).WithMessage("Название роли не должно превышать 50 символов.");
        }
    }
}
