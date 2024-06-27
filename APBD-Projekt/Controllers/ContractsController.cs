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
        await contractsService.CreateContractAsync(clientId, requestModel);
        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpDelete("{contractId:int}")]
    public async Task<IActionResult> DeleteContractAsync(int clientId, int contractId)
    {
        await contractsService.DeleteContractByIdAsync(clientId, contractId);
        return StatusCode(StatusCodes.Status204NoContent);
    }

    [HttpPost("{contractId:int}/payments")]
    public async Task<IActionResult> PayForContractAsync(int clientId, int contractId,
        [FromBody] PayForContractRequestModel requestModel)
    {
        var result = await contractsService.PayForContractAsync(clientId, contractId, requestModel.Amount);
        if (result == null)
        {
            return StatusCode(StatusCodes.Status201Created, "Payment successful");
        }

        return StatusCode(StatusCodes.Status201Created, result);
    }
}