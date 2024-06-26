using APBD_Projekt.Exceptions;
using APBD_Projekt.RequestModels;
using APBD_Projekt.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace APBD_Projekt.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUsersService usersService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequestModel requestModel)
    {
        try
        {
            await usersService.RegisterUserAsync(requestModel.Login, requestModel.Password);
            return Ok();
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginUserRequestModel requestModel)
    {
        try
        {
            var responseModel = await usersService.LoginUserAsync(requestModel.Login, requestModel.Password);
            return Ok(responseModel);
        }
        catch (UnauthorizedException e)
        {
            return Unauthorized(e.Message);
        }
    }

    [HttpPost("refresh")]
    [Authorize(AuthenticationSchemes = "IgnoreTokenExpirationScheme")]
    public async Task<IActionResult> Refresh(RefreshTokenRequestModel requestModel)
    {
        try
        {
            var login = HttpContext.User.Identity!.Name;
            var responseModel = await usersService.RefreshUserTokenAsync(login, requestModel.RefreshToken);
            return Ok(responseModel);
        }
        catch (SecurityTokenException e)
        {
            return Unauthorized(e.Message);
        }
    }
}