using APBD_Projekt.Exceptions.Abstractions;

namespace APBD_Projekt.Exceptions;

public class ClientNotFoundException(string message) : NotFoundException(message)
{
}