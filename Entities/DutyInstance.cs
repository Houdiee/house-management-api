namespace HouseManagementApi.Entities;

public class DutyInstance
{
    public int Id { get; set; }

    public required DateTime DueDate { get; set; }
    public required DateTime CompletedAt { get; set; }

    public required int AssignedUserId { get; set; }
    public User AssignedUser { get; set; } = null!;

    public required int TemplateId { get; set; }
    public DutyTemplate Template { get; set; } = null!;
}
