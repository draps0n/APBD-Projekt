using APBD_Projekt.ResponseModels;
using APBD_Projekt.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APBD_Projekt.Controllers;

[ApiController]
[Authorize]
[Route("/api/[controller]")]
public class RevenueController(IRevenueService revenueService) : ControllerBase
{
    [HttpGet("current")]
    public async Task<IActionResult> GetCurrentRevenueAsync(int? softwareId, string? currencyCode)
    {
        GetCurrentRevenueResponseModel response;
        if (softwareId == null)
        {
            response = await revenueService.GetCurrentTotalRevenueAsync(currencyCode);
        }
        else
        {
            response = await revenueService.GetCurrentRevenueForSoftwareAsync(softwareId.Value, currencyCode);
        }

        return Ok(response);
    }
    
    [HttpGet("forecast")]
    public async Task<IActionResult> GetForecastedRevenueAsync(int? softwareId, string? currencyCode)
    {
        GetForecastedRevenueResponse response;
        if (softwareId == null)
        {
            response = await revenueService.GetForecastedTotalRevenueAsync(currencyCode);
        }
        else
        {
            response = await revenueService.GetForecastedRevenueForSoftwareAsync(softwareId.Value, currencyCode);
        }

        return Ok(response);
    }
}