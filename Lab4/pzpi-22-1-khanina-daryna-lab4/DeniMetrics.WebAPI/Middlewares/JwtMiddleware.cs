using DineMetrics.BLL.Services.Interfaces;

namespace DeniMetrics.WebAPI.Middlewares;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IUserService userService, IJwtService jwtService)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var userId = jwtService.ValidateJwtToken(token);
        if (userId != null)
        {
            context.Items["User"] = await userService.GetUserById(userId.Value);
        }

        await _next(context);
    }
}