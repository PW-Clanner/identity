using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Pw.Clanner.Identity.Common.Exceptions;

namespace Pw.Clanner.Identity.Web;

public class ApiFilter : IExceptionFilter
{
    private readonly Dictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

    public ApiFilter()
    {
        _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
        {
            { typeof(AppValidationException), HandleValidationException }
        };
    }

    public void OnException(ExceptionContext context)
    {
        HandleException(context);
    }

    private void HandleException(ExceptionContext context)
    {
        var type = context.Exception.GetType();

        if (_exceptionHandlers.TryGetValue(type, out var handler))
        {
            handler.Invoke(context);
            return;
        }

        if (!context.ModelState.IsValid)
        {
            HandleModelStateException(context);
            return;
        }

        HandleUnknownException(context);
    }

    private void HandleValidationException(ExceptionContext context)
    {
        var exception = context.Exception as AppValidationException;
        var result = new ValidationProblemDetails(exception!.Errors);
        context.Result = new BadRequestObjectResult(result);
        context.ExceptionHandled = true;
    }

    private void HandleModelStateException(ExceptionContext context)
    {
        var result = new ValidationProblemDetails(context.ModelState);
        context.Result = new BadRequestObjectResult(result);
        context.ExceptionHandled = true;
    }

    private void HandleUnknownException(ExceptionContext context)
    {
        var result = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "InternalError",
            Detail = context.Exception.Message
        };

        context.Result = new ObjectResult(result) { StatusCode = StatusCodes.Status500InternalServerError };
        context.ExceptionHandled = true;
    }
}