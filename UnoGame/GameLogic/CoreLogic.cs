using UnoGame.GameObject;
using UnoGame.GameLogic;
using UnoGame.GameMenu;
using UnoGame.Rules;
using System;
using System.Collections.Generic;

namespace UnoGame.GameLogic
{
    public class CoreLogic
    {
        private UnoGameLogic unoGameLogic;
        private CardDeckLogic cardDeckLogic;
        private Player[] players;
        private EndMenu endMenu;
        private WinningLogic winningLogic;

        public CoreLogic(CardDeckLogic deckLogic, RulesBase gameRules, Player[] gamePlayers, EndMenu endMenu)
        {
            unoGameLogic = new UnoGameLogic(deckLogic, gamePlayers, new WinningLogic());
            unoGameLogic.GameEnded += HandleGameEnded;
            players = gamePlayers;
            cardDeckLogic = deckLogic;
            this.endMenu = endMenu; 
        }

        public void StartGame(Player[] gamePlayers, int initialCardCount, int totalCardsInDeck)
        {
            Console.WriteLine("Game started!");

            players = gamePlayers;

            Console.WriteLine("Step 2!");

            InitializeCardDeck(players.Length, totalCardsInDeck);

            DealInitialHands(players.Length, initialCardCount);

            int currentPlayerIndex = 0;

            while (!CheckForGameEnd())
            {
                unoGameLogic.PlayNextTurn();
                currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;
            }

            // Console.Out.Flush();
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
                List<Card> initialHand = cardDeckLogic.DealCards(initialCardsNumber);
                foreach (Card card in initialHand)
                {
                    player.DrawCard(card);
                }
            }
        }

        private void HandleGameEnded(Player winner)
        {
            endMenu.DisplayEndMessage(winner.Name);
        }

        private bool CheckForGameEnd()
        {
            /*foreach (var player in players)
            {
                if (WinningLogic.CheckForWin(player))
                {
                    // Additional logic can be added here before announcing the winner
                    if (player.Hand.GetCardCount() == 1)
                    {
                        Console.WriteLine($"{player.Name} shouts 'Uno!'");
                    }

                    WinningLogic.AnnounceWinner(player);
                    return true;
                }
            }*/

            return false;
        }
    }
}
