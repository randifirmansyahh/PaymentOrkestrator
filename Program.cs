using PaymentOrkestrator.core.merchant.service;
using PaymentOrkestrator.core.payin;
using PaymentOrkestrator.data.repositories;
using PaymentOrkestrator.shared.database;
using PaymentOrkestrator.shared.extensions;
using PaymentOrkestrator.shared.middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configuration from appsettings.json (MySQL, API Key, Finmo, etc.)
var configuration = builder.Configuration;

// 1. Add Controllers, JSON Serializer, and API Behavior Options, Db Connections
builder.Services.AddControllers().AddCustomJsonOptions();

if (builder.Environment.IsDevelopment()) builder.Services.AddCustomSwagger();

// Register Db Connection
builder.Services.AddTransient<IProductionDbConnectionWrite, ProductionDbConnectionWrite>();
builder.Services.AddTransient<IProductionDbConnectionReadOnly, ProductionDbConnectionReadOnly>();
builder.Services.AddTransient<ISandboxDbConnectionWrite, SandboxDbConnectionWrite>();
builder.Services.AddTransient<ISandboxDbConnectionReadOnly, SandboxDbConnectionReadOnly>();

// Register HTTP Context Accessor, which allows access to the current HTTP context
builder.Services.AddHttpContextAccessor();

// 3. DI
// Register Repository Layer / bagusnya di jadiin satu modul tersendiri
builder.Services
    .AddScoped<PayinRepository>()
    .AddScoped<MerchantRepository>()
    .AddScoped<TerminalSettingRepository>();

// Register Flow Modul
builder.Services.AddPayinModule();

// Register Merchant Services
builder.Services.AddSingleton<MerchantService>();

// 4. CORS Policy (optional, jika perlu)
builder.Services.AddCustomCors();

// 5. Register Custom Logging with Serilog from configuration, And use Sentry for error tracking
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
);

builder.WebHost.UseCustomSentry();

var app = builder.Build();

// 1. Use Exception Handler and Swagger Development and Developer exception page
if (app.Environment.IsDevelopment()) app.UseCustomSwagger().UseDeveloperExceptionPage();
else app.UseExceptionHandler("/error");

// 2. Use HSTS (HTTP Strict Transport Security) in Production and CORS
app.UseHttpsRedirection().UseCors();

// 3. Middleware Global Exception Handler
app.UseMiddleware<ExceptionMiddleware>();

// 4. untuk logging request ke sink Serilog apapun dan untuk tracing Sentry
app.UseSerilogRequestLogging().
    UseSentryTracing();

// 5. Routing Controllers
app.MapControllers();

app.Run();
