using LibrarySystem.Infrastructure.Enums;

namespace LibrarySystem.Models.DTO;

public class SystemAlert
{
    public string Title { get; set; }
    public string Description { get; set; }
    public SystemAlertSeverityLevel SeverityLevel { get; set; }
}
