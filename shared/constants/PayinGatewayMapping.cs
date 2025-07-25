using PaymentOrkestrator.shared.enums;

namespace PaymentOrkestrator.Shared.Constants
{
    public static class PayinGatewayMapping
    {
        public static readonly Dictionary<string, GatewayName> Map = new()
        {
            { "IDR:VA", GatewayName.FINMO },
            { "IDR:QRIS", GatewayName.FINMO },
            { "PHP:VA", GatewayName.LOCALPAYMENT }

            // Tambahkan mapping lain jika ada
        };

        public static GatewayName? Resolve(string currency, string paymentMethod)
        {
            var key = $"{currency}:{paymentMethod}";
            return Map.TryGetValue(key, out var gateway) ? gateway : null;
        }
    }
}
