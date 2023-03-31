using System.Net;

namespace AImage.SideFunctionality.Network
{
    public class ServerDownloader
    {
        public static async Task<bool> DownloadRemoteImageAsync(string ImageUrl, string LocalPath)
        {
            try
            {
                using HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(ImageUrl);

                if (response.IsSuccessStatusCode)
                {
                    using Stream stream = await response.Content.ReadAsStreamAsync();
                    string fullPath = Path.GetFullPath(LocalPath);
                    Directory.CreateDirectory(Path.GetDirectoryName(fullPath) ?? fullPath);

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
