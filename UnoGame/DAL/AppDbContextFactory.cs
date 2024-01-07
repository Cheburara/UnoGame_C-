using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace UnoGame.DAL
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlite("Data Source=C:\\Users\\arina\\RiderProjects\\UNO\\UnoGame\\DAL\\UnoGameDatabase.db");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}