using System.Linq;
using System.Net;

namespace WebParser
{
    public class WebParcer
    {
        private readonly HttpClient _httpClient;

        public WebParcer()
        {
            var handler = new HttpClientHandler
            {
                UseCookies = true,
                CookieContainer = new CookieContainer(),
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            _httpClient = new HttpClient(handler);

            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 " +
                "(KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36");
        }

        public async Task<string[]> DownloadPagesAsync(List<string> urls)
        {
            var tasks = urls.Select(url => _httpClient.GetStringAsync(url)).ToList();
            return await Task.WhenAll(tasks);
        }
    }

    public class Program
    {
        public static async Task Main()
        {
            var urls = new List<string>
            {
                "https://example.com",
                "https://metanit.com/sharp/tutorial/13.3.php?ysclid=mbj9merzpx518927599"
            };

            var downloader = new WebParcer();
            string[] pages = await downloader.DownloadPagesAsync(urls);

            for (int i = 0; i < pages.Length; i++)
            {
                Console.WriteLine($"Адрес {i + 1}");
                Console.WriteLine(pages[i]);
            }
        }
    }
}
