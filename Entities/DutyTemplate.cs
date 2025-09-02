namespace HouseManagementApi.Entities;

public class DutyTemplate
{
    public int Id { get; set; }

    public required string Name { get; set; }
    public required string Description { get; set; }
    public required DutyFrequency Frequency { get; set; }
    public required DutyType DutyType { get; set; }

    public required DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public required int HouseId { get; set; }
    public House House { get; set; } = null!;

    public required int? AssignedUserId { get; set; } // can be null for rollover duties
    public User? AssignedUser { get; set; }

    public required ICollection<DutyInstance> DutyInstances { get; set; } = [];
}

public enum DutyType
{
    Recurring,
    Rollover,
}

public enum DutyFrequency
{
    Daily,
    Weekly,
    Fortnightly,
    Monthly,
}
