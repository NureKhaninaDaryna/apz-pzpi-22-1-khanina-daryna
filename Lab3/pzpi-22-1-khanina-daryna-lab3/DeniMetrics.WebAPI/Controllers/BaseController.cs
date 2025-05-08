using DineMetrics.Core.Models;
using DineMetrics.Core.Shared;
using Microsoft.AspNetCore.Mvc;

namespace DeniMetrics.WebAPI.Controllers;

[ApiController]
[Route("[controller]")] 
public abstract class BaseController : Controller
{
    protected User? CurrentUser => HttpContext.Items["User"] as User;
    
    protected ActionResult HandleServiceResult(ServiceResult result)
    {
        return result.IsSuccess switch
        {
            true => Ok(),
            false when result.Error?.Message == "Not found" => NotFound(result.Error?.Message),
            false => BadRequest(result.Error)
        };
    }
    
    protected ActionResult HandleServiceResult<T>(ServiceResult<T> result)
    {
        return result.IsSuccess switch
        {
            true => Ok(result.Value),
            false when result.Error?.Message == "Not found" => NotFound(result.Error?.Message),
            false => BadRequest(result.Error)
        };
    }
}