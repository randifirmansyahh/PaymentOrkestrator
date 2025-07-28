using Dapper;
using PaymentOrkestrator.data.entities;
using PaymentOrkestrator.shared.database;
using PaymentOrkestrator.shared.extensions;
using SqlKata.Execution;

namespace PaymentOrkestrator.data.repositories
{
    public class PayinRepository(ILogger<PayinRepository> logger,
        IHttpContextAccessor httpContextAccessor,
        IProductionDbConnectionWrite productionDbConnectionWrite,
        IProductionDbConnectionReadOnly productionDbConnectionReadOnly,
        ISandboxDbConnectionWrite sandboxDbConnectionWrite,
        ISandboxDbConnectionReadOnly sandboxDbConnectionReadOnly)
    {
        private readonly ILogger<PayinRepository> _logger = logger;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IProductionDbConnectionWrite _productionDbConnectionWrite = productionDbConnectionWrite;
        private readonly IProductionDbConnectionReadOnly _productionDbConnectionReadOnly = productionDbConnectionReadOnly;
        private readonly ISandboxDbConnectionWrite _sandboxDbConnectionWrite = sandboxDbConnectionWrite;
        private readonly ISandboxDbConnectionReadOnly _sandboxDbConnectionReadOnly = sandboxDbConnectionReadOnly;

        private readonly string _tableName = "payins";

        public async Task<IEnumerable<PayinModel>> GetAllAsync()
        {
            using var conn = _httpContextAccessor.ResolveConnectionRead(
                _productionDbConnectionReadOnly,
                _sandboxDbConnectionReadOnly
            );

            var sql = "SELECT * FROM payins";
            return await conn.QueryAsync<PayinModel>(sql);
        }

        public async Task<IEnumerable<PayinModel>> SqlKataGetAllAsync()
        {
            using var conn = _httpContextAccessor.ResolveConnectionRead(
                _productionDbConnectionReadOnly,
                _sandboxDbConnectionReadOnly
            );
            return await conn.Table(_tableName).GetAsync<PayinModel>();
        }

        public async Task<PayinModel?> GetByIdAsync(string id)
        {
            using var conn = _httpContextAccessor.ResolveConnectionRead(
                _productionDbConnectionReadOnly,
                _sandboxDbConnectionReadOnly
            );

            var sql = "SELECT * FROM payins WHERE id = @id";
            return await conn.QueryFirstOrDefaultAsync<PayinModel>(sql, new { id });
        }

        public async Task<PayinModel?> SqlKataGetByIdAsync(string id)
        {
            using var conn = _httpContextAccessor.ResolveConnectionRead(
                _productionDbConnectionReadOnly,
                _sandboxDbConnectionReadOnly
            );
            return await conn.Table(_tableName).Where("id", id).FirstOrDefaultAsync<PayinModel>();
        }

        public async Task<int> CreateAsync(PayinModel payin)
        {
            using var conn = _httpContextAccessor.ResolveConnectionWrite(
                _productionDbConnectionWrite,
                _sandboxDbConnectionWrite
            );

            var sql = @"
                INSERT INTO payins (id, amount, currency, status)
                VALUES (@Id, @JourneyCode, @Amount, @Currency, @PaymentMethod, @Status, @CreatedAt)";
            return await conn.ExecuteAsync(sql, payin);
        }

        public async Task<int> SqlKataCreateAsync(PayinModel payin)
        {
            using var conn = _httpContextAccessor.ResolveConnectionWrite(
                _productionDbConnectionWrite,
                _sandboxDbConnectionWrite
            );

            return await conn.Table(_tableName).InsertAsync(payin);
        }

        public async Task<int> PayinCreateAsync(PayinModel payin)
        {
            using var conn = _httpContextAccessor.ResolveConnectionWrite(
                _productionDbConnectionWrite,
                _sandboxDbConnectionWrite
            );

            var sql = @"
                INSERT INTO payins (id, amount, currency, status, reference_id)
                VALUES (@Id, @Amount, @Currency, @Status, @Reference_Id)";

            return await conn.ExecuteAsync(sql, payin);
        }

        public async Task<int> UpdatePayinAsync(string payinId, string status)
        {
            using var conn = _httpContextAccessor.ResolveConnectionWrite(
                _productionDbConnectionWrite,
                _sandboxDbConnectionWrite
            );

            return await conn.Table(_tableName)
                .Where("id", payinId)
                .UpdateAsync(new { status });
        }
    }
}
