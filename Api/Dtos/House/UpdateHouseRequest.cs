using FluentValidation;

namespace HouseManagementApi.Dtos.House;

public class UpdateHouseRequest
{
  public string? Nickname { get; set; }
  public string? StreetNo { get; set; }
  public string? StreetName { get; set; }
}

public class UpdateHouseRequestValidator : AbstractValidator<UpdateHouseRequest>
{
  public UpdateHouseRequestValidator()
  {
    RuleFor(house => house.Nickname)
      .NotEmpty()
      .MaximumLength(255);

    RuleFor(house => house.StreetNo)
      .NotEmpty()
      .MaximumLength(255);

    RuleFor(house => house.StreetName)
      .NotEmpty()
      .MaximumLength(255);
  }
}
