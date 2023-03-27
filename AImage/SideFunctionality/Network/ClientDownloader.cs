using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;

namespace AImage.SideFunctionality.Network
{
    public class ClientDownloader
    {
        public static async Task<FileContentResult?> DownloadRemoteImageAsync(string ImgPath) 
        {
            using (var client = new HttpClient())
            {
                var uri = new Uri(@ImgPath);

                var response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsByteArrayAsync();

                    return new FileContentResult(content, System.Net.Mime.MediaTypeNames.Application.Octet) {FileDownloadName =  "AImage.jpg" };
                }
                else
                {
                    return null;
                }
            }
        }

        public static async Task<FileContentResult?> DownloadLocalImageAsync(string ImgPath)
        {
            if (!System.IO.File.Exists(ImgPath))
            {
                return null;
            }

            var fileBytes = await System.IO.File.ReadAllBytesAsync(ImgPath);
            return new FileContentResult(fileBytes, System.Net.Mime.MediaTypeNames.Image.Jpeg) { FileDownloadName =  "AImage.jpg" };
        }
    }
}
