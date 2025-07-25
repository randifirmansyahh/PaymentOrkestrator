using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PaymentOrkestrator.shared.helpers
{
    public class JsonConvertHelper
    {
        public static T? DeserializeObject<T>(string value, JsonSerializerSettings? settings = null)
        {
            if (string.IsNullOrEmpty(value)) return default;

            settings ??= new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            };

            return JsonConvert.DeserializeObject<T>(value, settings);
        }
    }
}
