namespace PaymentOrkestrator.data.entities
{
    public class TerminalSettingModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Currency { get; set; } = string.Empty;
        public string Merchant_Id { get; set; } = string.Empty;
        public string Gateway { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
        public bool Is_Active { get; set; }
    }
}
