namespace HouseManagementApi.Dtos.User;

using FluentValidation;

public class UpdateUserRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
}


public class UpdateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public UpdateUserRequestValidator()
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
