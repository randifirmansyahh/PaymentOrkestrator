using System.Data;

namespace PaymentOrkestrator.shared.database
{
    public interface IProductionDbConnectionReadOnly
    {
        IDbConnection GetConnection();
    }

    public interface IProductionDbConnectionWrite
    {
        IDbConnection GetConnection();
    }

    public interface ISandboxDbConnectionReadOnly
    {
        IDbConnection GetConnection();
    }

    public interface ISandboxDbConnectionWrite
    {
        IDbConnection GetConnection();
    }
}
