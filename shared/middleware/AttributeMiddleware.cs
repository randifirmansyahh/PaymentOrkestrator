using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using PaymentOrkestrator.core.merchant.service;
using PaymentOrkestrator.shared.constants;
using PaymentOrkestrator.shared.extensions;

namespace PaymentOrkestrator.shared.middleware
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class MerchantAuthAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var httpContext = context.HttpContext;
            var _logger = httpContext.RequestServices.GetRequiredService<ILogger<MerchantAuthAttribute>>();
            var merchantService = httpContext.RequestServices.GetRequiredService<MerchantService>();

            const string XApiKeyHeader = "x-api-key";

            try
            {
                // Check if the endpoint allows anonymous access
                var allowAnonymous = httpContext.GetEndpoint()?.Metadata.GetMetadata<IAllowAnonymous>() != null;
                if (allowAnonymous)
                {
                    await next();
                    return;
                }

                if (!httpContext.GetHeader(XApiKeyHeader, out var extractedApiKey))
                {
                    _logger.LogWarning("API key '{ApiKey}' not found in request headers", XApiKeyHeader);
                    await httpContext.HandleUnauthorized(ErrorCodes.MISSING_API_KEY, $"API Key '{XApiKeyHeader}' is missing");
                    return;
                }

                if (extractedApiKey.ToString().Split("|").Length != 2)
                {
                    _logger.LogWarning("Invalid API key format for header {XApiKeyHeader}: {ApiKey}", XApiKeyHeader, extractedApiKey);
                    await httpContext.HandleUnauthorized(ErrorCodes.INVALID_API_KEY, $"API Key '{XApiKeyHeader}' is invalid");
                    return;
                }

                var apiKey = extractedApiKey.ToString().Split("|").FirstOrDefault();
                var environment = extractedApiKey.ToString().Split("|").Skip(1).FirstOrDefault();

                // Set the environment in HttpContext for later use for database connections
                httpContext.SetDbEnvironment(environment);

                var merchant = await merchantService.GetMerchantByApiKey(apiKey!);
                if (merchant == null)
                {
                    _logger.LogWarning("Invalid API key {XApiKeyHeader}: {ApiKey}", XApiKeyHeader, apiKey);
                    await httpContext.HandleUnauthorized(ErrorCodes.INVALID_API_KEY, $"API Key '{XApiKeyHeader}' is invalid");
                    return;
                }

                // Attach merchant object to HttpContext for use in controller/service/anywhere use HttpContext
                httpContext.SetMerchant(merchant);

                await next();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ApiKeyMiddleware");
                await httpContext.HandleException(ex);
            }
        }
    }
}
