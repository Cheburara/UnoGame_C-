using System;
using UnoGame.GameObject;
using UnoGame.GameLogic;
using UnoGame.Rules;
using UnoGame.Storage;

namespace UnoGame.GameMenu
{
    public class NewGame
    {
        private int numberOfPlayers;
        private Player[] players;
        private bool useCustomRules;
        private bool useTraditionalRules;
        private CardDeckLogic cardDeckLogic;
        private CustomRules customRules;
        private EndMenu endMenu;
        private string gameName;
        private StorageType storageType;
        private GameStateStorage gameStateStorage;
        private SaveGame saveGame;

        public NewGame()
        {
            CardDeck cardDeck = new CardDeck();
            cardDeckLogic = new CardDeckLogic(cardDeck);
            endMenu = new EndMenu();
            this.saveGame = saveGame;
            gameStateStorage = new GameStateStorage();
        }

        public void Display()
        {
            Console.WriteLine("Select storage type:");
            Console.WriteLine($"{(int)StorageType.JsonFile}. Save to JSON file");
            Console.WriteLine($"{(int)StorageType.Database}. Save to Database");
            int storageTypeChoice = GetUserChoice(1, 2);
            storageType = (StorageType)storageTypeChoice;

            GameStateStorage gameStateStorage = new GameStateStorage();

            bool readyToStart = false;

            Console.Clear();
            Console.WriteLine("Enter a name for your game:");
            gameName = Console.ReadLine();
            gameStateStorage.GameName = gameName;

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
                    int playerTypeChoice = GetUserChoice(1, 2);

                    PlayerType playerType = playerTypeChoice == 1 ? PlayerType.Human : PlayerType.AI;

                    if (playerType == PlayerType.Human)
                    {
                        players[i] = new Player(playerName, playerType);
                    }
                    else
                    {
                        players[i] = new AiPlayer(playerName, cardDeckLogic);
                    }
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

            SaveGame();

            StartNewGame();
        }

        public void StartNewGame()
        {
            RulesBase gameRules;
            int initialCardCount;
            int totalCardsInDeck;

            CreateGameRules(out gameRules, out initialCardCount, out totalCardsInDeck);
            

            CoreLogic coreLogic = new CoreLogic(cardDeckLogic, gameRules, players, endMenu, gameName );
            coreLogic.StartGame(players, initialCardCount, totalCardsInDeck);

            // Save the game after it's played and finished
            // SaveGame();
        }

        public void SaveGame()
        {
            RulesBase gameRules;
            int initialCardCount;
            int totalCardsInDeck;

            CreateGameRules(out gameRules, out initialCardCount, out totalCardsInDeck);

            GameState gameState = GetGameState(gameRules, initialCardCount, totalCardsInDeck);

            if (!gameName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
            {
                // Append ".json" extension to gameName if it doesn't already have it
                gameName += ".json";
            }

            if (storageType == StorageType.JsonFile)
            {
                gameStateStorage.SaveToJSON(gameName, gameState);
            }
            else if (storageType == StorageType.Database)
            {
                gameStateStorage.SaveToDatabase(gameState);
            }
            else
            {
                saveGame.Save(gameName, cardDeckLogic, players);
            }
        }

        private GameState GetGameState(RulesBase gameRules, int initialCardCount, int totalCardsInDeck)
        {
            var gameState = new GameState
            {
                StorageType = storageType,
                GameName = gameName,
                UseCustomRules = useCustomRules,
                UseTraditionalRules = !useCustomRules,
                NumberOfPlayers = numberOfPlayers,
                Players = players,
                Deck = cardDeckLogic.Deck,
            };
            
            if (useCustomRules)
            {
                gameState.CustomRules = (CustomRules)gameRules;
                gameState.CustomRules.TotalCardsInDeck = totalCardsInDeck;
                gameState.CustomRules.InitialCardCount = initialCardCount;
            }
            else
            {
                gameState.TraditionalRules = (TraditionalRules)gameRules;
            }
            return gameState;
            
        }

        private void CreateGameRules(out RulesBase gameRules, out int initialCardCount, out int totalCardsInDeck)
        {
            if (useCustomRules)
            {
                if (customRules == null)
                {
                    customRules = new CustomRules();
                    customRules.Display();
                }

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
