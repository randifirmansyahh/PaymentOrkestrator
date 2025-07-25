using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PaymentOrkestrator.shared.constants;
using System.Net;

namespace PaymentOrkestrator.shared.helpers
{
    public class CustomHttpSuccess<T>(
        T? data,
        string message = "Success"
    )
    {
        public bool Status { get; set; } = true;
        public string Message { get; set; } = message;
        public T? Data { get; set; } = data;
    }

    public class CustomHttpException : Exception
    {
        public string? Code { get; set; }
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.BadRequest;
        public object? Errors { get; set; }

        // use for ModelState validation
        public CustomHttpException(ModelStateDictionary keyValuePairs)
            : base(ErrorCodes.VALIDATION_ERROR.Message)
        {
            Code = ErrorCodes.VALIDATION_ERROR.Code;
            Errors = keyValuePairs.FormatValidationErrors();
        }

        public CustomHttpException(ValidationResult validationResult)
            : base(ErrorCodes.VALIDATION_ERROR.Message)
        {
            Code = ErrorCodes.VALIDATION_ERROR.Code;
            Errors = validationResult.FormatValidationErrors();
        }

        // use for handled error with ErrorCodeResponse
        public CustomHttpException(ErrorCode error, HttpStatusCode? statusCode = HttpStatusCode.BadRequest)
            : base(error.Message)
        {
            Code = error.Code;
            StatusCode = statusCode!.Value;
        }

        public CustomHttpException(ErrorCode error, string? message)
            : base(message ?? error.Message)
        {
            Code = error.Code;
        }

        // use for handled error with custom message and custom errors
        public CustomHttpException(string? message, object? errors)
            : base(message)
        {
            Code = ErrorCodes.INTERNAL_SERVER_ERROR.Code;
            Errors = errors;
        }

        // use for generate general error response
        public object GetBody()
        {
            var dict = new Dictionary<string, object?>
            {
                { "status", false },
                { "message", Message }
            };

            if (Code != null)
                dict["code"] = Code;

            if (Errors != null)
                dict["errors"] = Errors;

            return dict;
        }
    }
}
