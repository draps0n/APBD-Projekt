using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_Projekt.Models;

public class SoftwareVersion
{
    [Key]
    public int IdSoftwareVersion { get; set; }

    [Required]
    [ForeignKey(nameof(Software))]
    public int IdSoftware { get; set; }

    [Required]
    [MaxLength(30)]
    public string Version { get; set; }
    
    
    public Software Software { get; set; }
    public ICollection<Contract> Contracts { get; set; }
}