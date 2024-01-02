using System;
using System.Collections.Generic;
using UnoGame.GameObject;
using UnoGame.Rules;

namespace UnoGame.GameLogic
{
    public class UnoGameLogic
    {
        private CardDeckLogic _cardDeckLogic;
        private Player[] _players;
        private WinningLogic _winningLogic;
        private int _currentPlayerIndex;
        private Card selectedCard;
        private TurnDirection.Enums.TurnDirection currentDirection;

        public event Action<Player> GameEnded;

        public UnoGameLogic(CardDeckLogic cardDeckLogic, Player[] players, WinningLogic winningLogic)
        {
            _cardDeckLogic = cardDeckLogic;
            _players = players;
            _winningLogic = winningLogic;
            _currentPlayerIndex = 0;
            
        }

        public void PlayNextTurn()
        {
            try
            {
                Enums.CardColor currentColor = _cardDeckLogic.GetCurrentColor();
                Enums.CardValue currentValue = _cardDeckLogic.GetCurrentValue();

                Player currentPlayer = _players[_currentPlayerIndex];
                

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
                        // Player chose to draw a card
                        Card drawnCard = _cardDeckLogic.DrawCard();
                        currentPlayer.DrawCard(drawnCard);

                        Console.WriteLine($"{currentPlayer.Name} drew a card: {drawnCard}");
                        playedCard = true; // Break the loop after drawing a card
                    }
                    else if (int.TryParse(input, out int selectedIndex) && selectedIndex >= 0 && selectedIndex < currentPlayer.GetCardsInHand().Count)
                    {
                        // Player is trying to play a card
                        Card selectedCard = currentPlayer.GetCardsInHand()[selectedIndex];

                        if (IsValidCardToPlay(selectedCard, currentColor, currentValue))
                        {
                            // Valid card to play
                            Console.WriteLine($"{currentPlayer.Name} played: {selectedCard}");
                            currentPlayer.PlayCard(selectedCard);

                            // Update the current color and value
                            _cardDeckLogic.UpdateCurrentColorAndValue(selectedCard);
                            
                            HandleWildCardEffects(selectedCard);
                            
                            HandleSpecialCards(selectedCard);
                            
                            playedCard = true;
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

                // Move to the next player
                _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Length;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An exception occurred: {ex}");
            }
        }
        
        private void HandleWildCardEffects(Card playedCard)
{
    if (playedCard.Value == Enums.CardValue.Wild || playedCard.Value == Enums.CardValue.WildDrawFour)
    {
        // Ask the player to choose a new color
        Console.Write("Choose a new color (Red, Blue, Yellow, Green): ");
        string chosenColor = Console.ReadLine();
        
        Console.WriteLine($"Chosen color: {chosenColor}");
        
        Card selectedCard = new Card { Color = playedCard.Color, Value = playedCard.Value };

        // Update the current color to the chosen color
        _cardDeckLogic.UpdateCurrentColorAndValue(new Card { Color = Enums.ParseEnum<Enums.CardColor>(chosenColor), Value = selectedCard.Value });

        if (playedCard.Value == Enums.CardValue.WildDrawFour)
        {
            // Draw four cards for the next player
            Player nextPlayer = _players[(_currentPlayerIndex + 1) % _players.Length];
            for (int i = 0; i < 4; i++)
            {
                Card drawnCard = _cardDeckLogic.DrawCard();
                nextPlayer.DrawCard(drawnCard);
            }
        }
    }
}
        private void HandleSpecialCards(Card playedCard)
        {
            switch (playedCard.Value)
            {
                case Enums.CardValue.Reverse:
                    // Reverse the direction of play
                    ReverseDirection();
                    break;

                case Enums.CardValue.Skip:
                    // Skip the next player
                    SkipNextPlayer();
                    break;
            }
        }

        private void ReverseDirection()
        {
            currentDirection = currentDirection == TurnDirection.Enums.TurnDirection.Clockwise
                ? TurnDirection.Enums.TurnDirection.Counterclockwise
                : TurnDirection.Enums.TurnDirection.Clockwise;

            Console.WriteLine($"Direction of play reversed. Current direction: {currentDirection}");
        }

        private void SkipNextPlayer()
        {
            // Implement logic to skip the next player
            _currentPlayerIndex = (_currentPlayerIndex + 2) % _players.Length;
        }

        private void DisplayTopCard()
        {
            Card topCard = _cardDeckLogic.GetTopDiscardCard();
            Console.WriteLine($"Top Card: {topCard}");
        }

        private void DisplayPlayerHand(Player player)
        {
            if (player != null)
            {
                List<Card> cardsInHand = player.GetCardsInHand();

                Console.WriteLine("Your Hand:");
                for (int i = 0; i < cardsInHand.Count; i++)
                {
                    Console.WriteLine($"{i}: {cardsInHand[i]}");
                }
            }
            else
            {
                Console.WriteLine("Player is null.");
            }
        }

        private bool IsValidCardToPlay(Card card, Enums.CardColor currentColor, Enums.CardValue currentValue)
        {
            Enums.CardColor cardColor = card.Color;
            Enums.CardValue cardValue = card.Value;

            // Check if the card color or value matches the current color or value
            if (cardColor == currentColor || cardValue == currentValue)
            {
                return true;
            }

            // Check for Wild cards
            if (cardValue == Enums.CardValue.Wild)
            {
                return true;
            }

            // Check for Wild Draw Four cards
            if (cardValue == Enums.CardValue.WildDrawFour)
            {
                // Implement additional checks if necessary
                return true;
            }
            
            if (cardValue == Enums.CardValue.Reverse)
            {
                return true;
            }

            // Add more specific rules based on the Uno game rules

            return false;
        }
    }
}
