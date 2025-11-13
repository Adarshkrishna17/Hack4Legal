namespace EvidenceAnalyzerAPI.Helpers
{
    public static class HttpHelper
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        
        public static async Task<string> DownloadStringAsync(string url)
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return content;
        }
    }
}
