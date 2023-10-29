
using UnoGame.GameObject;

namespace UnoGame.GameLogic
{
    public class PlayerTurnLogic
    {
        private CardDeckLogic _cardDeckLogic;
        private Player[] _players;
        private WinningLogic _winningLogic;
        private int _currentPlayerIndex;
        private PlayerAction _playerAction;
        private PlayerHand _playerHand;

        public event Action<Player> GameEnded;

        public PlayerTurnLogic(
            CardDeckLogic cardDeckLogic,
            Player[] players,
            WinningLogic winningLogic,
            PlayerAction playerAction,
            PlayerHand playerHand)
        {
            this._cardDeckLogic = cardDeckLogic;
            this._players = players;
            this._winningLogic = winningLogic;
            this._currentPlayerIndex = 0;
            this._playerAction = playerAction;
            this._playerHand = playerHand;
        }

        public void PlayTurn()
        {
            try
            {
                Enums.CardColor currentColor = _cardDeckLogic.GetCurrentColor();
                Enums.CardValue currentValue = _cardDeckLogic.GetCurrentValue();

                Player currentPlayer = _players[_currentPlayerIndex];

                if (currentPlayer == null || currentPlayer.Hand == null)
                {
                    Console.WriteLine("Player or player's hand is null.");
                    _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Length;
                    return;
                }

                Console.WriteLine($"{currentPlayer.Name}'s turn");
                DisplayPlayerHand(currentPlayer);

                // Display the current top card
                DisplayTopCard();

                // Player's turn to play
                bool playedCard = false;

                while (!playedCard)
                {
                    Console.Write("Enter the index of the card you want to play or 'D' to draw a card: ");
                    string input = Console.ReadLine();

                    if (input.ToLower() == "d")
                    {
                        if (_playerAction != null)
                        {
                            // Player chose to draw a card
                            Card drawnCard = _playerAction.DrawCard(currentPlayer);

                            if (drawnCard != null)
                            {
                                currentPlayer.Hand.AddCardToHand(drawnCard);
                                Console.WriteLine($"{currentPlayer.Name} drew a card: {drawnCard}");
                            }
                            else
                            {
                                Console.WriteLine("Card deck is empty or an error occurred while drawing a card.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Player action is null.");
                        }
                    }
                    else if (int.TryParse(input, out int selectedIndex) && selectedIndex >= 0 && selectedIndex < currentPlayer.Hand.GetCardsInHand().Count)
                    {
                        // Player is trying to play a card
                        Card selectedCard = currentPlayer.Hand.GetCardsInHand()[selectedIndex];

                        // Add debug output to check the card's validity
                        Console.WriteLine($"Checking if {currentPlayer.Name} can play: {selectedCard}");

                        if (_playerAction != null && _playerAction.IsValidCardToPlay(currentPlayer, selectedCard, currentColor))
                        {
                            // Valid card to play
                            Console.WriteLine($"{currentPlayer.Name} played: {selectedCard}");
                            bool cardPlayed = _playerAction.PlayCard(currentPlayer, selectedCard, currentColor);

                            if (cardPlayed)
                            {
                                playedCard = true;
                                currentPlayer.Hand.RemoveCardFromHand(selectedCard);
                                _cardDeckLogic.UpdateCurrentColorAndValue(selectedCard);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid card selection. Try again.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Try again.");
                    }
                }

                // Check for win conditions
                if (_winningLogic != null && WinningLogic.CheckForWin(currentPlayer))
                {
                    Console.WriteLine($"{currentPlayer.Name} has won!");
                    GameEnded?.Invoke(currentPlayer);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An exception occurred: {ex}");
            }
        }




        private void DisplayTopCard()
        {
            Card topCard = _cardDeckLogic.GetTopDiscardCard();
            Console.WriteLine($"Top Card: {topCard}");
        }

        private void SkipNextPlayer()
        {
            // Implement logic to skip the next player's turn
            // Add your code here
        }

        private void ReverseOrderOfPlay()
        {
            // Implement logic to reverse the order of play
            // Add your code here
        }

        private void DisplayPlayerHand(Player player)
        {
            if (player != null && player.Hand != null)
            {
                List<Card> cardsInHand = player.Hand.GetCardsInHand();

                Console.WriteLine("Your Hand:");
                for (int i = 0; i < cardsInHand.Count; i++)
                {
                    Console.WriteLine($"{i}: {cardsInHand[i]}");
                }
            }
            else
            {
                Console.WriteLine("Player or player's hand is null.");
            }
        }
    }
}
