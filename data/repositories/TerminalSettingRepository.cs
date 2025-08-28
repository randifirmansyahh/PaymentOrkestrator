using Dapper;
using PaymentOrkestrator.data.entities;
using PaymentOrkestrator.shared.database;
using PaymentOrkestrator.shared.extensions;
using SqlKata.Execution;

namespace PaymentOrkestrator.data.repositories
{
    public class TerminalSettingRepository(ILogger<TerminalSettingRepository> logger,
        IHttpContextAccessor httpContextAccessor,
        IProductionDbConnectionWrite productionDbConnectionWrite,
        IProductionDbConnectionReadOnly productionDbConnectionReadOnly,
        ISandboxDbConnectionWrite sandboxDbConnectionWrite,
        ISandboxDbConnectionReadOnly sandboxDbConnectionReadOnly)
    {
        private readonly ILogger<TerminalSettingRepository> _logger = logger;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IProductionDbConnectionWrite _productionDbConnectionWrite = productionDbConnectionWrite;
        private readonly IProductionDbConnectionReadOnly _productionDbConnectionReadOnly = productionDbConnectionReadOnly;
        private readonly ISandboxDbConnectionWrite _sandboxDbConnectionWrite = sandboxDbConnectionWrite;
        private readonly ISandboxDbConnectionReadOnly _sandboxDbConnectionReadOnly = sandboxDbConnectionReadOnly;

        private const string TableName = "terminal_settings";

        public async Task<TerminalSettingModel?> IsPayinAllowedAsync(string merchantId, string currency, string paymentMethod)
        {
            var sql = @"
                SELECT * FROM terminal_settings
                WHERE merchant_id = @merchantId
                AND currency = @currency
                AND method = @paymentMethod";

            using var conn = _httpContextAccessor.ResolveConnectionRead(
                _productionDbConnectionReadOnly,
                _sandboxDbConnectionReadOnly
            );
            return await conn.QueryFirstOrDefaultAsync<TerminalSettingModel>(sql, new
            {
                merchantId,
                currency,
                paymentMethod
            });
        }

        public async Task<TerminalSettingModel?> SqlKataPayinAllowedAsync(string merchantId, string currency, string paymentMethod)
        {
            using var conn = _httpContextAccessor.ResolveConnectionRead(
                _productionDbConnectionReadOnly,
                _sandboxDbConnectionReadOnly
            );

            return await conn.Table(TableName)
                .Where(new { merchant_id = merchantId, currency, method = paymentMethod })
                .FirstOrDefaultAsync<TerminalSettingModel>();

            //return await _db.Query(TableName)
            //    .Where("merchant_id", merchantId)
            //    .Where("currency", currency)
            //    .Where("method", paymentMethod)
            //    .FirstOrDefaultAsync<TerminalSettingModel>();
        }
    }
}
