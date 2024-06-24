using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_Projekt.Models;

[Table("Category")]
public class Category
{
    [Key] [Column("IdCategory")] public int IdCategory { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("Name")]
    public string Name { get; set; }

    public ICollection<Software> Softwares { get; set; }
}