using AImage.Models;
using AImage.SideFunctionality.DALLE_AI;
using Microsoft.AspNetCore.Mvc;

namespace AImage.Controllers
{
    public class AImageCreationController : Controller
    {
        private static ImageCardModel DefaultCardModel { get; set; } = new ImageCardModel() { ImgUrl = "", Description = "Nothing yet" };
        private static ImageCardModel DefaultErrorCardModel { get; set; } = new ImageCardModel() { ImgUrl = "/Img/Error.jpg", Description = "Sorry, something went wrong" };

        private readonly IConfiguration _configuration;
        private DalleRequests DalleRequests { get; set; }

        public AImageCreationController(IConfiguration configuration)
        {
            _configuration = configuration;
            DalleRequests = new DalleRequests(_configuration.GetValue<string>("openAiAPIKey"));
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AImageCreationForm()
        {
            return View(DefaultCardModel);
        }

        public IActionResult CreateImage(string UserDescription)
        {
            if (UserDescription != string.Empty)
            {
                var ImageUrl = DalleRequests.GenerateImageByDescription(UserDescription);
                if (ImageUrl != null)
                {
                    ImageCardModel card = new ImageCardModel()
                    {
                        Description = UserDescription,
                        ImgUrl = ImageUrl
                    };
                    return View("AImageCreationForm", card);
                }
            }
            return View("AImageCreationForm", DefaultErrorCardModel);
        }

        public async Task<IActionResult> DownloadImage(string ImgPath)
        {
            if (ImgPath != null && ImgPath != DefaultCardModel.ImgUrl && ImgPath != DefaultErrorCardModel.ImgUrl)
                using (var client = new HttpClient())
                {
                    var uri = new Uri(@ImgPath);

                    var response = await client.GetAsync(uri);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsByteArrayAsync();

                        return File(content, System.Net.Mime.MediaTypeNames.Application.Octet, "AImage.jpg");
                    }
                    else
                    {
                        return View("AImageCreationForm", DefaultErrorCardModel);
                    }
                }
            else
                return View("AImageCreationForm", DefaultErrorCardModel);
        }
    }
}
