using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;
using System.Data;

namespace PaymentOrkestrator.shared.extensions
{
    public static class DatabaseExtensions
    {
        public static Query Table(this IDbConnection conn, string tableName) => new QueryFactory(conn, new MySqlCompiler()).Query(tableName);
    }
}
