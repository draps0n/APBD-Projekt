namespace APBD_Projekt.Models;

public class Role
{
    public int IdRole { get; private set; }
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