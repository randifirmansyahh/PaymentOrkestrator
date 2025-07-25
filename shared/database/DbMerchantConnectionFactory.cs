using PaymentOrkestrator.core.merchant.service;
using PaymentOrkestrator.shared.constants;
using PaymentOrkestrator.shared.helpers;
using SqlKata.Compilers;
using SqlKata.Execution;
using System.Data;

namespace PaymentOrkestrator.shared.database
{
    public class DbMerchantConnectionFactory(ILogger<DbMerchantConnectionFactory> logger, DbConnectionFactory dbFactory, MerchantService merchantService)
    {
        private readonly ILogger<DbMerchantConnectionFactory> _logger = logger;
        private readonly DbConnectionFactory _dbFactory = dbFactory;
        private readonly MerchantService _merchantService = merchantService;

        public async Task<IDbConnection> GetMerchantConnectionAsync(string merchantId, bool readOnly = false)
        {
            var merchantIdentifier = await _merchantService.GetMerchantIdentifierById(merchantId);
            if (string.IsNullOrEmpty(merchantIdentifier))
            {
                _logger.LogError("Merchant identifier not found for merchantId: {MerchantId}", merchantId);
                throw new CustomHttpException(ErrorCodes.MERCHANT_NOT_FOUND);
            }

            // get conection string for the merchant database
            _logger.LogInformation("Creating connection for merchant: {MerchantId} with identifier: {MerchantIdentifier}", merchantId, merchantIdentifier);
            var conn = _dbFactory.CreateConnection(readOnly);
            if (conn.State != ConnectionState.Open) conn.Open();
            conn.ChangeDatabase(merchantIdentifier);

            return conn;
        }

        public async Task<QueryFactory?> GetMerchantSqlKataConnectionAsync(string merchantId, bool readOnly = false)
        {
            var conn = await GetMerchantConnectionAsync(merchantId, readOnly);
            return new QueryFactory(conn, new MySqlCompiler());
        }
    }
}
