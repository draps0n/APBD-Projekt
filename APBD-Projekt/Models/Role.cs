using System.ComponentModel.DataAnnotations;

namespace APBD_Projekt.Models;

public class Role
{
    [Key]
    public int IdRole { get; private set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; private set; }


    public ICollection<User> Users { get; private set; } = [];

    protected Role()
    {

    }

    public Role(string name)
    {
        Name = name;
    }
}