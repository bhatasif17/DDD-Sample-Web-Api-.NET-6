using DDDSampleWebApi.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DDDSampleWebApi.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizedAttribute : Attribute, IAsyncActionFilter
{
    private const string ApiKeyHeaderName = "secret";

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var secret))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var ctx = context.HttpContext.RequestServices.GetRequiredService<ApiContext>();
        var hikerExists = ctx.Hikers
            .FirstOrDefault(x => x.Secret == secret.ToString());

        if (hikerExists is null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        context.HttpContext.Items.Add("secret", hikerExists.Secret);

        await next();
    }
}