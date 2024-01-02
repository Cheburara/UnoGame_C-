using System;
using System.IO;
using System.Text.Json;
using UnoGame.GameObject;
using UnoGame.GameLogic;
using UnoGame.Rules;

namespace UnoGame.GameMenu
{
    public class NewGame
    {
        private int numberOfPlayers;
        private Player[] players;
        private bool useCustomRules;
        private bool useTraditionalRules;
        private CardDeckLogic cardDeckLogic;
        private EndMenu endMenu;
        private string gameName; // Added a field to store the game name.

        public void SaveGameState(string fileName, GameState gameState)
        {
            string jsonString = JsonSerializer.Serialize(gameState);
            File.WriteAllText(fileName, jsonString);
            Console.WriteLine("Game state saved to " + fileName);
        }

        public NewGame()
        {
            CardDeck cardDeck = new CardDeck();
            cardDeckLogic = new CardDeckLogic(cardDeck);
            endMenu = new EndMenu();
        }

        public void Display()
        {
            bool readyToStart = false;

            Console.Clear();
            Console.WriteLine("Enter a name for your saved game:");
            gameName = Console.ReadLine(); // Ask the user to name the game.

            while (!readyToStart)
            {
                Console.Clear();
                Console.WriteLine("New Game Menu - Enter the number of players (2-10):");
                numberOfPlayers = GetUserChoice(2, 10);

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

                Console.WriteLine("Select rules:");
                Console.WriteLine("1. Original Uno Rules");
                Console.WriteLine("2. Custom Rules");
                int rulesChoice = GetUserChoice(1, 2);
                useCustomRules = rulesChoice == 2;

                Console.Clear();
                Console.WriteLine("Review and edit your choices:");
                DisplayPlayerChoices();
                DisplayRuleChoice();

                Console.WriteLine("1. Continue to start the game");
                Console.WriteLine("2. Edit your choices");
                int reviewChoice = GetUserChoice(1, 2);

                if (reviewChoice == 1)
                {
                    readyToStart = true;
                }
            }

            StartNewGame();
        }

        public void StartNewGame()
        {
            RulesBase gameRules;
            int initialCardCount;
            int totalCardsInDeck;

            if (useCustomRules)
            {
                CustomRules customRules = new CustomRules();
                customRules.Display();
                gameRules = customRules;
                initialCardCount = customRules.InitialCardCount;
                totalCardsInDeck = customRules.TotalCardsInDeck;
            }
            else
            {
                TraditionalRules traditionalRules = new TraditionalRules();
                gameRules = traditionalRules;
                initialCardCount = traditionalRules.InitialCardCount;
                totalCardsInDeck = traditionalRules.TotalCardsInDeck;
            }

            CoreLogic coreLogic = new CoreLogic(cardDeckLogic, gameRules, players, endMenu);
            coreLogic.StartGame(players, initialCardCount, totalCardsInDeck);

            // Save the game after it's played and finished
            SaveGame();
        }

        public void SaveGame()
        {
            // Create a GameState object and populate it with data
            GameState gameState = new GameState
            {
                Deck = cardDeckLogic.Deck, // Assuming Deck is a List<Card>
                Players = players,
            };

            // Save the game state with the provided gameName
            string fileName = $"{gameName}.json";
            SaveGameState(fileName, gameState);
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

        public int GetUserChoice(int min, int max)
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
