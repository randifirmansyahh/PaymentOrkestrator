using PaymentOrkestrator.data.entities;
using PaymentOrkestrator.data.repositories;

namespace PaymentOrkestrator.core.merchant.service
{
    public class MerchantService(
        ILogger<MerchantService> logger,
        MerchantRepository merchantRepo,
        TerminalSettingRepository terminalSettingRepo
    )
    {
        private readonly ILogger<MerchantService> _logger = logger;
        private readonly MerchantRepository _merchantRepo = merchantRepo;
        private readonly TerminalSettingRepository _terminalSettingRepo = terminalSettingRepo;

        public async Task<MerchantModel?> GetMerchantByApiKey(string apiKey)
        {
            return await _merchantRepo.GetByApiKeyAsync(apiKey);
        }

        public async Task<string?> GetMerchantIdentifierById(string id)
        {
            return await _merchantRepo.GetMerchantIdentifier(id);
        }

        public async Task<TerminalSettingModel?> IsPayinAllowedAsync(string merchantId, string currency, string paymentMethod)
        {
            return await _terminalSettingRepo.SqlKataPayinAllowedAsync(merchantId, currency, paymentMethod);
        }
    }
}
