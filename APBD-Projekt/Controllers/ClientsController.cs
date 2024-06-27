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
        await clientsService.CreateNewClientAsync(requestModel);
        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpPut("{clientId:int}")]
    public async Task<IActionResult> UpdateClientAsync(int clientId, [FromBody] UpdateClientRequestModel requestModel)
    {
        await clientsService.UpdateClientByIdAsync(clientId, requestModel);
        return StatusCode(StatusCodes.Status204NoContent);
    }

    [HttpDelete("{clientId:int}")]
    public async Task<IActionResult> DeleteClientAsync(int clientId)
    {
        await clientsService.DeleteClientByIdAsync(clientId);
        return StatusCode(StatusCodes.Status204NoContent);
    }
}