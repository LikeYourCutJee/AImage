using AImage.Models;
using Microsoft.EntityFrameworkCore;

namespace AImage.Data
{
    public class DataContext : DbContext
    {
        public DbSet<ImageCardModel> ImageCards { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlite(@"Data source=Data/Db`s/Images.db");
    }
}
