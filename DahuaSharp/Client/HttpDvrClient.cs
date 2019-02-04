using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DahuaSharp
{
    public class HttpDvrClient : IDisposable
    {
        HttpClient httpClient;

        public HttpDvrClient(String host, int port, String login, String password)
        {
            if (String.IsNullOrWhiteSpace(host)) throw new ArgumentException(nameof(host));
            if (login == null) throw new ArgumentNullException(nameof(login));
            if (password == null) throw new ArgumentNullException(nameof(password));

            httpClient = new HttpClient()
            {
                BaseAddress = new Uri($"http://{host}:{port}/"),
                Timeout = new TimeSpan(0, 0, 30)
            };

            var basicAuthValue = Encoding.ASCII.GetBytes($"{login}:{password}");
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(basicAuthValue));
        }

        public async Task<byte[]> GetSnapshotAsync(byte channel = 0)
        {
            try
            {
                var response = await httpClient.GetAsync($"cgi-bin/snapshot.cgi?channel={channel}");
                var responseType = response.Content.Headers.ContentType.MediaType;

                if (responseType == "image/jpeg")
                {
                    return await response.Content.ReadAsByteArrayAsync();
                }
                else if(responseType == "text/plain")
                {
                    throw new DahuaHttpException(await response.Content.ReadAsStringAsync());
                }
                else
                {
                    throw new DahuaHttpException("Response has an invalid content type.");
                }
             
            }
            catch (TaskCanceledException)
            {
                throw new DahuaHttpException("Timeout. This can be a wrong password or invalid channel.");
            }

        }

        public void Dispose()
        {
            httpClient?.Dispose();
        }
    }
}
