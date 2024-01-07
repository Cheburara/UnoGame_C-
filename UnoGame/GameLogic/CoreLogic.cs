using UnoGame.GameObject;
using UnoGame.GameMenu;
using UnoGame.Rules;
using UnoGame.Storage;


namespace UnoGame.GameLogic
{
    public class CoreLogic
    {
        private UnoGameLogic unoGameLogic;
        private CardDeckLogic cardDeckLogic;
        private Player[] players;
        private EndMenu endMenu;
        private SaveGame saveGame;
        private bool isGameStateLoaded = false;
        private GameState existingGameState;
        private GameStateStorage gameStateStorage;
        private string gameName;
        private ShufflePlayers shufflePlayers;
        
        
        public string FileName { get; set; }

        public CoreLogic(CardDeckLogic deckLogic, RulesBase gameRules, Player[] gamePlayers, EndMenu endMenu, string gameName)
        {
            {
                unoGameLogic = new UnoGameLogic(deckLogic, gamePlayers, new WinningLogic());
            }
            unoGameLogic.GameEnded += HandleGameEnded;
            players = gamePlayers;
            cardDeckLogic = deckLogic;
            this.endMenu = endMenu;
            saveGame = new SaveGame();
            gameStateStorage = new GameStateStorage();
            this.gameName = gameName;
            shufflePlayers = new ShufflePlayers();
        }

        public void StartGame(Player[] gamePlayers, int initialCardCount, int totalCardsInDeck)
        {
            Console.WriteLine("Game started!");

            players = gamePlayers;
            
            ShufflePlayers();

            Console.WriteLine("Step 2!");

            InitializeCardDeck(players.Length, totalCardsInDeck);

            DealInitialHands(players.Length, initialCardCount);

            ContinueGame();

        }
        public void ContinueGame()
        {
            Console.WriteLine("Step 3!");
            
            Console.WriteLine("Continuing the game...");
            
            LoadGameState(); 

            int currentPlayerIndex = 0;

            while (true)
            {
                unoGameLogic.PlayNextTurn();
                currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;

                if (CheckForGameEnd())
                {
                    Console.WriteLine("Game over!");
                    break;
                }

                if (CheckForStopCondition(currentPlayerIndex))
                {
                    Console.WriteLine("Game stopped by user.");
                    break;
                }

                UpdateGameState(); // Capture and save the updated game state
            }
        }
        private void ShufflePlayers()
        {
            Console.WriteLine("Shuffling players...");
            players = shufflePlayers.Shuffle(players);

            Console.WriteLine("Shuffled players:");
            foreach (var player in players)
            {
                Console.WriteLine(player.Name);
            }
        }

        private void InitializeCardDeck(int numberOfPlayers, int totalCardsInDeck)
        {
            Console.WriteLine("Shuffling the card deck...");
            cardDeckLogic.ShuffleDeck();

            Card firstCard = cardDeckLogic.ChooseFirstCard();

            Console.WriteLine("Shuffled card deck:");
            List<Card> shuffledDeck = cardDeckLogic.Deck;
            foreach (Card card in shuffledDeck)
            {
                Console.WriteLine(card);
            }

            Console.WriteLine($"The amount of cards in deck: {totalCardsInDeck}");
            Console.WriteLine($"First card: {firstCard}");
            
        }

        private void DealInitialHands(int numberOfPlayers, int initialCardsNumber)
        {
            foreach (Player player in players)
            {
                List<Card> initialHand = cardDeckLogic.DealCards(initialCardsNumber, player.Name);

                if (initialHand != null)
                {
            
                    foreach (Card card in initialHand)
                    {
                        player.DrawCard(card);
                    }
                }
            }
        }

        private void HandleGameEnded(Player winner)
        {
            endMenu.DisplayEndMessage(winner.Name);
        }

        private bool CheckForGameEnd()
        {
            if (cardDeckLogic.Deck.Count == 0)
            {
                Console.WriteLine("The deck is empty. The game ends in a draw.");
                endMenu.DisplayEndMessage("Draw");
                return true; // End the game
            }

            foreach (var player in players)
            {
                if (WinningLogic.CheckForWin(player))
                {
                    // Additional logic can be added here before announcing the winner
                    if (player.Hand.GetCardCount() == 1)
                    {
                        Console.WriteLine($"{player.Name} shouts 'Uno!'");
                    }

                    WinningLogic.AnnounceWinner(player);
                    endMenu.DisplayEndMessage(player.Name); // Display the end message
                    return true; // End the game
                }
            }

            return false; // Continue the game
        }
        
        private void LoadGameState()
        {
            string directoryPath = @"C:\Users\arina\RiderProjects\UNO\UnoGame\JSON";

            // Load the existing game state from the JSON file
            string filePath = Path.Combine(directoryPath, gameName);
            existingGameState = gameStateStorage.LoadFromJSON(filePath);

            if (existingGameState != null)
            {

                // Set the total cards in deck
                cardDeckLogic.Deck.Clear(); // Clear the current deck
                cardDeckLogic.Deck.AddRange(existingGameState.Deck);

                // Set the top discard card
                cardDeckLogic.GetTopDiscardCard();
                // Set the players' hands
                foreach (var player in players)
                {
                    if (existingGameState.PlayersHands.TryGetValue(player.Name, out List<Card> handCards))
                    {
                        player.Hand.Clear(); // Clear the current hand
                        player.Hand.AddCards(handCards);
                    }
                }

                // Set the flag to indicate that the game state is loaded
                isGameStateLoaded = true;
            }
            else
            {
                Console.WriteLine("Failed to load the existing game state. Loading aborted.");
            }
        }
        private void UpdateGameState()
        {
            string directoryPath = @"C:\Users\arina\RiderProjects\UNO\UnoGame\JSON";

            // Load the existing game state from the JSON file
            string filePath = Path.Combine(directoryPath, gameName);
            existingGameState = gameStateStorage.LoadFromJSON(filePath);

            if (existingGameState != null)
            {
                // Update the relevant values in the existing game state
                existingGameState.TotalCardsInDeck = cardDeckLogic.Deck.Count;
                existingGameState.TopCard = cardDeckLogic.GetTopDiscardCard();
        
                // Populate the cards in players' hands
                existingGameState.PlayersHands = new Dictionary<string, List<Card>>();
                foreach (var player in players)
                {
                    existingGameState.PlayersHands[player.Name] = new List<Card>(player.Hand.GetCards());
                }

                // Update the deck
                existingGameState.Deck = new List<Card>(cardDeckLogic.Deck);

                // Save the updated game state back to the JSON file
                gameStateStorage.SaveToJSON(filePath, existingGameState);

                // Set the flag to indicate that the game state is loaded
                isGameStateLoaded = true;
            }
            else
            {
                Console.WriteLine("Failed to load the existing game state. Update aborted.");
            }
        }

        private GameState GetGameState()
        {
            // Capture the current state of the game and return a GameState object
            return new GameState
            {
                StorageType = StorageType.JsonFile,
                // Add other properties based on your game state
            };
        }
        private bool CheckForStopCondition(int currentPlayerIndex)
        {
            
            if (players[currentPlayerIndex].Type == PlayerType.AI)
            {
                // AI player, continue the game
                return false;
            }
            
            Console.WriteLine("Do you want to stop the game?");
            Console.WriteLine("1. Yes");
            Console.WriteLine("2. No");

            int choice = GetUserChoice(1, 2);

            if (choice == 1)
            {
                Console.WriteLine("Do you want to return to the game?");
                Console.WriteLine("1. Yes");
                Console.WriteLine("2. No");

                int returnChoice = GetUserChoice(1, 2);

                if (returnChoice == 1)
                {
                    Console.WriteLine("Returning to the game...");
                    return false; 
                }
                else
                {
                    DisplayExitMenu();
                    return true; 
                }
            }

            return false; 
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
        
        private void DisplayExitMenu()
        {
            ExitGame exitMenu = new ExitGame();
            exitMenu.Display();
        }
    }
}
