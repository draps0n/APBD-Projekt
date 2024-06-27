using APBD_Projekt.Exceptions.Abstractions;

namespace APBD_Projekt.Exceptions;

public class ClientTypeException(string message) : BadRequestException(message)
{
}