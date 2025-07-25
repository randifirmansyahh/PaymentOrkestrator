using System.ComponentModel.DataAnnotations;

namespace PaymentOrkestrator.shared.helpers
{
    public class GuidValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value != null) return Guid.TryParse(value?.ToString(), out _);
            return true;
        }

        public override string FormatErrorMessage(string name)
            => $"{name} must be a valid GUID.";
    }

    public class CustomValidation
    {
        public static bool IsValidGuid(string? id) => Guid.TryParse(id, out _);
    }

    // buat class untuk validasi lainnya jika diperlukan
}
