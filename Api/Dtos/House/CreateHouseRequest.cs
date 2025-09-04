using FluentValidation;

namespace HouseManagementApi.Dtos.House;

public class CreateHouseRequest
{
  public required string Nickname { get; set; }
  public required string StreetNo { get; set; }
  public required string StreetName { get; set; }
  public required ICollection<int> UserIds = [];
}

public class CreateHouseRequestValidator : AbstractValidator<CreateHouseRequest>
{
  public CreateHouseRequestValidator()
  {
    RuleFor(house => house.Nickname)
      .NotNull()
      .NotEmpty()
      .MaximumLength(255);

    RuleFor(house => house.StreetNo)
      .NotNull()
      .NotEmpty()
      .MaximumLength(255);

    RuleFor(house => house.StreetName)
      .NotNull()
      .NotEmpty()
      .MaximumLength(255);

    RuleFor(house => house.UserIds)
      .NotNull()
      .NotEmpty().WithMessage("Cannot create a house with no users")
      .Must(list => list.Count <= 50).WithMessage("Cannot exceed over 50 users in a single house");
  }
}
