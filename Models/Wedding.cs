#pragma warning disable CS8618

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingPlanner.Models;


public class Wedding
{
    [Key]
    public int WeddingId { get; set; }

    [Required(ErrorMessage ="is required")]
    [Display(Name = "Wedder One")]
    public string WedderOne { get; set; }

    [Required(ErrorMessage ="is required")]
    [Display(Name = "Wedder Two")]
    public string WedderTwo { get; set; }

    [Required(ErrorMessage ="is required")]
    [Display(Name = "Date")]
    [FutureDate]
    public DateTime Date { get; set; }

    [Required(ErrorMessage ="is required")]
    [Display(Name = "Wedding Address")]
    public string WeddingAddress { get; set; }

    public DateTime CreateAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public int UserId { get; set; }

    public User? Planner { get; set; }

    public List<Association> Attendees { get; set; } = new List<Association>();
}