namespace APBD_Projekt.Models;

public class Category
{
    public int IdCategory { get; private set; }
    public string Name { get; private set; }

    public ICollection<Software> Software { get; private set; } = [];

    protected Category()
    {
    }

    public Category(string name)
    {
        Name = name;
    }
}