using PaymentOrkestrator.core.payin.dto;
using PaymentOrkestrator.data.repositories;

namespace PaymentOrkestrator.core.payin.service
{
    /// <summary>
    /// Handler untuk memproses webhook dari Finmo/payment gateway lain
    /// </summary>
    public class PayinWebhookHandlerService(
        ILogger<PayinWebhookHandlerService> logger,
        PayinRepository payinRepository
    )
    {
        private readonly ILogger<PayinWebhookHandlerService> _logger = logger;
        private readonly PayinRepository _payinRepository = payinRepository;

        public async Task HandleAsync(CreateFinmoPayinWebhookDto payload)
        {
            _logger.LogInformation("Handling Finmo Payin Webhook for Merchant: {MerchantId}, With Request: {payload}", payload.GetMerchantId(), payload);

            await _payinRepository.UpdatePayinAsync(payload.Id, payload.Status);
        }
    }
}
