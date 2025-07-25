using PaymentOrkestrator.data.entities;
using PaymentOrkestrator.shared.database;
using PaymentOrkestrator.shared.extensions;
using SqlKata.Execution;

namespace PaymentOrkestrator.data.repositories
{
    public class MerchantRepository(ILogger<MerchantRepository> logger, DbConnectionFactory dbFactory)
    {
        private readonly ILogger<MerchantRepository> _logger = logger;
        private readonly DbConnectionFactory _dbFactory = dbFactory;

        public async Task<MerchantModel?> GetByApiKeyAsync(string apiKey)
        {
            _logger.LogInformation("Fetching merchant by API key: {ApiKey}", apiKey);
            using var conn = _dbFactory.CreateConnectionRead();

            return await conn.Table("merchant_users")
                .Where("api_key", apiKey)
                .FirstOrDefaultAsync<MerchantModel>();
        }

        public async Task<string?> GetMerchantIdentifier(string id)
        {
            _logger.LogInformation("Fetching merchant identifier for ID: {Id}", id);
            using var conn = _dbFactory.CreateConnectionRead();

            return await conn.Table("merchant_users")
                .Select("merchant_identifier")
                .Where("id", id)
                .FirstOrDefaultAsync<string>();
        }
    }
}
