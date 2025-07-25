using PaymentOrkestrator.core.payin.dto;

namespace PaymentOrkestrator.core.payin.@interface
{
    /// <summary>
    /// Interface untuk menormalisasi response dari payment currency menjadi response standar internal.
    /// </summary>
    public interface IPayinResponseNormalizer
    {
        /// <summary>
        /// Apakah normalizer ini mendukung currency & method tertentu.
        /// </summary>
        /// <param name="currency"></param>
        /// <param name="paymentMethod"></param>
        /// <returns></returns>
        bool Supports(string currency, string paymentMethod);

        /// <summary>
        /// Menormalisasi response mentah dari payment gateway ke format internal.
        /// </summary>
        /// <param name="rawResponse"></param>
        /// <returns></returns>
        CreatePayinResponseNormalize Normalize(string rawResponse);
    }
}
