using System.Reflection;

namespace PaymentOrkestrator.shared.helpers
{
    public static class QueryDataHelper
    {
        // USE
        //string[] columns = ["id", "process_name", "data", "merchant_id"];
        //var values = QueryDataHelper.GenerateBulkDataForInsert<LogModel>(
        //    [
        //        new("payin_request_payload", JsonSerializer.Serialize(payload), merchant.Id),
        //                new("payin_response_raw", rawResponse, merchant.Id),
        //                new("payin_response_normalized", JsonSerializer.Serialize(normalized), merchant.Id),
        //            ]
        //);

        public static List<object[]> GenerateBulkDataForInsert<T>(
            IEnumerable<T> datas,
            params string[] ignoreProperties
        ) where T : class
        {
            // Dapatkan semua properti dari tipe model T
            // Filter properti yang ingin diabaikan (misal: properti Id jika auto-increment)
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                      .Where(p => !ignoreProperties.Contains(p.Name, StringComparer.OrdinalIgnoreCase) && p.CanRead)
                                      .ToArray();

            // Dapatkan nama-nama kolom dari properti yang terpilih
            // Kita bisa asumsikan nama properti sama dengan nama kolom di database
            // Jika nama kolom di database berbeda, Anda perlu mekanisme mapping tambahan (misal: atribut kustom)
            var columns = properties.Select(p => p.Name).ToArray();

            var dataRows = new List<object[]>();

            // Iterasi setiap model dalam daftar
            foreach (var data in datas)
            {
                var rowValues = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    // Dapatkan nilai properti dari model saat ini
                    rowValues[i] = properties[i].GetValue(data) ?? DBNull.Value;
                }
                dataRows.Add(rowValues);
            }

            return dataRows;
        }
    }
}
