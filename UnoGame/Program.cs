using System;
using Microsoft.EntityFrameworkCore;
using UnoGame.GameMenu;
using UnoGame.DAL;
using UnoGame.DAL.Entity;

namespace UnoGame
{
    class Program
    {
        
        static void Main(string[] args)
        {
            string dbDirectory = "C:\\Users\\arina\\RiderProjects\\UNO\\UnoGame\\DAL";

            // Combine the directory and file name to create a complete path for the database file
            string dbFilePath = Path.Combine(dbDirectory, "UnoGameDatabase.db");

            // Build the connection string
            string connectionString = $"Data Source={dbFilePath};Cache=Shared";

            // Configure DbContext options
            var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connectionString)
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                .Options;

            // Create DbContext and apply migrations
            using var db = new AppDbContext(contextOptions);
            db.Database.Migrate();

            // Your GameRepositoryEF instance
            var gameRepository = new GameRepositoryEF(db);

            MainMenu mainMenu = new MainMenu();

            while (true)
            {
                Console.Clear();
                mainMenu.Display();

                int choice = mainMenu.GetUserChoice(1, 3);

                switch (choice)
                {
                    case 1:
                        NewGame newGameMenu = new NewGame();
                        newGameMenu.Display();
                        break;
                    case 2:
                        LoadGame loadGameMenu = new LoadGame();
                        loadGameMenu.Display();
                        break;
                    case 3:
                        ExitGame exitGame = new ExitGame();
                        exitGame.Display();
                        break;
                }
            }
        }
    }
}