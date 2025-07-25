using PaymentOrkestrator.core.merchant.service;
using PaymentOrkestrator.core.payin.adapter.finmo;
using PaymentOrkestrator.shared.constants;
using PaymentOrkestrator.shared.enums;
using PaymentOrkestrator.shared.helpers;
using PaymentOrkestrator.shared.interfaces;
using PaymentOrkestrator.Shared.Constants;

namespace PaymentOrkestrator.core.payin.service
{
    public class PayinGatewayResolverService
    {
        private readonly MerchantService _configService;
        private readonly FinmoAdapter _finmoAdapter;
        // private readonly LocalPaymentAdapter _localAdapter; // Uncomment jika ada adapter lain

        private readonly Dictionary<GatewayName, IPayinGatewayAdapter> _adapterMap;

        public PayinGatewayResolverService(
            MerchantService configService,
            FinmoAdapter finmoAdapter
        // LocalPaymentAdapter localAdapter
        )
        {
            _configService = configService;
            _finmoAdapter = finmoAdapter;
            // _localAdapter = localAdapter;

            _adapterMap = new Dictionary<GatewayName, IPayinGatewayAdapter>
            {
                { GatewayName.FINMO, _finmoAdapter },
                // { GatewayNames.LocalPayment, _localAdapter }
                // Tambahkan adapter lain di sini
            };
        }

        public async Task<IPayinGatewayAdapter> ResolveGatewayAsync(string currency, string method, string merchantId)
        {
            // Cek merchant allowed for payin
            _ = await _configService.IsPayinAllowedAsync(merchantId, currency, method) ?? throw new CustomHttpException(ErrorCodes.INVALID_ROUTE);

            // Ambil gateway dari mapping constants
            var gateway = PayinGatewayMapping.Resolve(currency, method) ?? throw new CustomHttpException(ErrorCodes.CURRENCY_OR_PAYMENT_METHOD_NOT_SUPPORTED);

            // Cek apakah gateway tersedia
            if (!_adapterMap.TryGetValue(gateway, out var adapter) || adapter == null)
                throw new CustomHttpException(ErrorCodes.CURRENCY_OR_PAYMENT_METHOD_NOT_AVAILABLE);

            return adapter;
        }
    }
}
