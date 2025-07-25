using FluentValidation;
using PaymentOrkestrator.core.payin.dto;
using PaymentOrkestrator.core.payin.@interface;
using PaymentOrkestrator.data.entities;
using PaymentOrkestrator.shared.enums;

namespace PaymentOrkestrator.Payin.Adapter.Finmo.PayloadBuilder
{
    public class FinmoVAPayloadBuilder : IPayinPayloadBuilder
    {
        public void Validate(CreatePayinDto input, MerchantModel merchant)
        {
            return;
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
            return paymentMethod.Equals(PaymentMethod.VA.ToString()) &&
                   currency.Equals(CurrencyList.IDR.ToString());
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
            public PayinValidator()
            {
                RuleFor(x => x.PayerId)
                    .NotEmpty().WithMessage("is required");
            }
        }
    }
}
