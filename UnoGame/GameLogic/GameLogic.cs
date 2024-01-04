using UnoGame.GameObject;


namespace UnoGame.GameLogic
{
    public class UnoGameLogic
    {
        private CardDeckLogic _cardDeckLogic;
        private Player[] _players;
        private WinningLogic _winningLogic;
        private int _currentPlayerIndex;
        // private Card selectedCard;
        private TurnDirection.Enums.TurnDirection currentDirection;
        // private List<Card> playedCards = new List<Card>();

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
        
        DisplayTopCard();

        // Player's turn to play
        bool playedCard = false;

        if (currentPlayer.Type == PlayerType.AI)
        {
            // AI Player's turn to play
            if (currentPlayer is AiPlayer aiPlayer)
            {
                PlayAiPlayerTurn(aiPlayer);
                playedCard = true;
            }
        }

        while (!playedCard && currentPlayer.Type != PlayerType.AI)
        {
            Console.Write("Enter the index of the card you want to play or 'D' to draw a card: ");
            string input = Console.ReadLine();

            if (input.ToLower() == "d")
            {
                if (_cardDeckLogic.IsDeckEmpty())
                {
                    Console.WriteLine("The deck is empty. The game will end.");
                    GameEnded?.Invoke(null); 
                    return;
                }
                // Player chose to draw a card
                
                Card drawnCard = _cardDeckLogic.DrawCard();
                currentPlayer.Hand.AddCardToHand(drawnCard);
                
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

                    HandleWildCardEffects(selectedCard);

                    HandleSpecialCards(selectedCard);
                    
                    _cardDeckLogic.UpdateCurrentColorAndValue(selectedCard);

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
        
        private void PlayAiPlayerTurn(AiPlayer aiPlayer)
        {
            // Call the AI player's logic to play a card
            aiPlayer.PlayCard(_cardDeckLogic.GetTopDiscardCard());
            
            Card playedCard = aiPlayer.GetLastPlayedCard();
            
            HandleWildCardEffects(playedCard);

            if (playedCard != null)
            {
                // Update the current color and value with the played card
                _cardDeckLogic.UpdateCurrentColorAndValue(playedCard);
            }
            
        }
        
        private void HandleWildCardEffects(Card playedCard)
        {
            if (playedCard != null && (playedCard.Value == Enums.CardValue.Wild || playedCard.Value == Enums.CardValue.WildDrawFour))
            {
                if (_players[_currentPlayerIndex] is AiPlayer aiPlayer)
                {
                    // Use AI-specific logic to choose the color for Wild and Wild Draw Four cards
                    Enums.CardColor chosenColor = aiPlayer.ChooseColorForWildCard();
                    playedCard.Color = chosenColor;
                }
                else
                {
                    // Assume a default color for non-AI players or handle differently
                    playedCard.Color = Enums.CardColor.Red;
                }

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
