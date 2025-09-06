namespace HouseManagementApi.Entities;

public class HouseUser
{
    public required int UserId { get; set; }
    public User User { get; set; } = null!;

    public required int HouseId { get; set; }
    public House House { get; set; } = null!;

    public required UserRole UserRole { get; set; }

    public required DateTime JoinedAt { get; set; }
}

public enum UserRole
{
    Member,
    Admin,
}

