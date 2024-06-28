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
        var (wasSigned, alternativeContract) =
            await contractsService.PayForContractAsync(clientId, contractId, requestModel.Amount);

        if (alternativeContract == null)
        {
            return StatusCode(StatusCodes.Status201Created,
                "Payment successful" + (wasSigned ? ". Contract has been signed" : ""));
        }

        alternativeContract.Message = "Contract is not active. Here is a new offer";
        return StatusCode(StatusCodes.Status201Created, alternativeContract);
    }
}