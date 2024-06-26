using APBD_Projekt.Exceptions;
using APBD_Projekt.RequestModels;
using APBD_Projekt.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APBD_Projekt.Controllers;

[ApiController]
[Authorize]
[Route("/api/clients/{clientId:int}/orders")]
public class ContractsController(IContractsService contractsService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateNewContract(int clientId, [FromBody] CreateContractRequestModel requestModel)
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
}