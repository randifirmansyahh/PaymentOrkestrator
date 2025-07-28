using PaymentOrkestrator.shared.constants;
using PaymentOrkestrator.shared.helpers;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;
using System.Data;

namespace PaymentOrkestrator.shared.extensions
{
    public static class DatabaseExtensions
    {
        public static Query Table(this IDbConnection conn, string tableName) => new QueryFactory(conn, new MySqlCompiler()).Query(tableName);

        public static async Task WithTransactionAsync(this IDbConnection conn, Func<IDbConnection, IDbTransaction, Task> action, ILogger logger)
        {
            if (conn.State != ConnectionState.Open) conn.Open();
            using var tx = conn.BeginTransaction();

            try
            {
                logger.LogInformation("Starting transaction for connection: {ConnectionId}", conn.GetHashCode());

                await action(conn, tx);
                tx.Commit();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Transaction failed for connection: {ConnectionId}, Rolling back", conn.GetHashCode());
                if (tx.Connection != null && tx.Connection.State == ConnectionState.Open) tx.Rollback();

                throw new CustomHttpException(ErrorCodes.INTERNAL_SERVER_ERROR);
            }
        }
    }
}
