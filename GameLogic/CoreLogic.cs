using UnoGame.GameObject;
using UnoGame.Rules;
using UnoGame.GameMenu;

namespace UnoGame.GameLogic
{
    public class CoreLogic
    {
        private CardDeckLogic cardDeckLogic;
        private Player[] players;
        private int currentPlayerIndex;
        private Enums.CardColor currentColor;
        private Enums.CardValue currentValue;
        private RulesBase rules;
        private PlayerTurnLogic playerTurnLogic;
        private ShufflePlayers shufflePlayers; 
        private WinningLogic winningLogic;
        private EndMenu endMenu;

        public CoreLogic(CardDeckLogic deckLogic, RulesBase gameRules, Player[] gamePlayers, EndMenu endMenu)
        {
            cardDeckLogic = deckLogic;
            rules = gameRules;
            players = gamePlayers;
            winningLogic = new WinningLogic(); 
            playerTurnLogic = new PlayerTurnLogic(cardDeckLogic, players, winningLogic, endMenu);
            this.endMenu = endMenu; // Store EndMenu instance
            playerTurnLogic.GameEnded += HandleGameEnded; 
            shufflePlayers = new ShufflePlayers();
            
        }

        public void StartGame(int numberOfPlayers, Player[] gamePlayers)
        {
            Console.WriteLine("Game started!");
            
            Console.WriteLine("Shuffling players...");
            players = shufflePlayers.Shuffle(players);

            // Display the shuffled players
            Console.WriteLine("Shuffled players:");
            foreach (Player player in players)
            {
                Console.WriteLine(player.Name);
            }

            Console.WriteLine("Shuffling the card deck...");
            cardDeckLogic.ShuffleDeck();

            // Display the shuffled card deck
            Console.WriteLine("Shuffled card deck:");
            List<Card> shuffledDeck = cardDeckLogic.Deck;  // Access the deck from cardDeckLogic
            foreach (Card card in shuffledDeck)
            {
                Console.WriteLine(card);
            }

            int numInitialCards = 7;

            if (rules is TraditionalRules)
            {
                TraditionalRules traditionalRules = (TraditionalRules)rules;
                numInitialCards = traditionalRules.InitialCardCount;
            }
            else if (rules is CustomRules)
            {
                CustomRules customRules = (CustomRules)rules;
                numInitialCards = customRules.InitialCardCount;
            }

            while (!CheckForGameEnd())
            {
                Player currentPlayer = players[currentPlayerIndex];
                playerTurnLogic.PlayTurn();

                // Move to the next player in a circular manner
                currentPlayerIndex = (currentPlayerIndex + 1) % numberOfPlayers;
            }

            DealInitialHands(numInitialCards);

            Console.WriteLine("Game started!");
            Console.Out.Flush();
        }

        private void DealInitialHands(int numInitialCards)
        {
            foreach (Player player in players)
            {
                List<Card> initialHand = cardDeckLogic.DealCards(numInitialCards);
                foreach (Card card in initialHand)
                {
                    player.Hand.AddCardToHand(card);
                }
            }
        }
        
        private void HandleGameEnded(Player winner)
        {
            if (winner != null)
            {
                endMenu.DisplayEndMessage(winner.Name);
            }
        }
        private bool CheckForGameEnd()
        {
            // Implement game end conditions
            return false;
        }
    }
}
