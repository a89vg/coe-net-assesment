using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel.DataAnnotations;
using TA_API.Auth;
using TA_API.Models;
using TA_API.Models.Data;

namespace TA_API.Filters;

public class ErrorHandlingFilter : IExceptionFilter
{
    public ErrorHandlingFilter(CurrentUserSessionProvider currentUserSession)
    {
        CurrentUserSession = currentUserSession.UserSession is not null ? currentUserSession.UserSession : null;
    }

    public UserSession? CurrentUserSession { get; set; }

    public void OnException(ExceptionContext context)
    {
        var action = ((ControllerActionDescriptor)context.ActionDescriptor).DisplayName;

        string errorId;

        if (context.Exception.Data.Contains("ErrorId"))
        {
            errorId = context.Exception.Data["ErrorId"].ToString();
        }
        else
        {
            errorId = Guid.NewGuid().ToString("D");
        }

        var response = new Response
        {
            ErrorId = errorId
        };

        response.Message = context.Exception switch
        {
            ValidationException validationException => validationException.Message,
            _ => context.Exception.Data.Contains("Message") ? context.Exception.Data["Message"].ToString() : context.Exception.Message,
        };

        var result = context.Exception switch
        {
            ValidationException => new BadRequestObjectResult(response),
            _ => new ObjectResult(response) { StatusCode = StatusCodes.Status500InternalServerError }
        };

        context.Result = result;
    }
}
