using System.Net;

namespace AImage.SideFunctionality.Network
{
    public class Downloader
    {
        public static async Task<bool> DownloadImageAsync(string imageUrl, string relativePath)
        {
            try
            {
                using HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(imageUrl);

                if (response.IsSuccessStatusCode)
                {
                    using Stream stream = await response.Content.ReadAsStreamAsync();
                    string fullPath = Path.GetFullPath(relativePath);
                    Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                    using FileStream fileStream = File.Create(fullPath);
                    await stream.CopyToAsync(fileStream);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
