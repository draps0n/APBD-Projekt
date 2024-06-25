using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APBD_Projekt.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateClientAsync()
    {
        return StatusCode(StatusCodes.Status201Created);
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