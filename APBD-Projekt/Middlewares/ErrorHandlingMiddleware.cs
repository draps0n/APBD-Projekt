using System.Net;
using System.Security.Authentication;
using APBD_Projekt.Exceptions;
using APBD_Projekt.Exceptions.Abstractions;
using Microsoft.IdentityModel.Tokens;

namespace APBD_Projekt.Middlewares;

public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (BadRequestException e)
        {
            logger.LogError(e, "An error occurred while processing your request");
            await HandleBadRequestExceptionAsync(context, e);
        }
        catch (NotFoundException e)
        {
            logger.LogError(e, "Some of given resources could not be found");
            await HandleNotFoundExceptionAsync(context, e);
        }
        catch (InvalidCredentialException e)
        {
            logger.LogError(e, "An error occured during authorization process");
            await HandleUnauthorizedExceptionAsync(context, e);
        }
        catch (SecurityTokenException e)
        {
            logger.LogError(e, "An error occured during processing your token");
            await HandleSecurityTokenExceptionAsync(context, e);
        }
        catch (Exception e)
        {
            logger.LogError(e, "An unhandled exception occurred");
            await HandleOtherExceptionAsync(context);
        }
    }

    private static async Task HandleSecurityTokenExceptionAsync(HttpContext context, SecurityTokenException exception)
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        context.Response.ContentType = "application/json";

        var response = new
        {
            error = new
            {
                message = "An error occured during processing your token",
                detail = exception.Message
            }
        };

        var jsonResponse = System.Text.Json.JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(jsonResponse);
    }

    private static async Task HandleNotFoundExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
        context.Response.ContentType = "application/json";

        var response = new
        {
            error = new
            {
                message = "Some of given resources could not be found",
                detail = exception.Message
            }
        };

        var jsonResponse = System.Text.Json.JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(jsonResponse);
    }

    private static async Task HandleUnauthorizedExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        context.Response.ContentType = "application/json";

        var response = new
        {
            error = new
            {
                message = "An error occured during authorization process",
                detail = exception.Message
            }
        };

        var jsonResponse = System.Text.Json.JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(jsonResponse);
    }

    private static async Task HandleBadRequestExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        context.Response.ContentType = "application/json";

        var response = new
        {
            error = new
            {
                message = "An error occurred while processing your request",
                detail = exception.Message
            }
        };

        var jsonResponse = System.Text.Json.JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(jsonResponse);
    }
    
    private static async Task HandleOtherExceptionAsync(HttpContext context)
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/json";

        var response = new
        {
            error = new
            {
                message = "An error occurred while processing your request"
            }
        };

        var jsonResponse = System.Text.Json.JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(jsonResponse);
    }
}