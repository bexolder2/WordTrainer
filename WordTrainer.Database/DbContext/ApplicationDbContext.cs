using Microsoft.EntityFrameworkCore;
using WordTrainer.Models.DbModels;

namespace WordTrainer.Database
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Word> UsersWords { get; set; }

        public ApplicationDbContext() : base()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("FileName=Words.db");
            optionsBuilder.EnableSensitiveDataLogging();
        }
    }
}
