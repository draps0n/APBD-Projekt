using APBD_Projekt.RequestModels;
using APBD_Projekt.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APBD_Projekt.Controllers;

[ApiController]
[Authorize]
[Route("/api/clients/{clientId:int}/[controller]")]
public class SubscriptionsController(ISubscriptionsService subscriptionsService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateNewSubscriptionAsync(int clientId,
        [FromBody] CreateSubscriptionRequestModel requestModel)
    {
        var result = await subscriptionsService.CreateSubscriptionAsync(clientId, requestModel);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpPost("{subscriptionId:int}/payments")]
    public async Task<IActionResult> PayForSubscriptionAsync(int clientId, int subscriptionId,
        [FromBody] PayForSubscriptionRequestModel requestModel)
    {
        await subscriptionsService.PayForSubscriptionAsync(clientId, subscriptionId, requestModel);
        return StatusCode(StatusCodes.Status201Created); 
    }
}