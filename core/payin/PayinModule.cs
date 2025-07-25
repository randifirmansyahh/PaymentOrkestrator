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
            services.AddScoped<PayinController>();
            services.AddScoped<WebhookController>();

            // Daftar semua service Payin
            services.AddScoped<PayinService>();
            services.AddScoped<PayinWebhookHandlerService>();
            services.AddScoped<PayinGatewayResolverService>();

            // Finmo
            services.AddScoped<FinmoAdapter>();
            services.AddScoped<FinmoVAPayloadBuilder>();
            services.AddScoped<FinmoQRISPayloadBuilder>();
            services.AddScoped<FinmoVANormalizer>();
            services.AddScoped<FinmoQRISNormalizer>();

            // Tambah LocalPayment dsb kalau ada

            return services;
        }
    }
}
