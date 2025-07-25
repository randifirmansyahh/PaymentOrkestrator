using System.Text.Json;

namespace PaymentOrkestrator.shared.extensions
{
    public static class JsonOptionsExtensions
    {
        public static IMvcBuilder AddCustomJsonOptions(this IMvcBuilder builder)
        {
            builder.AddJsonOptions(o =>
            {
                // Property key jadi snake_case otomatis
                o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;

                // Hilangkan property null dari response (aktifkan jika perlu)
                o.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            });

            builder.ConfigureApiBehaviorOptions(options =>
            {
                // supaya masuk ke controller meskipun kena validasi
                options.SuppressModelStateInvalidFilter = true;
            });

            return builder;
        }
    }
}
