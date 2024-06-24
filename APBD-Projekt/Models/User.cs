using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_Projekt.Models;

[Table("User")]
public class User
{
    [Key] [Column("IdUser")] public int IdUser { get; set; }

    [Required]
    [Column("Login")]
    [MaxLength(50)]
    public string Login { get; set; }

    [Required]
    [Column("Password")]
    [MaxLength(60)]
    public string Password { get; set; }

    [Required]
    [Column("Salt")]
    [MaxLength(30)]
    public string Salt { get; set; }

    [Required]
    [Column("IdRole")]
    [ForeignKey(nameof(Role))]
    public int IdRole { get; set; }

    public Role Role { get; set; }

    [Required]
    [Column("RefreshToken")]
    [MaxLength(60)]
    public string RefreshToken { get; set; }

    [Column("RefreshTokenExp", TypeName = "datetime")]
    public DateTime? RefreshTokenExp { get; set; }
}