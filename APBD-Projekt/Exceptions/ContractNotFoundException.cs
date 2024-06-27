using APBD_Projekt.Exceptions.Abstractions;

namespace APBD_Projekt.Exceptions;

public class ContractNotFoundException(string message) : NotFoundException(message)
{
    
}