#pragma warning disable CS8618

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingPlanner.Models;


public class User
{
    [Key]
    public int UserId { get; set; }

    [Required(ErrorMessage = "is required")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "is required")]
    [Display(Name = "Last Name")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "is required")]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "is required")]
    [MinLength(8, ErrorMessage = "must be at least 8 characters long.")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [NotMapped]
    [Required(ErrorMessage = "is required")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "doesn't match password.")]
    [Display(Name = "Confirm Password")]
    public string ConfirmPassword { get; set; }

    public DateTime CreateAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    List<Association> Weddings { get; set; } = new List<Association>();

}