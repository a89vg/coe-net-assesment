using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog.Context;
using TA_API.Auth;
using TA_API.Helpers;
using TA_API.Models;
using TA_API.Models.Data;


namespace TA_API.Filters;

public class ErrorHandlingFilter(CurrentUserSessionProvider currentUserSession, ILogger<ErrorHandlingFilter> logger) : IExceptionFilter
{
    public UserSession? CurrentUserSession { get; set; } = currentUserSession.UserSession is not null ? currentUserSession.UserSession : null;

    public void OnException(ExceptionContext context)
    {
        var level = LogLevel.Error;
        int statusCode = 500;

        ResponseModel response;

        switch (context.Exception)
        {
            case ApiException apiException:
                response = apiException.ErrorResponse;
                level = apiException.Severity;
                statusCode = apiException.StatusCode;
                break;
            default:
                response = new ResponseModel
                {
                    Message = "Unhandled Error in API"
                };
                break;
        }

        var errorId = Guid.NewGuid().ToString();
        var action = (ControllerActionDescriptor)context.ActionDescriptor;
        var endpointId = action.MethodInfo.Name;

        LogContext.PushProperty("ErrorId", errorId);
        LogContext.PushProperty("EndpointId", endpointId);

        if (CurrentUserSession is not null)
        {
            LogContext.PushProperty("User", CurrentUserSession.Username);
        }

        logger.Log(level, context.Exception, response.ErrorDetails ?? response.Message);

        response.ErrorId = errorId;

        var result = new ObjectResult(response) { StatusCode = statusCode };

        context.Result = result;
    }
}
