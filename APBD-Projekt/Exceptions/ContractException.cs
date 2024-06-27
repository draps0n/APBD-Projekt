using APBD_Projekt.Exceptions.Abstractions;

namespace APBD_Projekt.Exceptions;

public class ContractException(string message) : BadRequestException(message)
{
    
}