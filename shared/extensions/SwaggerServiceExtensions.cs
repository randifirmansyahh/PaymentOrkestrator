using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;

namespace PaymentOrkestrator.shared.extensions
{
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
        {
            // Untuk explore semua endpoint
            services.AddEndpointsApiExplorer();

            // Untuk generate Swagger UI
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Payment API",
                    Version = "v1"
                });

                // Menambahkan API Key untuk Swagger
                c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    Description = "API Key required. Example: \"x-api-key: {your key}\"",
                    In = ParameterLocation.Header,
                    Name = "x-api-key",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "ApiKeyScheme"
                });

                // Menambahkan security requirement untuk semua endpoint
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "ApiKey"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            return services;
        }
    }

    public static class SwaggerAppExtensions
    {
        public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app)
        {
            // Expose /swagger/v1/swagger.json
            app.UseSwagger(
                options =>
                {
                    options.OpenApiVersion = OpenApiSpecVersion.OpenApi2_0;
                });

            // Tampilkan Swagger UI di /swagger
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Payment API v1");
                options.RoutePrefix = "swagger"; // akses di /swagger
            });

            return app;
        }
    }
}
