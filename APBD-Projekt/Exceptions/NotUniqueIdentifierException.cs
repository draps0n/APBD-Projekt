using APBD_Projekt.Exceptions.Abstractions;

namespace APBD_Projekt.Exceptions;

public class NotUniqueIdentifierException(string message) : BadRequestException(message)
{
}