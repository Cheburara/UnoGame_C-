using System;
using System.Collections.Generic;
using UnoGame.Rules;
using UnoGame.GameMenu;
using UnoGame.GameObject;

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
        private TraditionalRules traditionalRules;
        private CustomRules customRules;
        private PlayerTurnLogic playerTurnLogic;
        private ShufflePlayers shufflePlayers;
        private PlayerHand playerHand;
        private PlayerAction playerAction;
        private WinningLogic winningLogic;
        private EndMenu endMenu;

        public CoreLogic(CardDeckLogic deckLogic, RulesBase gameRules, Player[] gamePlayers, EndMenu endMenu)
        {
            cardDeckLogic = deckLogic;
            rules = gameRules;
            players = gamePlayers;
            winningLogic = new WinningLogic();
            playerTurnLogic = new PlayerTurnLogic(cardDeckLogic, players, winningLogic, playerAction, playerHand);
            this.endMenu = endMenu; // Store EndMenu instance
            playerTurnLogic.GameEnded += HandleGameEnded;
            shufflePlayers = new ShufflePlayers();
            currentPlayerIndex = 0;
            playerHand = new PlayerHand();
            playerAction = new PlayerAction(deckLogic, playerHand);
        }

        public void StartGame(int numberOfPlayers, Player[] gamePlayers, int initialCardCount, int TotalCardsInDeck)
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
            Card firstCard = cardDeckLogic.ChooseFirstCard();
    
            // Display the shuffled card deck
            Console.WriteLine("Shuffled card deck:");
            List<Card> shuffledDeck = cardDeckLogic.Deck;
            foreach (Card card in shuffledDeck)
            {
                Console.WriteLine(card);
            }

            Console.WriteLine("The amount of cards in deck: " + TotalCardsInDeck);
            Console.WriteLine("First card: " + firstCard);

            DealInitialHands(numberOfPlayers, initialCardCount);

            currentPlayerIndex = 0;

            while (!CheckForGameEnd())
            {
                Player currentPlayer = players[currentPlayerIndex];
                playerTurnLogic.PlayTurn();

                currentPlayerIndex = (currentPlayerIndex + 1) % numberOfPlayers;
            }

            Console.Out.Flush();
        }
        private void DealInitialHands(int numberOfPlayers, int initialCardsNumber)
        {
            foreach (Player player in players)
            {
                List<Card> initialHand = cardDeckLogic.DealCards(initialCardsNumber);
                foreach (Card card in initialHand)
                {
                    player.Hand.AddCardToHand(card);
                }
            }
        }
        
        public void PlayCard(Player player, Card card)
        {
            bool cardPlayed = playerAction.PlayCard(player, card, currentColor);

            if (cardPlayed)
            {
                AdvanceToNextPlayer();
            }
        }
        
        private void AdvanceToNextPlayer()
        {
            // Increment the currentPlayerIndex to move to the next player
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;
        }
        
        public void PlayPlayerTurn()
        {
            playerTurnLogic.PlayTurn();
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
