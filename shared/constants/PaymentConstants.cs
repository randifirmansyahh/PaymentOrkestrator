using PaymentOrkestrator.shared.enums;

namespace PaymentOrkestrator.Shared.Constants
{
    public static class PaymentConstants
    {
        public static readonly string[]
            SupportedCurrencies = [
                CurrencyList.IDR.ToString(),
                CurrencyList.PHP.ToString()

                // Tambahkan mata uang lain jika ada
            ];

        public static readonly string[] SupportedPayinMethods = [
                PaymentMethod.VA.ToString(),
                PaymentMethod.QRIS.ToString()

                // Tambahkan metode pembayaran lain jika ada
            ];

        public static readonly string ProductionEnvironment = "PRODUCTION";
        public static readonly string SandboxEnvironment = "SANDBOX";

        public static readonly string[] ValidEnvironments = [ProductionEnvironment, SandboxEnvironment];

        public static bool IsValidEnvironment(string environment) => ValidEnvironments.Contains(environment.ToUpperInvariant());
    }
}
