using APBD_Projekt.Exceptions.Abstractions;

namespace APBD_Projekt.Exceptions;

public class PaymentException(string message) : BadRequestException(message)
{
    
}