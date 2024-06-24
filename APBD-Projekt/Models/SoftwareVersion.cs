using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_Projekt.Models;

[Table("SoftwareVersion")]
public class SoftwareVersion
{
    [Key] [Column("IdSoftwareVersion")] public int IdSoftwareVersion { get; set; }

    [Required]
    [ForeignKey(nameof(Software))]
    public int IdSoftware { get; set; }

    public Software Software { get; set; }

    [Required] [MaxLength(30)] public string Version { get; set; }

    public ICollection<Contract> Contracts { get; set; }
}