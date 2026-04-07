using System.Net;
using Microsoft.AspNetCore.Mvc;
using NetInventory.Api.Common;
using NetInventory.Domain.Common;

namespace NetInventory.Api.Common.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToActionResult(this Result result, ControllerBase controller)
    {
        if (result.IsSuccess)
            return controller.NoContent();
       
        return result.Error.ToHttpResult(controller);
    }

    public static IActionResult ToActionResult(this Result result, ControllerBase controller, Func<IActionResult> onSuccess)
    {
        if (result.IsSuccess)
            return onSuccess();
        
        return result.Error.ToHttpResult(controller);
    }

    public static IActionResult ToActionResult<T>(this Result<T> result, ControllerBase controller,
        Func<T, IActionResult>? onSuccess = null)
    {
        if (result.IsSuccess)
            return onSuccess is not null
                ? onSuccess(result.Value)
                : controller.Ok(ApiResponse<object>.Ok(result.Value!));

        return result.Error.ToHttpResult(controller);
    }

    private static IActionResult ToHttpResult(this Error error, ControllerBase controller)
    {
        var response = ApiResponse.Fail(error);

        return error.HttpStatus switch
        {
            HttpStatusCode.NotFound => controller.NotFound(response),
            HttpStatusCode.Unauthorized => controller.Unauthorized(response),
            _ => controller.BadRequest(response)
        };
    }
}
