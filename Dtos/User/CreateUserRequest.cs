using FluentValidation;

namespace HouseManagementApi.Dtos.User;

public class CreateUserRequest
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(user => user.FirstName)
            .NotNull()
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(user => user.LastName)
            .NotNull()
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(user => user.Email)
            .NotNull()
            .NotEmpty()
            .MaximumLength(255)
            .EmailAddress();

        RuleFor(user => user.Password)
            .NotNull()
            .NotEmpty()
            .MaximumLength(255);
    }
}
