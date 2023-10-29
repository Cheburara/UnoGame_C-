using System;
using System.IO;
using System.Text.Json;
using UnoGame.GameObject;

namespace UnoGame.GameMenu
{
    public class LoadGame
    {
        public LoadGame()
        {
        }

        public void Display()
        {
            Console.Clear();
            Console.WriteLine("Load Game Menu - Enter the name of the saved game file:");
            string fileName = Console.ReadLine();
            
            if (File.Exists(fileName))
            {
                Console.WriteLine("Game found. Do you want to load this game?");
                Console.WriteLine("1. Yes");
                Console.WriteLine("2. No");

                int choice = GetUserChoice(1, 2);

                if (choice == 1)
                {
                    // Implement logic to load the game state from the file
                    GameState loadedGameState = LoadGameState(fileName);

                    if (loadedGameState != null)
                    {
                        // Continue the game using the loaded game state
                        ContinueGame(loadedGameState);
                    }
                    else
                    {
                        Console.WriteLine("Failed to load the game state. Returning to the main menu.");
                    }
                }
                // No need to add logic for choice == 2 since it's "No" (i.e., not loading the game).
            }
            else
            {
                Console.WriteLine("File not found. Please enter a valid saved game file name.");
                Console.WriteLine("1. Try Again");
                Console.WriteLine("2. Return to Main Menu");

                int choice = GetUserChoice(1, 2);

                if (choice == 1)
                {
                    Display(); // Try again
                }
                else if (choice == 2)
                {
                    // Return to the main menu
                    Console.WriteLine("Returning to the Main Menu...");
                }
            }
        }

        private GameState LoadGameState(string fileName)
        {
            if (File.Exists(fileName))
            {
                try
                {
                    string jsonString = File.ReadAllText(fileName);
                    return JsonSerializer.Deserialize<GameState>(jsonString);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to load the game state: " + ex.Message);
                }
            }
            return null; // File not found or failed to load game state
        }

        private void ContinueGame(GameState gameState)
        {
            // Implement logic to continue the game using the loaded game state
            Console.WriteLine("Game loaded successfully. Continuing the game...");
        }

        private int GetUserChoice(int min, int max)
        {
            int choice;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out choice) && choice >= min && choice <= max)
                {
                    return choice;
                }
                Console.WriteLine("Invalid input. Please enter a valid choice.");
            }
        }
    }
}
