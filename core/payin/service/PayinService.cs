using PaymentOrkestrator.core.payin.dto;
using PaymentOrkestrator.core.payin.@interface;
using PaymentOrkestrator.data.entities;
using PaymentOrkestrator.data.repositories;
using PaymentOrkestrator.shared.database;
using PaymentOrkestrator.shared.extensions;
using PaymentOrkestrator.shared.interfaces;
using SqlKata.Execution;

namespace PaymentOrkestrator.core.payin.service
{
    public class PayinService(
        PayinGatewayResolverService gatewayResolver,
        PayinRepository payinRepo,
        ILogger<PayinService> logger,
        DbConnectionFactory dbFactory
    )
    {
        private readonly PayinGatewayResolverService _gatewayResolver = gatewayResolver;
        private readonly PayinRepository _payinRepo = payinRepo;

        private readonly ILogger<PayinService> _logger = logger;
        private readonly DbConnectionFactory _dbFactory = dbFactory;
        /// <summary>
        /// Create a new payin transaction, full flow
        /// </summary>
        public async Task<CreatePayinResponseDto> PayinAsync(CreatePayinDto dto, MerchantModel merchant)
        {
            _logger.LogInformation("Processing Payin for Merchant: {Merchant}, With Request: {dto}", merchant, dto);

            // Get the gateway adapter by resolving the gateway based on currency and payment method
            IPayinGatewayAdapter gateway = await _gatewayResolver.ResolveGatewayAsync(dto.Currency, dto.PaymentMethod, merchant.Id!);

            // Get the payload builder for the specific gateway, And check if the payment method is supported
            IPayinPayloadBuilder payloadBuilder = gateway.GetPayinPayloadBuilder(dto.Currency, dto.PaymentMethod);

            // Get the response normalizer for the spesific gateway, And Check if the response is supported
            IPayinResponseNormalizer responseNormalizer = gateway.NormalizePayinResponse(dto.Currency, dto.PaymentMethod);

            // Build the payload using the DTO and merchant information, get the object payload
            object payload = payloadBuilder.Build<object>(dto, merchant);

            // Send to external gateway, send object payload and get string response
            string rawResponse = await gateway.PayinAsync(payload);

            // Get the normalized response
            CreatePayinResponseNormalize normalized = responseNormalizer.Normalize(rawResponse);

            // Save to both DB
            await _payinRepo.SyncSqlKataPayinCreateAsync(merchant.Id!, normalized.ToPayinTable());

            // Prepare response
            return normalized.ToPayinResponse();
        }

        /// <summary>
        /// Get a payin transaction by its ID
        /// </summary>
        public async Task<CreatePayinResponseDto?> GetPayinByIdAsync(string id)
        {
            _logger.LogInformation("Retrieving Payin by ID: {Id}", id);

            var payin = await _payinRepo.SqlKataGetByIdAsync(id);
            return payin?.ToPayinResponse();
        }

        public async Task<CreatePayinResponseDto> PayinAsyncWithTx(CreatePayinDto dto, MerchantModel merchant)
        {
            _logger.LogInformation("Processing Payin for Merchant: {Merchant}, With Request: {dto}", merchant, dto);

            // Get the gateway adapter by resolving the gateway based on currency and payment method
            IPayinGatewayAdapter gateway = await _gatewayResolver.ResolveGatewayAsync(dto.Currency, dto.PaymentMethod, merchant.Id!);

            // Get the payload builder for the specific gateway, And check if the payment method is supported
            IPayinPayloadBuilder payloadBuilder = gateway.GetPayinPayloadBuilder(dto.Currency, dto.PaymentMethod);

            // Get the response normalizer for the spesific gateway, And Check if the response is supported
            IPayinResponseNormalizer responseNormalizer = gateway.NormalizePayinResponse(dto.Currency, dto.PaymentMethod);

            // Build the payload using the DTO and merchant information, get the object payload
            object payload = payloadBuilder.Build<object>(dto, merchant);

            // Send to external gateway, send object payload and get string response
            string rawResponse = await gateway.PayinAsync(payload);

            // Get the normalized response
            CreatePayinResponseNormalize normalized = responseNormalizer.Normalize(rawResponse);

            await _dbFactory.WithTransactionAsync(async (c, tx) =>
            {
                await c.Table("payins").InsertAsync(normalized.ToPayinTable(), tx);
                await c.Table("logs").InsertAsync(new LogModel("payin_response_psp", rawResponse, merchant.Id), tx);
                await c.Table("logs").Where("id", "123").UpdateAsync(new { process_name = "payin_response_psp" }, tx);
                await c.Table("logs").WhereNull("process_name").DeleteAsync(tx);
            });

            // test combine write and read only connection
            //var ori = await _payinRepo.SqlKataGetByIdAsync(normalized.Id!);
            //_logger.LogInformation("Payin with ID {Id} already exists: {ori}", normalized.Id, ori?.Id);

            return normalized.ToPayinResponse();
        }
    }
}