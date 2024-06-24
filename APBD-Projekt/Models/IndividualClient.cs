using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_Projekt.Models;

[Table("IndividualClient")]
public class IndividualClient : Client
{
    [Required]
    [MaxLength(50)]
    [Column("Name")]
    public string Name { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("LastName")]
    public string LastName { get; set; }

    [Required]
    [StringLength(11, MinimumLength = 11)]
    [Column("PESEL")]
    public string PESEL { get; set; }
}