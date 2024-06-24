using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_Projekt.Models;

[Table("CompanyClient")]
public class CompanyClient : Client
{
    [Required]
    [MaxLength(100)]
    [Column("CompanyName")]
    public string CompanyName { get; set; }
    
    [Required]
    [StringLength(10, MinimumLength = 10)]
    [Column("KRS")]
    public string KRS { get; set; }
}