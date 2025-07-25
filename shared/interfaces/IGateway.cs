using PaymentOrkestrator.core.payin.@interface;

namespace PaymentOrkestrator.shared.interfaces
{
    public interface IPayinGatewayAdapter
    {
        IPayinPayloadBuilder GetPayinPayloadBuilder(string currency, string paymentMethod);
        IPayinResponseNormalizer NormalizePayinResponse(string currency, string paymentMethod);
        Task<string> PayinAsync(object payload);
    }

    // You can add more interfaces here if needed
}
