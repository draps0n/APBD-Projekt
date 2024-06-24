using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_Projekt.Models;

[Table("Role")]
public class Role
{
    [Key] [Column("IdRole")] public int IdRole { get; set; }

    [Required]
    [Column("Name")]
    [MaxLength(50)]
    public required string Name { get; set; }

    public ICollection<User> Users { get; set; }
}