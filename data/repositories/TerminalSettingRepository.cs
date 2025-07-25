using Dapper;
using PaymentOrkestrator.data.entities;
using PaymentOrkestrator.shared.database;
using PaymentOrkestrator.shared.extensions;
using SqlKata.Execution;

namespace PaymentOrkestrator.data.repositories
{
    public class TerminalSettingRepository(DbConnectionFactory dbConnectionFactory)
    {
        private readonly DbConnectionFactory _dbFactory = dbConnectionFactory;
        private const string TableName = "terminal_settings";

        public async Task<TerminalSettingModel?> IsPayinAllowedAsync(string merchantId, string currency, string paymentMethod)
        {
            var sql = @"
                SELECT * FROM terminal_settings
                WHERE merchant_id = @merchantId
                AND currency = @currency
                AND method = @paymentMethod";

            using var conn = _dbFactory.CreateConnectionRead();
            return await conn.QueryFirstOrDefaultAsync<TerminalSettingModel>(sql, new
            {
                merchantId,
                currency,
                paymentMethod
            });
        }

        public async Task<TerminalSettingModel?> SqlKataPayinAllowedAsync(string merchantId, string currency, string paymentMethod)
        {
            using var conn = _dbFactory.CreateConnectionRead();

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
