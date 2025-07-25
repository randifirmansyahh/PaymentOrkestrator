using Microsoft.AspNetCore.Mvc;
using PaymentOrkestrator.core.payin.dto;
using PaymentOrkestrator.core.payin.service;
using PaymentOrkestrator.shared.constants;
using PaymentOrkestrator.shared.extensions;
using PaymentOrkestrator.Shared.Constants;

namespace PaymentOrkestrator.core.payin.controller
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class WebhookController(
        ILogger<WebhookController> logger,
        PayinWebhookHandlerService webhookHandlerService
    ) : ControllerBase
    {
        private readonly ILogger<WebhookController> _logger = logger;
        private readonly PayinWebhookHandlerService _webhookHandlerService = webhookHandlerService;

        [HttpPost("finmo/{env}")]
        public async Task<IActionResult> ReceiveWebhookFinmo([FromBody] CreateFinmoPayinWebhookDto payload, string env)
        {
            try
            {
                if (!PaymentConstants.IsValidEnvironment(env))
                {
                    _logger.LogError("Invalid Environment Type {Env}", env);
                    this.HandleValidation(ErrorCodes.VALIDATION_ERROR, "Invalid Environment");
                }

                HttpContext.SetDbEnvironment(env);
                await _webhookHandlerService.HandleAsync(payload);
                return this.HandleSuccess<string>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing Finmo webhook");
                return this.HandleException(ex);
            }
        }
    }
}