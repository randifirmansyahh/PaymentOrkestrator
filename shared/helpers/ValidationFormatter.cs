using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text;

namespace PaymentOrkestrator.shared.helpers
{
    public static class ValidationFormatter
    {
        public static Dictionary<string, string> FormatValidationErrors(this ModelStateDictionary modelState)
        {
            return modelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => ToSnakeDotCase(kvp.Key),
                    kvp =>
                    {
                        var snakeKey = ToSnakeDotCase(kvp.Key);
                        var propName = kvp.Key.Split('.').Last();
                        var snakeProp = snakeKey.Split('.').Last();
                        // Replace only if propName found in error message
                        return string.Join(" | ", kvp.Value!.Errors.Select(e =>
                            e.ErrorMessage.Replace(propName, snakeProp)
                        ));
                    }
                );
        }

        public static IDictionary<string, string> FormatValidationErrors(this ValidationResult result)
        => result.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => ToSnakeDotCase(g.Key),
                g => g.First().ErrorMessage // Ambil satu pesan saja
            );

        // "Individual.FirstName" => "individual.first_name"
        public static string ToSnakeDotCase(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return string.Join(".", input.Split('.').Select(ToSnakeCase));
        }

        public static string ToSnakeCase(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            var sb = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                var c = input[i];
                if (char.IsUpper(c))
                {
                    if (i > 0) sb.Append('_');
                    sb.Append(char.ToLowerInvariant(c));
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}