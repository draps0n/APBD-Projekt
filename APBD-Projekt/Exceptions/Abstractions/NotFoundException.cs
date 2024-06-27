namespace APBD_Projekt.Exceptions.Abstractions;

public abstract class NotFoundException(string message) : Exception(message)
{
}