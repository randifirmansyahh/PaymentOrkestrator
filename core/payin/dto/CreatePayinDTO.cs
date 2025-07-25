using PaymentOrkestrator.data.entities;
using System.ComponentModel.DataAnnotations;

namespace PaymentOrkestrator.core.payin.dto
{
    // Master DTO for creating a payin transaction
    // insert validation if that property has same required/validate on all payment gateways
    // if not, you can handle it in each payment gateway adapter
    public class CreatePayinDto
    {
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(16, ErrorMessage = "Payment method max length is 16 characters.")]
        public string PaymentMethod { get; set; } = string.Empty;

        [Required]
        [StringLength(8, ErrorMessage = "Currency max length is 8 characters.")]
        public string Currency { get; set; } = string.Empty;

        //[GuidValidation] just for example, you can use custom validation attribute
        public string? PayerId { get; set; }

        public Individual Individual { get; set; } = new();

        //[AllowNull] // AllowNull is used to indicate that AdditionalInfo can be null
        public AdditionalInfo? AdditionalInfo { get; set; }
    }

    public class Individual
    {
        [Required(AllowEmptyStrings = false)]
        public string FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; }
    }

    public class AdditionalInfo
    {
        public string? OrderReference { get; set; }
        public Individual Individual { get; set; } = new();
    }

    /// <summary>
    /// Response DTO for creating a payin transaction
    /// </summary>
    public class CreatePayinResponseDto
    {
        public string? Id { get; set; }
        public decimal Amount { get; set; }
        public string? Currency { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Status { get; set; }

        // Optional for all gateways
        public string? ReferenceId { get; set; }

        // Required for some gateways
        public string? PayId { get; set; }
    }

    /// <summary>
    /// Class to normalize response from payment gateway.
    /// </summary>
    public class CreatePayinResponseNormalize()
    {
        public string? Id { get; set; }
        public decimal? Amount { get; set; }
        public string? Currency { get; set; }
        public string? PaymentMethod { get; set; }
        public string? PayId { get; set; }
        public string? ReferenceId { get; set; }
        public string? Status { get; set; }

        /// <summary>
        /// Converts to CreatePayinResponseDto (for returning consistent response format)
        /// </summary>
        public CreatePayinResponseDto ToPayinResponse()
        {
            return new()
            {
                Id = Id,
                Amount = Amount ?? 0,
                Currency = Currency,
                PaymentMethod = PaymentMethod,
                ReferenceId = ReferenceId,
                Status = Status,
                PayId = PayId
            };
        }

        /// <summary>
        /// Converts to Payin entity (for saving to DB)
        /// </summary>
        public PayinModel ToPayinTable()
        {
            return new()
            {
                Id = Id,
                Amount = Amount ?? 0,
                Currency = Currency,
                Status = Status,
                Reference_Id = ReferenceId,
            };
        }
    }

    /// <summary>
    /// Class to handle webhook response from payment gateway.
    /// </summary>
    public class CreateFinmoPayinWebhookDto
    {
        public string Id { get; set; } = string.Empty;
        public decimal? Amount { get; set; }
        public string? Currency { get; set; }
        public string? PaymentMethod { get; set; }
        public string Status { get; set; } = string.Empty;
        public string ReferenceId { get; set; } = string.Empty;
        public string? PayId { get; set; }

        public string GetMerchantId()
        {
            return ReferenceId?.Split('|').First() ?? string.Empty;
        }
    }
}