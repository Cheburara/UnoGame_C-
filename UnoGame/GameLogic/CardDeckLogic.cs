using System;
using System.Collections.Generic;
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

        public List<Card> DealCards(int numCards)
        {
            if (deck.GetDeck().Count < numCards)
            {
                Console.WriteLine("Not enough cards in the deck.");
                return null;
            }

            List<Card> dealtCards = deck.GetDeck().GetRange(0, numCards);
            deck.GetDeck().RemoveRange(0, numCards);

            Console.WriteLine("Dealt " + numCards + " card(s).");
            return dealtCards;
        }

        public Card DrawCard()
        {
            if (deck.GetDeck().Count > 0)
            {
                Card drawnCard = deck.GetDeck()[0]; // Get the top card from the deck
                deck.GetDeck().RemoveAt(0); // Remove the drawn card from the deck

                Console.WriteLine("Drew a card: " + drawnCard);
                return drawnCard;
            }

            Console.WriteLine("No more cards in the deck.");
            return null; // Handle the case where the deck is empty
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
                deckCards.RemoveAt(0);
                UpdateCurrentColorAndValue(topDiscard); // Update the current color and value
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

        // New method to update current color and value based on a card
        public void UpdateCurrentColorAndValue(Card card)
        {
            currentColor = card.Color;
            currentValue = card.Value;
            topDiscard = card;
            Console.WriteLine("Current color: " + currentColor + ", Current value: " + currentValue);
        }
    }
}
