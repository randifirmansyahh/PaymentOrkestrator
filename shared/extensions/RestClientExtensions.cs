using RestSharp;

namespace PaymentOrkestrator.shared.extensions
{
    public static class RestClientExtensions
    {
        public static async Task<RestResponse> CreatePostAsync(
            this RestClient restClient,
            string path,
            Dictionary<string, string>? headers = null,
            object? body = null,
            Dictionary<string, string>? queryParams = null,
            ContentType? contentType = null)
        {
            var request = new RestRequest(path, Method.Post);

            if (body != null) request.AddBody(body, contentType ?? ContentType.Json);
            if (headers != null) request.AddHeaders(headers);
            queryParams?.ToList().ForEach(kv => request.AddQueryParameter(kv.Key, kv.Value));

            return await restClient.ExecuteAsync(request);
        }

        public static async Task<RestResponse> CreateGetAsync(
            this RestClient restClient,
            string path,
            Dictionary<string, string>? headers = null,
            Dictionary<string, string>? queryParams = null,
            ContentType? contentType = null)
        {
            var request = new RestRequest(path, Method.Get);

            if (headers != null) request.AddHeaders(headers);
            if (contentType != null) request.AddHeader("Content-Type", contentType.ToString());
            queryParams?.ToList().ForEach(kv => request.AddQueryParameter(kv.Key, kv.Value));

            return await restClient.ExecuteAsync(request);
        }
    }
}
