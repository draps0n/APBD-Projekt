using APBD_Projekt.RequestModels;
using APBD_Projekt.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APBD_Projekt.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUsersService usersService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequestModel requestModel)
    {
        await usersService.RegisterUserAsync(requestModel.Login, requestModel.Password);
        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginUserRequestModel requestModel)
    {
        var responseModel = await usersService.LoginUserAsync(requestModel.Login, requestModel.Password);
        return Ok(responseModel);
    }

    [HttpPost("refresh")]
    [Authorize(AuthenticationSchemes = "IgnoreTokenExpirationScheme")]
    public async Task<IActionResult> Refresh(RefreshTokenRequestModel requestModel)
    {
        var login = HttpContext.User.Identity!.Name;
        var responseModel = await usersService.RefreshUserTokenAsync(login, requestModel.RefreshToken);
        return Ok(responseModel);
    }
}