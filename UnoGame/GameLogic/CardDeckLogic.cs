using UnoGame.GameObject;

namespace UnoGame.GameLogic
{
    public class CardDeckLogic
    {
        private CardDeck deck;
        private Card topDiscard;
        private Enums.CardColor currentColor;
        private Enums.CardValue currentValue;
        public CardDeckLogic(CardDeck cardDeck)
        {
            deck = cardDeck;
        }

        public List<Card> Deck
        {
            get { return deck.GetDeck(); }
        }
        
        public void SetCurrentColor(Enums.CardColor color)
        {
            currentColor = color;
        }

        public Enums.CardColor GetCurrentColor()
        {
            return currentColor;
        }

        public Enums.CardValue GetCurrentValue()
        {
            return currentValue;
        }

        public List<Card> DealCards(int numCards, string playerName)
        {
            if (deck.GetDeck().Count < numCards)
            {
                Console.WriteLine($"Not enough cards in the deck to deal to {playerName}.");
                return null;
            }

            List<Card> dealtCards = deck.GetDeck().GetRange(0, numCards);
            deck.GetDeck().RemoveRange(0, numCards);

            Console.WriteLine($"Dealt {numCards} card(s) to {playerName}:");
            foreach (Card card in dealtCards)
            {
                Console.WriteLine(card); // Print each dealt card
            }

            return dealtCards;
        }

        public Card DrawCard(Player currentPlayer)
        {
            if (deck.GetDeck().Count > 0)
            {
                Card drawnCard = deck.GetDeck()[0]; // Get the top card from the deck
                deck.GetDeck().RemoveAt(0); // Remove the drawn card from the deck
                
                Console.WriteLine("Drew a card: " + drawnCard);

                // Check if the drawn card is playable
                while (!IsPlayableCard(drawnCard))
                {
                    Console.WriteLine("Drew an unplayable card: " + drawnCard);

                    // Draw a new card from the deck
                    if (deck.GetDeck().Count > 0)
                    {
                        drawnCard = deck.GetDeck()[0];
                        deck.GetDeck().RemoveAt(0);

                        Console.WriteLine("Drew a new card: " + drawnCard);
                    }
                    else
                    {
                        Console.WriteLine("No more cards in the deck.");
                        return null; // Exit the method if the deck is empty
                    }
                }

                // If the current player is AI, automatically play the drawn card
                if (currentPlayer.Type == PlayerType.AI)
                {
                    Console.WriteLine("AI player is automatically playing the drawn card: " + drawnCard);
                    currentPlayer.PlayCard(drawnCard); // Assuming there's a PlayCard method in the Player class
                }
        
                return drawnCard;
            }

            Console.WriteLine("No more cards in the deck.");
            return null; // Exit the method if the deck is empty
        }
        public void DrawFour(Player nextPlayer)
        {
            for (int i = 0; i < 4; i++)
            {
                if (deck.GetDeck().Count > 0)
                {
                    Card drawnCard = deck.GetDeck()[0]; 
                    deck.GetDeck().RemoveAt(0); 
                    nextPlayer.Hand.AddCardToHand(drawnCard);
                }
                else
                {
                    Console.WriteLine("Deck is empty!");
                    break; 
                }
            }
            if (nextPlayer.Type == PlayerType.Human)
            {
                Console.Write("Choose a color for the Wild card (Red, Blue, Yellow, Green): ");
                string chosenColor = Console.ReadLine();

                Console.WriteLine($"Chosen color: {chosenColor}");

                // Update the current color and value
                UpdateCurrentColorAndValue(new Card { Color = Enums.ParseEnum<Enums.CardColor>(chosenColor), Value = Enums.CardValue.Wild });
            }
            else if (nextPlayer.Type == PlayerType.AI)
            {
                // Implement AI logic for choosing color (replace this with your actual logic)
                Enums.CardColor aiChosenColor = ((AiPlayer)nextPlayer).ChooseColorForWildCard();

                // Update the current color and value
                UpdateCurrentColorAndValue(new Card { Color = aiChosenColor, Value = Enums.CardValue.Wild });
            }
        }

        public void ShuffleDeck()
        {
            Random random = new Random();
            int n = deck.GetDeck().Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                Card card = deck.GetDeck()[k];
                deck.GetDeck()[k] = deck.GetDeck()[n];
                deck.GetDeck()[n] = card;
            }

            Console.WriteLine("Deck has been shuffled.");
        }

        public Card ChooseFirstCard()
        {
            List<Card> deckCards = deck.GetDeck();
            if (deckCards.Count > 0)
            {
                topDiscard = deckCards[0]; // Set topDiscard as the first card

                // Check if the first card is a Wild or WildDrawFour card
                if (topDiscard.Value == Enums.CardValue.Wild || topDiscard.Value == Enums.CardValue.WildDrawFour)
                {
                    ShuffleDeck();
                    topDiscard = deckCards[0];
                }

                // Remove the chosen card from the deck
                deckCards.RemoveAt(0);

                // Update the current color and value based on the chosen card
                UpdateCurrentColorAndValue(topDiscard);

                Console.WriteLine("The first card has been chosen and is " + topDiscard.ToString());
                return topDiscard;
            }

            Console.WriteLine("No more cards in the deck.");
            return null;
        }


        public Card GetTopDiscardCard()
        {
            return topDiscard;

        }
        public void SetTopDiscardCard(Card card)
        {
            topDiscard = card;
        }

        public bool IsPlayableCard(Card card)
        {
            Enums.CardColor currentColor = GetCurrentColor();
            Enums.CardValue currentValue = GetCurrentValue();
            
            if (card.Value == Enums.CardValue.Wild || card.Value == Enums.CardValue.WildDrawFour)
            {
                return true; // Wild cards are always playable
            }
            
            // Check if the card is playable based on the current color and value
            return card.Color == currentColor || card.Value == currentValue;
            
        }
        public void UpdateCurrentColorAndValue(Card card)
        {
            currentColor = card.Color;
            currentValue = card.Value;
            topDiscard = card;
            Console.WriteLine("Current color: " + currentColor + ", Current value: " + currentValue);
        }
        
        public bool IsDeckEmpty()
        {
            return deck.GetDeck().Count == 0;
        }
    }
}
