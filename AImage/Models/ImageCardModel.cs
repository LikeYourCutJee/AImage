using System.ComponentModel.DataAnnotations;

namespace AImage.Models
{
    public class ImageCardModel
    {
        [Key]
        public int Id { get; set; }
        public string ImgUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreationTime { get; set; } = DateTime.Now;
    }
}
