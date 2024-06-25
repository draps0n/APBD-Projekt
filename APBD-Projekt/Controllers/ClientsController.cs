using APBD_Projekt.Exceptions;
using APBD_Projekt.RequestModels;
using APBD_Projekt.Services;
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
    public async Task<IActionResult> UpdateClientAsync(int clientId)
    {
        return StatusCode(StatusCodes.Status204NoContent);
    }

    [HttpDelete("{clientId:int}")]
    public async Task<IActionResult> DeleteClientAsync(int clientId)
    {
        return StatusCode(StatusCodes.Status204NoContent);
    }
}