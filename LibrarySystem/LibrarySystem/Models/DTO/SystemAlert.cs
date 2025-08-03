using LibrarySystem.Infrastructure.Enums;

namespace LibrarySystem.Models.DTO;

public class SystemAlert
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public SystemAlertSeverityLevel SeverityLevel { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
