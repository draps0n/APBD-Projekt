using APBD_Projekt.RequestModels;
using APBD_Projekt.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APBD_Projekt.Controllers;

[ApiController]
[Authorize]
[Route("/api/clients/{clientId:int}/[controller]")]
public class ContractsController(IContractsService contractsService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateNewContractAsync(int clientId,
        [FromBody] CreateContractRequestModel requestModel)
    {
        var result = await contractsService.CreateContractAsync(clientId, requestModel);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpDelete("{contractId:int}")]
    public async Task<IActionResult> DeleteContractAsync(int clientId, int contractId)
    {
        await contractsService.DeleteContractByIdAsync(clientId, contractId);
        return StatusCode(StatusCodes.Status204NoContent, "Successfully deleted");
    }

    [HttpPost("{contractId:int}/payments")]
    public async Task<IActionResult> PayForContractAsync(int clientId, int contractId,
        [FromBody] PayForContractRequestModel requestModel)
    {
        var result = await contractsService.PayForContractAsync(clientId, contractId, requestModel.Amount);
        return result == null
            ? StatusCode(StatusCodes.Status201Created, "Payment successful")
            : StatusCode(StatusCodes.Status201Created, result);
    }
}