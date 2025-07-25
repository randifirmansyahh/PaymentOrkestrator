using System.Net;

namespace PaymentOrkestrator.shared.constants
{
    public static class ErrorCodes
    {
        // route
        public static readonly ErrorCode MERCHANT_NOT_FOUND = new("1001", "Merchant not found");
        public static readonly ErrorCode INVALID_ROUTE = new("1002", "Invalid route");
        public static readonly ErrorCode CURRENCY_OR_PAYMENT_METHOD_NOT_SUPPORTED = new("1003", "Currency or payment method not supported");
        public static readonly ErrorCode CURRENCY_OR_PAYMENT_METHOD_NOT_AVAILABLE = new("1004", "Currency or payment method not available");

        // payin
        public static readonly ErrorCode PAYIN_NOT_FOUND = new("2001", "Payin not found");

        // general
        public static readonly ErrorCode INVALID_HEADER = new(((int)HttpStatusCode.BadRequest).ToString(), "Invalid header");
        public static readonly ErrorCode VALIDATION_ERROR = new(((int)HttpStatusCode.BadRequest).ToString(), "Validation error");
        public static readonly ErrorCode INTERNAL_SERVER_ERROR = new(((int)HttpStatusCode.InternalServerError).ToString(), "Internal Server Error");
        public static readonly ErrorCode MISSING_API_KEY = new(((int)HttpStatusCode.Unauthorized).ToString(), "Missing API Key");
        public static readonly ErrorCode INVALID_API_KEY = new(((int)HttpStatusCode.Unauthorized).ToString(), "Invalid API Key");

        // Tambah code lain...
    }

    public class ErrorCode(string code, string message)
    {
        public string Code { get; set; } = code;
        public string Message { get; set; } = message;
    }
}
