using System;
using UnoGame.GameObject;

namespace UnoGame.GameMenu
{
    public class NewGame
    {
        private int numberOfPlayers;
        private Player[] players;

        public NewGame()
        {
        }

        public void Display()
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

            // Start a new game with the selected settings
            StartNewGame();
        }

        private void StartNewGame()
        {
            // Implement logic to start a new game with the players
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