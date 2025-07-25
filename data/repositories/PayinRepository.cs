using Dapper;
using PaymentOrkestrator.data.entities;
using PaymentOrkestrator.shared.constants;
using PaymentOrkestrator.shared.database;
using PaymentOrkestrator.shared.extensions;
using PaymentOrkestrator.shared.helpers;
using SqlKata.Execution;
using System.Data;

namespace PaymentOrkestrator.data.repositories
{
    public class PayinRepository(ILogger<PayinRepository> logger, DbConnectionFactory dbFactory, DbMerchantConnectionFactory databaseService)
    {
        private readonly ILogger<PayinRepository> _logger = logger;
        private readonly DbConnectionFactory _dbFactory = dbFactory;

        // Use For Get Merchant DB Connection
        private readonly DbMerchantConnectionFactory _databaseService = databaseService;

        private readonly string _tableName = "payins";

        public async Task<IEnumerable<PayinModel>> GetAllAsync()
        {
            using var conn = _dbFactory.CreateConnectionRead();

            var sql = "SELECT * FROM payins";
            return await conn.QueryAsync<PayinModel>(sql);
        }

        public async Task<IEnumerable<PayinModel>> SqlKataGetAllAsync()
        {
            using var conn = _dbFactory.CreateConnectionRead();
            return await conn.Table(_tableName).GetAsync<PayinModel>();
        }

        public async Task<PayinModel?> GetByIdAsync(string id)
        {
            using var conn = _dbFactory.CreateConnectionRead();

            var sql = "SELECT * FROM payins WHERE id = @id";
            return await conn.QueryFirstOrDefaultAsync<PayinModel>(sql, new { id });
        }

        public async Task<PayinModel?> SqlKataGetByIdAsync(string id)
        {
            using var conn = _dbFactory.CreateConnectionRead();
            return await conn.Table(_tableName).Where("id", id).FirstOrDefaultAsync<PayinModel>();
        }

        public async Task<int> CreateAsync(PayinModel payin)
        {
            using var conn = _dbFactory.CreateConnectionWrite();

            var sql = @"
                INSERT INTO payins (id, amount, currency, status)
                VALUES (@Id, @JourneyCode, @Amount, @Currency, @PaymentMethod, @Status, @CreatedAt)";
            return await conn.ExecuteAsync(sql, payin);
        }

        public async Task<int> SqlKataCreateAsync(PayinModel payin)
        {
            using var conn = _dbFactory.CreateConnectionWrite();

            return await conn.Table(_tableName).InsertAsync(payin);
        }

        public async Task<int> SyncPayinCreateAsync(string merchantId, PayinModel payin)
        {
            using var conn = _dbFactory.CreateConnectionWrite();
            if (conn.State != ConnectionState.Open) conn.Open();

            using var tx = conn.BeginTransaction();

            try
            {
                var sql = @"
                INSERT INTO payins (id, amount, currency, status, reference_id)
                VALUES (@Id, @Amount, @Currency, @Status, @Reference_Id)";
                int affected = await conn.ExecuteAsync(sql, payin, tx);

                // get the merchant DB connection
                using var dbMerchant = await _databaseService.GetMerchantConnectionAsync(merchantId);
                if (dbMerchant == null)
                {
                    _logger.LogError("Failed to get merchant DB connection for ID: {MerchantId}", merchantId);
                    throw new CustomHttpException(ErrorCodes.INTERNAL_SERVER_ERROR);
                }

                await dbMerchant.ExecuteAsync(sql, payin);

                tx.Commit();

                return affected;
            }
            catch (Exception ex)
            {
                // Rollback transaction on error
                tx.Rollback();

                // If it's a known custom exception, rethrow it
                if (ex is CustomHttpException) throw;

                _logger.LogError(ex, "Error syncing payin create for merchant ID: {MerchantId}", merchantId);
                throw new CustomHttpException(ErrorCodes.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<int> SyncSqlKataPayinCreateAsync(string merchantId, PayinModel payin)
        {
            using var conn = _dbFactory.CreateConnectionWrite();

            if (conn.State != ConnectionState.Open) conn.Open();
            using var tx = conn.BeginTransaction();

            try
            {
                int affected = await conn.Table(_tableName).InsertAsync(payin, tx);

                using var dbMerchant = await _databaseService.GetMerchantSqlKataConnectionAsync(merchantId);
                if (dbMerchant == null)
                {
                    _logger.LogError("Failed to get merchant DB connection for ID: {MerchantId}", merchantId);
                    throw new CustomHttpException(ErrorCodes.INTERNAL_SERVER_ERROR);
                }

                await dbMerchant.Query(_tableName).InsertAsync(payin);

                tx.Commit();

                return affected;
            }
            catch (Exception ex)
            {
                tx.Rollback();

                if (ex is CustomHttpException) throw;

                _logger.LogError(ex, "Error syncing payin create for merchant ID: {MerchantId}", merchantId);
                throw new CustomHttpException(ErrorCodes.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<int> SyncPayinUpdateAsync(string merchantId, string payinId, string status)
        {
            using var conn = _dbFactory.CreateConnectionWrite();

            if (conn.State != ConnectionState.Open) conn.Open();
            using var tx = conn.BeginTransaction();

            try
            {
                int affected = await conn.Table(_tableName)
                    .Where("id", payinId)
                    .UpdateAsync(new { status }, tx);

                if (affected == 0)
                {
                    _logger.LogWarning("No payin found with ID: {PayinId} for merchant ID: {MerchantId}", payinId, merchantId);
                    throw new CustomHttpException(ErrorCodes.PAYIN_NOT_FOUND);
                }

                using var dbMerchant = await _databaseService.GetMerchantSqlKataConnectionAsync(merchantId);
                if (dbMerchant == null)
                {
                    _logger.LogError("Failed to get merchant DB connection for ID: {MerchantId}", merchantId);
                    throw new CustomHttpException(ErrorCodes.INTERNAL_SERVER_ERROR);
                }

                await dbMerchant.Query(_tableName)
                    .Where("id", payinId)
                    .UpdateAsync(new { status }, tx);

                tx.Commit();

                return affected;
            }
            catch (Exception ex)
            {
                tx.Rollback();

                if (ex is CustomHttpException) throw;

                _logger.LogError(ex, "Error syncing payin update for merchant ID: {MerchantId}", merchantId);
                throw new CustomHttpException(ErrorCodes.INTERNAL_SERVER_ERROR);
            }
        }
    }
}
