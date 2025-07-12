using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Models;
public class PickupPoint
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [StringLength(200)]
    public string Address { get; set; }

    [Required]
    [StringLength(50)]
    public string City { get; set; }

    [Required]
    [StringLength(50)]
    public string Province { get; set; }
    [Required]
    [StringLength(3)]
    public string ProvinceCode { get; set; }

    [StringLength(20)]
    public string Phone { get; set; }

    [StringLength(100)]
    public string ContactPerson { get; set; }

    public TimeSpan OpeningTime { get; set; }
    public TimeSpan ClosingTime { get; set; }

    public bool IsActive { get; set; } = true; //Active by default
}//class
