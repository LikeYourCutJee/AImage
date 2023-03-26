using AImage.Data;
using AImage.Models;
using AImage.SideFunctionality.Network;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace AImage.Controllers
{
    public class AImageDbController : Controller
    {
        private DataContext DbContext { get; set; }
        public string RootDirectory { get; set; } = "wwwroot";
        public string RelativeStorageDirectoryPath { get; set; } = "/Img/";
        public AImageDbController()
        {
            DbContext = new DataContext();
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AImageDb()
        {
            return View(DbContext.ImageCards.ToList());
        }
        public IActionResult RemoveImgCardFromDb(int ImageId)
        {
            RemoveCardById(ImageId);

            return View("AImageDB", DbContext.ImageCards.ToList());
        }
        private bool RemoveCardById(int ImageId)
        {
            var ImageCard = DbContext.ImageCards.FirstOrDefault(img => img.Id == ImageId);

            if (ImageCard != null)
            {

                string absolutePath = Path.GetFullPath(RootDirectory + ImageCard.ImgUrl);

                // Проверьте, существует ли файл
                if (System.IO.File.Exists(absolutePath))
                {
                    // Удалите файл
                    System.IO.File.Delete(absolutePath);
                }

                DbContext.ImageCards.Remove(ImageCard);
                DbContext.SaveChanges();

                return true;
            }

            return false;
        }
        public async Task<IActionResult> AddImgCardtoDb(ImageCardModel UserCard)
        {
            if (await AddCardToDb(UserCard))
                return View("AImageDb", DbContext.ImageCards.ToList());
            else
                return View("~/Views/AImageCreation/AImageCreationForm.cshtml", AImageCreationController.DefaultErrorCardModel);
        }
        private async Task<bool> AddCardToDb(ImageCardModel UserCard)
        {
            if (ModelState.IsValid)
            {
                var UniqueFileId = UserCard.GetHashCode() + ".jpg";
                var relativePath = RootDirectory + RelativeStorageDirectoryPath + UniqueFileId;
                if (await Downloader.DownloadImageAsync(UserCard.ImgUrl, relativePath))
                {
                    UserCard.ImgUrl = RelativeStorageDirectoryPath + UniqueFileId;
                    DbContext.ImageCards.Add(UserCard);
                    DbContext.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }
    }
}
