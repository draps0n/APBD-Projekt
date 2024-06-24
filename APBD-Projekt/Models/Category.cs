using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_Projekt.Models;

public class Category
{
    [Key]
    public int IdCategory { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    
    public ICollection<Software> Software { get; set; }
}