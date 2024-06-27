namespace APBD_Projekt.Exceptions.Abstractions;

public abstract class BadRequestException(string message) : Exception(message)
{
}