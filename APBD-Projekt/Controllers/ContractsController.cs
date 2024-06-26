using APBD_Projekt.Exceptions;
using APBD_Projekt.RequestModels;
using APBD_Projekt.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APBD_Projekt.Controllers;

[ApiController]
[Authorize]
[Route("/api/clients/{clientId:int}/contracts")]
public class ContractsController(IContractsService contractsService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateNewContractAsync(int clientId,
        [FromBody] CreateContractRequestModel requestModel)
    {
        try
        {
            await contractsService.CreateContractAsync(clientId, requestModel);
            return StatusCode(StatusCodes.Status201Created);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("{contractId:int}")]
    public async Task<IActionResult> DeleteContractAsync(int clientId, int contractId)
    {
        try
        {
            await contractsService.DeleteContractByIdAsync(clientId, contractId);
            return StatusCode(StatusCodes.Status204NoContent);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost("{contractId:int}/pay")]
    public async Task<IActionResult> PayForContractAsync(int clientId, int contractId,
        [FromBody] PayForContractRequestModel requestModel)
    {
        try
        {
            var result = await contractsService.PayForContractAsync(clientId, contractId, requestModel.Amount);
            if (result == null)
            {
                return StatusCode(StatusCodes.Status201Created, "Payment successful");
            }

            return StatusCode(StatusCodes.Status201Created, result);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }
}