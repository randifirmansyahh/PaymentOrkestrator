using PaymentOrkestrator.core.payin.dto;
using PaymentOrkestrator.core.payin.@interface;
using PaymentOrkestrator.shared.constants;
using PaymentOrkestrator.shared.enums;
using PaymentOrkestrator.shared.helpers;

namespace PaymentOrkestrator.core.payin.adapter.finmo.normalizer
{
    public class FinmoQRISNormalizer(
        ILogger<FinmoQRISNormalizer> logger
    ) : IPayinResponseNormalizer
    {
        private readonly ILogger<FinmoQRISNormalizer> _logger = logger;

        public CreatePayinResponseNormalize Normalize(string rawResponse)
        {
            if (string.IsNullOrEmpty(rawResponse))
            {
                _logger.LogError("Raw response cannot be null when normalizing FinmoQRISNormalizer.");
                throw new CustomHttpException(ErrorCodes.INTERNAL_SERVER_ERROR);
            }

            // cast rawResponse ke tipe yang sesuai
            var payinResponse = JsonConvertHelper.DeserializeObject<PayinResponseBody>(rawResponse);
            if (payinResponse == null)
            {
                _logger.LogError("Failed to deserialize raw response in FinmoQRISNormalizer.");
                throw new CustomHttpException(ErrorCodes.INTERNAL_SERVER_ERROR);
            }

            // other business logic can be added here

            return new()
            {
                Id = Guid.NewGuid().ToString(),
                Amount = payinResponse.Amount,
                Currency = payinResponse.Currency,
                Status = payinResponse.Status,
                ReferenceId = payinResponse.ReferenceId,
                PaymentMethod = payinResponse.PaymentMethod
            };
        }

        public bool Supports(string currency, string paymentMethod)
        {
            return currency.Equals(CurrencyList.IDR.ToString(), StringComparison.OrdinalIgnoreCase) &&
                   paymentMethod.Equals(PaymentMethod.QRIS.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        private class PayinResponseBody
        {
            public string? Id { get; set; }
            public decimal Amount { get; set; }
            public string? Currency { get; set; }
            public string? Status { get; set; }
            public string? ReferenceId { get; set; }
            public string? PaymentMethod { get; set; }
        }
    }
}
