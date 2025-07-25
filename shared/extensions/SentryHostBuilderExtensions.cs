namespace PaymentOrkestrator.shared.extensions
{
    public static class SentryHostBuilderExtensions
    {
        public static IWebHostBuilder UseCustomSentry(this IWebHostBuilder builder)
        {
            return builder.UseSentry(options =>
            {
                options.SendDefaultPii = true; // Tambah detail PII (IP, header, dsb.)
                options.SetBeforeSend((@event, hint) =>
                {
                    // Jangan kirim server name
                    @event.ServerName = null;
                    return @event;
                });
                options.Debug = true; // Mode debug
                options.TracesSampleRate = 1.0; // Sampling 100% traces
            });
        }
    }
}
