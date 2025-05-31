using DineMetrics.Core.Enums;
using DineMetrics.Core.Models;
using DineMetrics.Core.Stores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DeniMetrics.WebAPI.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class PermissionAuthorizeAttribute(ManagementName management, PermissionAccess requiredAccess)
    : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var user = (User?)context.HttpContext.Items["User"];

        if (user == null)
        {
            context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            return;
        }

        var hasPermission = RolePermissionsStore.AllPermissions.Any(p =>
            p.Role == user.Role &&
            p.Management == management &&
            (
                p.Access == PermissionAccess.Full ||
                (p.Access == PermissionAccess.Read && requiredAccess == PermissionAccess.Read)
            ));

        if (!hasPermission)
        {
            context.Result = new JsonResult(new { message = "You don't have proper permission to do this" }) { StatusCode = StatusCodes.Status401Unauthorized };
            return;
        }

        await next();
    }
}
