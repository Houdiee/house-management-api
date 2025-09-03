namespace HouseManagementApi.Entities;

public class User
{
    public int Id { get; set; }

    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string HashedPassword { get; set; }

    public required ICollection<HouseUser> HouseUsers { get; set; } = [];
    public required ICollection<DutyInstance> DutyInstances { get; set; } = [];
}
