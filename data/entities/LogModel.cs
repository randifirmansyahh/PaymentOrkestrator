namespace PaymentOrkestrator.data.entities
{
    public class LogModel(
        string? processName,
        string? data,
        string? merchantId
    )
    {
        public string? Id { get; set; } = Guid.NewGuid().ToString();
        public string? Merchant_Id { get; set; } = merchantId;
        public string? Process_Name { get; set; } = processName;
        public string? Data { get; set; } = data;
    }
}
