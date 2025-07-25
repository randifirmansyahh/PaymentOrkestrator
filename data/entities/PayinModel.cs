using PaymentOrkestrator.core.payin.dto;

namespace PaymentOrkestrator.data.entities
{
    public class PayinModel
    {
        public string? Id { get; set; }
        public decimal Amount { get; set; }
        public string? Currency { get; set; }
        public string? Status { get; set; }
        public string? Reference_Id { get; set; }

        /// <summary>
        /// Converts the PayinModel to a CreatePayinResponseDto.
        /// </summary>
        public CreatePayinResponseDto ToPayinResponse()
        {
            return new CreatePayinResponseDto
            {
                Id = Id,
                Amount = Amount,
                Currency = Currency,
                Status = Status,
                ReferenceId = Reference_Id
            };
        }
    }
}
