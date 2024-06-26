using APBD_Projekt.Exceptions;
using APBD_Projekt.RequestModels;
using APBD_Projekt.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APBD_Projekt.Controllers;

[ApiController]
[Authorize]
[Route("/api/clients/{clientId:int}/subscriptions")]
public class SubscriptionsController(ISubscriptionsService subscriptionsService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateNewSubscriptionAsync(int clientId,
        [FromBody] CreateSubscriptionRequestModel requestModel)
    {
        try
        {
            var result = await subscriptionsService.CreateSubscriptionAsync(clientId, requestModel);
            return StatusCode(StatusCodes.Status201Created, result);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost("{subscriptionId:int}/payments")]
    public async Task<IActionResult> PayForSubscriptionAsync(int clientId, int subscriptionId,
        [FromBody] PayForSubscriptionRequestModel requestModel)
    {
        try
        {
            await subscriptionsService.PayForSubscriptionAsync(clientId, subscriptionId, requestModel);
            return StatusCode(StatusCodes.Status201Created);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}