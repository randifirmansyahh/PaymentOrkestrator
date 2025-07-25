using PaymentOrkestrator.shared.constants;
using PaymentOrkestrator.shared.extensions;

namespace PaymentOrkestrator.shared.middleware
{
    public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        private readonly ILogger<ExceptionMiddleware> _logger = logger;
        private readonly RequestDelegate _next = next;
        private readonly IHostEnvironment _env = env;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Lanjutkan pipeline ke the next middleware or controller
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");

                // for test global exception on production
                //_env.EnvironmentName = Environments.Production;

                object? errors = _env.IsDevelopment()
                    ? new { detail = ex.StackTrace }
                    : ErrorCodes.INTERNAL_SERVER_ERROR.Message;

                await context.HandleException(ex.Message, errors);
            }
        }
    }
}
