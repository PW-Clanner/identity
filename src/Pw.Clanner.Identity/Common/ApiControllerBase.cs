using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Pw.Clanner.Identity.Common;

[ApiController]
public class ApiControllerBase : ControllerBase
{
    private ISender _mediatr;
    
    protected ISender Mediatr => _mediatr ??= HttpContext.RequestServices.GetService<ISender>();
}