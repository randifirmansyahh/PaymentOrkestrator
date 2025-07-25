using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PaymentOrkestrator.shared.constants;
using PaymentOrkestrator.shared.helpers;
using System.Net;

namespace PaymentOrkestrator.shared.extensions
{
    public static class ApiResultExtensions
    {
        // use this method to handle catch exceptions on controller
        public static IActionResult HandleException(this ControllerBase controller, Exception ex)
        {
            if (ex is CustomHttpException customEx)
            {
                return controller.StatusCode((int)customEx.StatusCode, customEx.GetBody());
            }
            return controller.StatusCode((int)HttpStatusCode.InternalServerError, new CustomHttpException(ErrorCodes.INTERNAL_SERVER_ERROR).GetBody());
        }

        // use this method to handle catch exceptions on HttpContext / Middleware
        public static async Task<HttpContext> HandleException(this HttpContext httpContext, Exception ex)
        {
            if (ex is CustomHttpException customEx)
            {
                httpContext.Response.StatusCode = (int)customEx.StatusCode;
                await httpContext.Response.WriteAsJsonAsync(customEx.GetBody());
                return httpContext;
            }

            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(new CustomHttpException(ErrorCodes.INTERNAL_SERVER_ERROR).GetBody());
            return httpContext;
        }

        public static async Task<HttpContext> HandleException(this HttpContext httpContext, string? message, object? error)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(new CustomHttpException(message, error).GetBody());
            return httpContext;
        }

        public static IActionResult HandleValidation(this ControllerBase controller, ModelStateDictionary modelState)
        {
            return controller.BadRequest(new CustomHttpException(modelState).GetBody());
        }

        public static IActionResult HandleValidation(this ControllerBase controller, ValidationResult validationResult)
        {
            return controller.BadRequest(new CustomHttpException(validationResult).GetBody());
        }

        public static IActionResult HandleValidation(this ControllerBase controller, ModelStateDictionary modelState, string key, string message)
        {
            modelState.AddModelError(key, message);
            return controller.BadRequest(new CustomHttpException(modelState).GetBody());
        }

        // for more than one key message pairs
        public static IActionResult HandleValidation(this ControllerBase controller, ModelStateDictionary modelState, Dictionary<string, string> keyMessagePairs)
        {
            foreach (var kvp in keyMessagePairs)
            {
                modelState.AddModelError(kvp.Key, kvp.Value);
            }
            return controller.BadRequest(new CustomHttpException(modelState).GetBody());
        }

        public static IActionResult HandleValidation(this ControllerBase controller, ErrorCode? errorCodes = null, string? message = null)
        {
            return controller.BadRequest(new CustomHttpException(errorCodes ?? ErrorCodes.VALIDATION_ERROR, message).GetBody());
        }

        public static async Task<HttpContext> HandleUnauthorized(this HttpContext httpContext, ErrorCode? errorCodes = null, string? message = null)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await httpContext.Response.WriteAsJsonAsync(new CustomHttpException(errorCodes ?? ErrorCodes.INVALID_API_KEY, message).GetBody());
            return httpContext;
        }

        public static IActionResult HandleNotFound(this ControllerBase controller, ErrorCode errorCodes)
        {
            return controller.NotFound(new CustomHttpException(errorCodes).GetBody());
        }

        public static IActionResult HandleSuccess<T>(this ControllerBase controller, T? data = default, string message = "Success")
        {
            return controller.Ok(new CustomHttpSuccess<T>(data, message));
        }
    }
}
