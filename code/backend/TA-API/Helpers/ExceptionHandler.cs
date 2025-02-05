using Serilog;
using Serilog.Context;

namespace TA_API.Helpers;

public static class ExceptionHandler
{
    public static void LogError(this Exception ex, string message)
    {
        ex.Data.Add("Message", message);

        var errorId = Guid.NewGuid().ToString("D");
        ex.Data.Add("ErrorId", errorId);

        LogContext.PushProperty("ErrorId", errorId);

        Log.Error(ex, message);
    }
}
