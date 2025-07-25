using MySql.Data.MySqlClient;
using PaymentOrkestrator.shared.constants;
using PaymentOrkestrator.shared.extensions;
using PaymentOrkestrator.shared.helpers;
using PaymentOrkestrator.Shared.Constants;
using System.Data;

namespace PaymentOrkestrator.shared.database
{
    public class DbConnectionFactory(
        ILogger<DbConnectionFactory> logger,
        IConfiguration config,
        IHttpContextAccessor contextAccessor
        )
    {
        private readonly ILogger<DbConnectionFactory> _logger = logger;
        private readonly IConfiguration _config = config;
        private readonly IHttpContextAccessor _httpContextAccessor = contextAccessor;

        private const string ReadOnlyEnv = "READONLY";
        private const string WriteEnv = "WRITE";

        public IDbConnection CreateConnectionWrite() => CreateConnection();
        public IDbConnection CreateConnectionRead() => CreateConnection(true);
        public IDbConnection CreateConnection(bool readOnly = false)
        {
            if (_httpContextAccessor.HttpContext == null)
            {
                _logger.LogError("HttpContext is null, cannot determine DB environment.");
                throw new CustomHttpException(ErrorCodes.INVALID_API_KEY);
            }

            string env = _httpContextAccessor.HttpContext.GetDbEnvironment();
            if (!PaymentConstants.IsValidEnvironment(env))
            {
                _logger.LogError("Invalid environment: {Env}. Valid environments are: {ValidEnvironments}", env, string.Join(", ", PaymentConstants.ValidEnvironments));
                throw new CustomHttpException(ErrorCodes.INVALID_API_KEY);
            }

            _logger.LogInformation("Creating DB connection for environment: {Env}, ReadOnly: {ReadOnly}", env, readOnly);

            string key = $"{env}_{(readOnly ? ReadOnlyEnv : WriteEnv)}";
            var connString = _config.GetConnectionString(key);

            return new MySqlConnection(connString);
        }

        public async Task WithTransactionAsync(Func<IDbConnection, IDbTransaction, Task> action)
        {
            using var conn = CreateConnectionWrite();

            if (conn.State != ConnectionState.Open) conn.Open();
            using var tx = conn.BeginTransaction();

            try
            {
                _logger.LogInformation("Starting transaction for connection: {ConnectionId}", conn.GetHashCode());

                await action(conn, tx);
                tx.Commit();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Transaction failed for connection: {ConnectionId}, Rolling back", conn.GetHashCode());
                if (tx.Connection != null && tx.Connection.State == ConnectionState.Open) tx.Rollback();

                throw new CustomHttpException(ErrorCodes.INTERNAL_SERVER_ERROR);
            }
        }
    }
}
