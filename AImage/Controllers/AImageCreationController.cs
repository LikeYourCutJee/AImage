using AImage.Models;
using AImage.SideFunctionality.DALLE_AI;
using AImage.SideFunctionality.Network;
using Microsoft.AspNetCore.Mvc;

namespace AImage.Controllers
{
    public class AImageCreationController : Controller
    {
        public static ImageCardModel DefaultCardModel { get; } = new ImageCardModel() { ImgUrl = "/Img/Error.jpg ", Description = "Nothing yet" };
        public static ImageCardModel DefaultErrorCardModel { get; } = new ImageCardModel() { ImgUrl = "/Img/Error.jpg", Description = "Sorry, something went wrong" };

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
            {
                FileContentResult? file = await ClientDownloader.DownloadRemoteImageAsync(ImgPath);
                if (file != null)
                    return file;
                else
                    return View("AImageCreationForm", DefaultErrorCardModel);
            }
            else
                return View("AImageCreationForm", DefaultErrorCardModel);
        }
    }
}
