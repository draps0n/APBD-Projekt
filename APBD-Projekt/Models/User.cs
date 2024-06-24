using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_Projekt.Models;

public class User
{
    [Key]
    public int IdUser { get; set; }

    [Required]
    [MaxLength(50)]
    public string Login { get; set; }

    [Required]
    [MaxLength(60)]
    public string Password { get; set; }

    [Required]
    [MaxLength(30)]
    public string Salt { get; set; }

    [Required]
    [ForeignKey(nameof(Role))]
    public int IdRole { get; set; }

    [Required]
    [MaxLength(60)]
    public string RefreshToken { get; set; }

    [Column("RefreshTokenExp", TypeName = "datetime")]
    public DateTime? RefreshTokenExp { get; set; }
    
    
    public Role Role { get; set; }
}