using PaymentOrkestrator.core.payin.dto;
using PaymentOrkestrator.data.entities;

namespace PaymentOrkestrator.core.payin.@interface
{
    /// <summary>
    /// Interface untuk membangun payload payment gateway (Finmo/LocalPayment, QRIS/VA, dll)
    /// </summary>
    public interface IPayinPayloadBuilder
    {
        /// <summary>
        /// Validasi input untuk memastikan semua data yang diperlukan sudah ada dan valid.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="merchant"></param>
        /// <returns></returns>
        void Validate(CreatePayinDto input, MerchantModel merchant);
        /// <summary>
        /// Apakah payload builder ini mendukung method tertentu (misal QRIS/VA).
        /// </summary>
        /// <param name="paymentMethod"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        bool Supports(string currency, string paymentMethod);

        /// <summary>
        /// Membuat payload untuk dikirim ke payment gateway.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="merchant"></param>
        /// <returns></returns>
        T Build<T>(CreatePayinDto input, MerchantModel merchant);

        // need private class for validation abstract validator
        // need private class for payload
    }
}
