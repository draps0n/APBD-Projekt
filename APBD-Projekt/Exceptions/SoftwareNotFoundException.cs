using APBD_Projekt.Exceptions.Abstractions;

namespace APBD_Projekt.Exceptions;

public class SoftwareNotFoundException(string message) : NotFoundException(message)
{
    
}