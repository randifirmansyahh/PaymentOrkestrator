using FluentValidation;
using PaymentOrkestrator.core.payin.dto;
using PaymentOrkestrator.core.payin.@interface;
using PaymentOrkestrator.data.entities;
using PaymentOrkestrator.shared.enums;
using PaymentOrkestrator.shared.helpers;

namespace PaymentOrkestrator.Payin.Adapter.Finmo.PayloadBuilder
{
    public class FinmoQRISPayloadBuilder(
        ILogger<FinmoQRISPayloadBuilder> logger
    ) : IPayinPayloadBuilder
    {
        private readonly ILogger<FinmoQRISPayloadBuilder> _logger = logger;

        public void Validate(CreatePayinDto input, MerchantModel merchant)
        {
            PayinValidator _validator = new();
            var result = _validator.Validate(input);
            if (!result.IsValid)
            {
                _logger.LogWarning("Validation failed for PayinFinmoQrisValidator: {Errors}", result.Errors);
                throw new CustomHttpException(result);
            }
        }

        public T Build<T>(CreatePayinDto input, MerchantModel merchant)
        {
            Validate(input, merchant);

            // other business logic can be added here if needed

            return (T)(object)new CreatePayinPayload
            {
                Amount = input.Amount,
                Currency = input.Currency,
                PaymentMethod = input.PaymentMethod,
                ReferenceId = $"{merchant.Id}|{Guid.NewGuid()}"
            };
        }

        public bool Supports(string currency, string paymentMethod)
        {
            return paymentMethod.Equals(PaymentMethod.QRIS.ToString(), StringComparison.OrdinalIgnoreCase) &&
                   currency.Equals(CurrencyList.IDR.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        // Payload untuk Finmo QRIS, bisa diisi sesuai dengan spesifikasi Finmo QRIS
        private class CreatePayinPayload
        {
            public decimal Amount { get; set; }
            public string Currency { get; set; } = string.Empty;
            public string PaymentMethod { get; set; } = string.Empty;
            public string ReferenceId { get; set; } = string.Empty;
        }

        private class PayinValidator : AbstractValidator<CreatePayinDto>
        {
            private readonly string[] PAYER_ID_CURRENCY_NEED = [CurrencyList.IDR.ToString(), CurrencyList.PHP.ToString()];
            private readonly string[] PAYER_ID_PAYMENT_METHODS = [PaymentMethod.VA.ToString()];

            public PayinValidator()
            {
                // PayerId validation
                RuleFor(x => x.PayerId)
                    //.Cascade(CascadeMode.Continue) // optional, kalau ingin lanjutkan Rule berikutnya meskipun ada error
                    .NotEmpty()
                    //.WithName("payer_id") // optional, karena sudah di format di CustomHttpException
                    .When(x =>
                        PAYER_ID_PAYMENT_METHODS.Contains(x.PaymentMethod) &&
                        PAYER_ID_CURRENCY_NEED.Contains(x.Currency))
                    .WithMessage(x => $"is required for payment_method {x.PaymentMethod} with currency {x.Currency}");

                RuleFor(x => x.PayerId)
                    .Must(CustomValidation.IsValidGuid)
                    .When(x => !string.IsNullOrEmpty(x.PayerId))
                    .WithMessage("must be a valid GUID format");

                // Individual validation
                RuleFor(x => x.Individual)
                    .NotEmpty()
                    .When(x => x.PaymentMethod == PaymentMethod.QRIS.ToString())
                    .WithMessage(x => $"is required if payment_method is {x.PaymentMethod}");

                // Individual.FirstName validation
                RuleFor(x => x.Individual.FirstName)
                    .NotEmpty()
                    .When(x => x.PaymentMethod == PaymentMethod.QRIS.ToString() && x.Currency == CurrencyList.IDR.ToString())
                    .WithMessage(x =>
                        $"is required for payment_method {x.PaymentMethod} with currency {x.Currency}"
                    );
            }
        }
    }
}
