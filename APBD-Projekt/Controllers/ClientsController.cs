using APBD_Projekt.Exceptions;
using APBD_Projekt.RequestModels;
using APBD_Projekt.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APBD_Projekt.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ClientsController(IClientsService clientsService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateClientAsync([FromBody] CreateClientRequestModel requestModel)
    {
        try
        {
            await clientsService.CreateNewClientAsync(requestModel);
            return StatusCode(StatusCodes.Status201Created);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }

    }

    [HttpPut("{clientId:int}")]
    public async Task<IActionResult> UpdateClientAsync(int clientId, [FromBody] UpdateClientRequestModel requestModel)
    {
        try
        {
            await clientsService.UpdateClientByIdAsync(clientId, requestModel);
            return StatusCode(StatusCodes.Status204NoContent);
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

    [HttpDelete("{clientId:int}")]
    public async Task<IActionResult> DeleteClientAsync(int clientId)
    {
        try
        {
            await clientsService.DeleteClientByIdAsync(clientId);
            return StatusCode(StatusCodes.Status204NoContent);
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