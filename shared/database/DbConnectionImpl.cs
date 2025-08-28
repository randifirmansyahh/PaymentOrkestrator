using MySql.Data.MySqlClient;
using PaymentOrkestrator.shared.constants;
using PaymentOrkestrator.shared.helpers;
using System.Data;

namespace PaymentOrkestrator.shared.database
{
    public class ProductionDbConnectionWrite(
        ILogger<ProductionDbConnectionWrite> logger,
        IConfiguration config)
    : IProductionDbConnectionWrite
    {
        private readonly ILogger<ProductionDbConnectionWrite> _logger = logger;
        private readonly string _connStr = config.GetConnectionString("PRODUCTION_WRITE") ?? "";
        public IDbConnection GetConnection()
        {
            try
            {
                _logger.LogInformation("Creating a new MySQL connection for production write.");
                return new MySqlConnection(_connStr);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating MySQL connection for production write.");
                throw new CustomHttpException(ErrorCodes.INTERNAL_SERVER_ERROR);
            }
        }
    }

    public class ProductionDbConnectionReadOnly(
        IConfiguration config,
        ILogger<ProductionDbConnectionReadOnly> logger)
        : IProductionDbConnectionReadOnly
    {
        private readonly ILogger<ProductionDbConnectionReadOnly> _logger = logger;
        private readonly string _connStr = config.GetConnectionString("PRODUCTION_READONLY") ?? "";

        public IDbConnection GetConnection()
        {
            try
            {
                _logger.LogInformation("Creating a new MySQL connection for production read-only.");
                return new MySqlConnection(_connStr);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating MySQL connection for production read-only.");
                throw new CustomHttpException(ErrorCodes.INTERNAL_SERVER_ERROR);
            }
        }
    }

    public class SandboxDbConnectionWrite(
        IConfiguration config,
        ILogger<SandboxDbConnectionWrite> logger) : ISandboxDbConnectionWrite
    {
        private readonly ILogger<SandboxDbConnectionWrite> _logger = logger;
        private readonly string _connStr = config.GetConnectionString("SANDBOX_WRITE") ?? "";

        public IDbConnection GetConnection()
        {
            try
            {
                _logger.LogInformation("Creating a new MySQL connection for sandbox write.");
                return new MySqlConnection(_connStr);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating MySQL connection for sandbox write.");
                throw new CustomHttpException(ErrorCodes.INTERNAL_SERVER_ERROR);
            }
        }
    }

    public class SandboxDbConnectionReadOnly(
        IConfiguration config,
        ILogger<SandboxDbConnectionReadOnly> logger)
        : ISandboxDbConnectionReadOnly
    {
        private readonly ILogger<SandboxDbConnectionReadOnly> _logger = logger;
        private readonly string _connStr = config.GetConnectionString("SANDBOX_READONLY") ?? "";

        public IDbConnection GetConnection()
        {
            try
            {
                _logger.LogInformation("Creating a new MySQL connection for sandbox read-only.");
                return new MySqlConnection(_connStr);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating MySQL connection for sandbox read-only.");
                throw new CustomHttpException(ErrorCodes.INTERNAL_SERVER_ERROR);
            }
        }
    }
}
