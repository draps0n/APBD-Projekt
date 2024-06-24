using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_Projekt.Models;

public class Role
{
    [Key]
    public int IdRole { get; set; }

    [Required]
    [MaxLength(50)]
    public required string Name { get; set; }

    
    public ICollection<User> Users { get; set; }
}