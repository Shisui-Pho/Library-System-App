using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Models;
public class PaymentMethod
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [StringLength(255)]
    public string Description { get; set; }

    public bool IsActive { get; set; } = true;
}