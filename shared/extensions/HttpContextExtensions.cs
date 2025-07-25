using Microsoft.Extensions.Primitives;
using PaymentOrkestrator.data.entities;
using PaymentOrkestrator.shared.constants;
using PaymentOrkestrator.shared.helpers;

namespace PaymentOrkestrator.shared.extensions
{
    public static class HttpContextExtensions
    {
        private const string MerchantKey = "Merchant";
        private const string DbEnvironmentKey = "DbEnvironment";

        public static MerchantModel GetMerchant(this HttpContext context)
        {
            context.Items.TryGetValue(MerchantKey, out var merchantObj);
            MerchantModel merchant = merchantObj as MerchantModel ?? throw new CustomHttpException(ErrorCodes.MERCHANT_NOT_FOUND);
            return merchant;
        }

        public static void SetMerchant(this HttpContext context, MerchantModel merchant)
        {
            context.Items[MerchantKey] = merchant ?? throw new CustomHttpException(ErrorCodes.MERCHANT_NOT_FOUND);
        }

        public static bool GetHeader(this HttpContext context, string key, out StringValues value)
        {
            if (context.Request.Headers.TryGetValue(key, out value) && !StringValues.IsNullOrEmpty(value)) return true;
            return false;
        }

        public static void SetDbEnvironment(this HttpContext context, string? environment)
        {
            context.Items[DbEnvironmentKey] = environment?.ToUpper() ?? throw new CustomHttpException(ErrorCodes.INVALID_API_KEY);
        }

        public static string GetDbEnvironment(this HttpContext context)
        {
            context.Items.TryGetValue(DbEnvironmentKey, out var envObj);
            string environment = envObj as string ?? throw new CustomHttpException(ErrorCodes.INVALID_API_KEY);
            return environment;
        }
    }
}
