using APBD_Projekt.Exceptions.Abstractions;

namespace APBD_Projekt.Exceptions;

public class InvalidRequestFormatException(string message) : BadRequestException(message)
{
    
}