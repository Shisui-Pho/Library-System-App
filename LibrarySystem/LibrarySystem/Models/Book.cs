using LibrarySystem.Models.Base;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Models;

public class Book : BaseBook
{
    [Required]
    public override int Id { get; set; }

    [Required]
    public override required string ISBN { get; set; }

    [Required]
    public override required string BookTitle { get; set; }

    [Required]
    public override required string Publisher { get; set; }
    [Required]
    public override DateTime PublicationDate { get; set; }

    [Precision(18,2)]
    public override decimal Price { get; set; }
}//class