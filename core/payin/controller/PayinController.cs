using Microsoft.AspNetCore.Mvc;
using PaymentOrkestrator.core.payin.dto;
using PaymentOrkestrator.core.payin.service;
using PaymentOrkestrator.shared.constants;
using PaymentOrkestrator.shared.extensions;
using PaymentOrkestrator.shared.middleware;

namespace PaymentOrkestrator.core.payin.controller
{
    [MerchantAuth]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PayinController(
        PayinService payinService,
        ILogger<PayinController> logger
    ) : ControllerBase
    {
        private readonly PayinService _payinService = payinService;
        private readonly ILogger<PayinController> _logger = logger;

        [HttpPost]
        public async Task<IActionResult> CreatePayin([FromBody] CreatePayinDto dto)
        {
            try
            {
                // Validate the DTO using Data Annotations
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for CreatePayinDto {@Dto}", dto);
                    return this.HandleValidation(ModelState);
                }

                // get merchant from HttpContext and continue processing
                var result = await _payinService.PayinAsync(dto, HttpContext.GetMerchant());
                return this.HandleSuccess(result, "Payin created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating payin");
                return this.HandleException(ex);
            }
        }

        //[AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPayin(string id)
        {
            try
            {
                if (Guid.TryParse(id, out var parsedId) == false)
                {
                    _logger.LogWarning("Invalid payin ID format: {Id}", id);
                    return this.HandleValidation(ModelState, "id", "Invalid payin ID format, must be a valid GUID");
                }

                var result = await _payinService.GetPayinByIdAsync(id);
                if (result == null) return this.HandleNotFound(ErrorCodes.PAYIN_NOT_FOUND);

                return this.HandleSuccess(result, "Payin retrieved successfully");
            }
            catch (Exception ex)
            {
                //coba Sentry
                //SentrySdk.CaptureMessage("Hello Sentry");
                //SentrySdk.CaptureException(ex);

                _logger.LogError(ex, "Error retrieving payin");
                return this.HandleException(ex);
            }
        }
    }
}
