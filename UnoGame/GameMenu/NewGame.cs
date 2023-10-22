using System;
using UnoGame.GameObject;

namespace UnoGame.GameMenu
{
    public class NewGame
    {
        private int numberOfPlayers;
        private Player[] players;
        private bool useCustomRules;

        public NewGame()
        {
        }

        public void Display()
        {
            bool readyToStart = false;

            while (!readyToStart)
            {
                Console.Clear();
                Console.WriteLine("New Game Menu - Enter the number of players (2-10):");
                numberOfPlayers = GetUserChoice(2, 10);

                // Create an array to store player information
                players = new Player[numberOfPlayers];

                for (int i = 0; i < numberOfPlayers; i++)
                {
                    Console.WriteLine($"Player {i + 1}:");
                    Console.Write("Enter player name: ");
                    string playerName = Console.ReadLine();

                    Console.WriteLine("Select player type:");
                    Console.WriteLine("1. Human");
                    Console.WriteLine("2. AI");
                    int playerType = GetUserChoice(1, 2);

                    PlayerType type = playerType == 1 ? PlayerType.Human : PlayerType.AI;

                    players[i] = new Player(playerName, type);
                }

                // Choose between original or custom rules
                Console.WriteLine("Select rules:");
                Console.WriteLine("1. Original Uno Rules");
                Console.WriteLine("2. Custom Rules");
                int rulesChoice = GetUserChoice(1, 2);
                useCustomRules = rulesChoice == 2;

                // Review and edit step
                Console.Clear();
                Console.WriteLine("Review and edit your choices:");
                DisplayPlayerChoices(); // Display player names and types
                DisplayRuleChoice(); // Display rule choice

                Console.WriteLine("1. Continue to start the game");
                Console.WriteLine("2. Edit your choices");
                int reviewChoice = GetUserChoice(1, 2);

                if (reviewChoice == 1)
                {
                    readyToStart = true; // Set readyToStart to true to exit the loop and start the game
                }
                // If the user chooses to edit, they will remain in the menu to make changes
            }

            StartNewGame(); // Start the game when the user is ready
        }

        private void StartNewGame()
        {
            // Implement logic to start a new game with the players and custom rules if selected
            if (useCustomRules)
            {
                // Implement custom rules logic
            }
            else
            {
                // Implement original Uno rules logic
            }
        }

        private void DisplayPlayerChoices()
        {
            Console.WriteLine("Player Choices:");
            for (int i = 0; i < players.Length; i++)
            {
                Console.WriteLine($"Player {i + 1}: {players[i].Name} ({players[i].Type})");
            }
        }

        private void DisplayRuleChoice()
        {
            Console.WriteLine("Rule Choice:");
            Console.WriteLine(useCustomRules ? "Custom Rules" : "Original Uno Rules");
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
