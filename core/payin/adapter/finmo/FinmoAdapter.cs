using PaymentOrkestrator.core.payin.adapter.finmo.normalizer;
using PaymentOrkestrator.core.payin.@interface;
using PaymentOrkestrator.Payin.Adapter.Finmo.PayloadBuilder;
using PaymentOrkestrator.shared.constants;
using PaymentOrkestrator.shared.extensions;
using PaymentOrkestrator.shared.helpers;
using PaymentOrkestrator.shared.interfaces;
using RestSharp;

namespace PaymentOrkestrator.core.payin.adapter.finmo
{
    public class FinmoAdapter(
        ILogger<FinmoAdapter> logger,
        FinmoVAPayloadBuilder vaBuilder,
        FinmoQRISPayloadBuilder qrisBuilder,
        FinmoVANormalizer vaNormalizer,
        FinmoQRISNormalizer qrisNormalizer,
        IConfiguration config
        ) : IPayinGatewayAdapter
    {
        private readonly ILogger<FinmoAdapter> _logger = logger;
        private readonly IConfiguration _config = config;

        private readonly FinmoVAPayloadBuilder _vaBuilder = vaBuilder;
        private readonly FinmoQRISPayloadBuilder _qrisBuilder = qrisBuilder;
        // Tambahkan builder lain jika ada

        private readonly FinmoVANormalizer _vaNormalizer = vaNormalizer;
        private readonly FinmoQRISNormalizer _qrisNormalizer = qrisNormalizer;
        // Tambahkan normalizer lain jika ada

        public IPayinPayloadBuilder GetPayinPayloadBuilder(string currency, string method)
        {
            var key = $"{currency}:{method}".ToUpperInvariant();

            var map = new Dictionary<string, IPayinPayloadBuilder>(StringComparer.OrdinalIgnoreCase)
            {
                { "IDR:VA", _vaBuilder },
                { "PHP:VA", _vaBuilder },
                { "IDR:QRIS", _qrisBuilder }
                // Tambahkan builder lain di sini jika ada
            };

            if (!map.TryGetValue(key, out var builder) || builder == null)
            {
                _logger.LogError("Unsupported Builder from combination of currency and payment method: {Currency}:{Method}", currency, method);
                throw new CustomHttpException(ErrorCodes.CURRENCY_OR_PAYMENT_METHOD_NOT_SUPPORTED);
            }

            if (!builder.Supports(currency, method))
            {
                _logger.LogError("Unsupported Builder from combination of currency and payment method: {Currency}:{Method}", currency, method);
                throw new CustomHttpException(ErrorCodes.CURRENCY_OR_PAYMENT_METHOD_NOT_SUPPORTED);
            }

            return builder;
        }

        public IPayinResponseNormalizer NormalizePayinResponse(string currency, string method)
        {
            var key = $"{currency}:{method}".ToUpperInvariant();

            var map = new Dictionary<string, IPayinResponseNormalizer>(StringComparer.OrdinalIgnoreCase)
            {
                { "IDR:VA", _vaNormalizer },
                { "PHP:VA", _vaNormalizer },
                { "IDR:QRIS", _qrisNormalizer }
                // Tambahkan normalizer lain di sini jika ada
            };

            if (!map.TryGetValue(key, out var normalizer) || normalizer == null)
            {
                _logger.LogError("Unavailable Normalizer from combination of currency and payment method: {Currency}:{Method}", currency, method);
                throw new CustomHttpException(ErrorCodes.CURRENCY_OR_PAYMENT_METHOD_NOT_AVAILABLE);
            }

            if (!normalizer.Supports(currency, method))
            {
                _logger.LogError("Unsupported Normalizer from combination of currency and payment method: {Currency}:{Method}", currency, method);
                throw new CustomHttpException(ErrorCodes.CURRENCY_OR_PAYMENT_METHOD_NOT_SUPPORTED);
            }

            return normalizer;
        }

        public async Task<string> PayinAsync(object payload)
        {
            _logger.LogInformation("🚀 [Finmo] Payin with payload: {@Payload}", payload);

            var (BASE_URL, PATH, TOKEN) = (
                _config.GetSection("Finmo:BaseUrl").Value,
                "v1/e920f83c-5480-41e1-b5f0-004fb0ae9fb3",
                _config.GetSection("Finmo:ApiKey").Value
            );

            var client = new RestClient(BASE_URL ?? "");
            var response = await client.CreateGetAsync(PATH, new Dictionary<string, string>()
            {
                { "Content-Type", "application/json" },
                { "Authorization", $"Bearer {TOKEN}" }
            });
            if (!response.IsSuccessful)
            {
                _logger.LogError("Failed to call Finmo Payin API: {StatusCode} - {ErrorMessage} - {Content}", response.StatusCode, response.ErrorMessage, response.Content);
                throw new CustomHttpException(ErrorCodes.INTERNAL_SERVER_ERROR);
            }

            _logger.LogInformation("Finmo Payin API response: {ResponseContent}", response.Content);

            return response.Content ?? string.Empty;
        }

        //{
        //  "currency": "IDR",
        //  "amount": 150000,
        //  "status": "PENDING",
        //  "reference_id": "ref-finmo-2d6b4e70-56b8-4de9-8b97-909bb85e76d7",
        //  "payment_method": "VA",
        //  "pay_id": "9f81d6ee-0a2c-4a1f-bc2c-2b3a5305c1bb"
        //}
    }
}
