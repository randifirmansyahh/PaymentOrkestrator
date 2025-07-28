using PaymentOrkestrator.core.payin.adapter.finmo;
using PaymentOrkestrator.core.payin.adapter.finmo.normalizer;
using PaymentOrkestrator.core.payin.controller;
using PaymentOrkestrator.core.payin.service;
using PaymentOrkestrator.Payin.Adapter.Finmo.PayloadBuilder;

namespace PaymentOrkestrator.core.payin
{
    public static class PayinModule
    {
        public static IServiceCollection AddPayinModule(this IServiceCollection services)
        {
            // Daftar semua controller Payin
            services.AddSingleton<PayinController>();
            services.AddSingleton<WebhookController>();

            // Daftar semua service Payin
            services.AddSingleton<PayinService>();
            services.AddSingleton<PayinWebhookHandlerService>();
            services.AddSingleton<PayinGatewayResolverService>();

            // Finmo
            services.AddSingleton<FinmoAdapter>();
            services.AddSingleton<FinmoVAPayloadBuilder>();
            services.AddSingleton<FinmoQRISPayloadBuilder>();
            services.AddSingleton<FinmoVANormalizer>();
            services.AddSingleton<FinmoQRISNormalizer>();

            // Tambah LocalPayment dsb kalau ada

            return services;
        }
    }
}
