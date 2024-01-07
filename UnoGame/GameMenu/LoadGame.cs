using System;
using System.IO;
using System.Text.Json;
using UnoGame.Storage;
using UnoGame.GameObject;
using UnoGame.GameLogic;
using UnoGame.Rules;

namespace UnoGame.GameMenu
{
    public class LoadGame
    {
        public void Display()
        {
            Console.Clear();
            Console.WriteLine("Load Game Menu - Enter the name of the saved game file:");
            string fileName = Console.ReadLine();

            if (DoesFileExist(fileName))
            {
                Console.WriteLine("Game found. Do you want to load this game?");
                Console.WriteLine("1. Yes");
                Console.WriteLine("2. No");

                int choice = GetUserChoice(1, 2);

                if (choice == 1)
                {
                    GameState loadedGameState = LoadGameState(fileName);

                    if (loadedGameState != null)
                    {
                        ContinueGame(loadedGameState, fileName);
                    }
                    else
                    {
                        Console.WriteLine("Failed to load the game state. Returning to the main menu.");
                    }
                }
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
                    Console.WriteLine("Returning to the Main Menu...");
                }
            }
        }

        private static bool DoesFileExist(string fileName)
        {
            string directoryPath = @"C:\Users\arina\RiderProjects\UNO\UnoGame\JSON";
            string filePath = Path.Combine(directoryPath, fileName);
            return File.Exists(filePath) || File.Exists(filePath + ".json");
        }

        private static GameState LoadGameState(string fileName)
        {
            string directoryPath =@"C:\Users\arina\RiderProjects\UNO\UnoGame\JSON";
            string filePath = Path.Combine(directoryPath, fileName);

            if (File.Exists(filePath))
            {
                try
                {
                    // Create an instance of GameStateStorage
                    GameStateStorage gameStateStorage = new GameStateStorage();

                    // Use the instance to load the game state from JSON
                    GameState loadedGameState = gameStateStorage.LoadFromJSON(filePath);

                    if (loadedGameState != null)
                    {
                        return loadedGameState;
                    }
                    else
                    {
                        Console.WriteLine("Failed to load the game state. Returning to the main menu.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to load the game state: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("File not found: " + filePath);
            }

            return null; // File not found or failed to load game state
        }


        private static void ContinueGame(GameState gameState, string gameName)
        {
            Console.WriteLine("Game loaded successfully. Continuing the game...");
    
            CardDeckLogic cardDeckLogic = new CardDeckLogic(new CardDeck());
            EndMenu endMenu = new EndMenu();
    
            RulesBase gameRules;
            int initialCardCount;
            int totalCardsInDeck;

            if (gameState.UseCustomRules && gameState.CustomRules != null)
            {
                gameRules = gameState.CustomRules;
                initialCardCount = gameState.CustomRules.InitialCardCount;
                totalCardsInDeck = gameState.CustomRules.TotalCardsInDeck;
            }
            else
            {
                gameRules = gameState.TraditionalRules; // Assuming you have TraditionalRules in your GameState
                initialCardCount = gameState.TraditionalRules.InitialCardCount;
                totalCardsInDeck = gameState.TraditionalRules.TotalCardsInDeck;
            }

            CoreLogic coreLogic = new CoreLogic(cardDeckLogic, gameRules, gameState.Players, endMenu, gameName);
            
            // Implement logic to continue the game using the loaded game state
            coreLogic.ContinueGame();
        }

        private static int GetUserChoice(int min, int max)
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
