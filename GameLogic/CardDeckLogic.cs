
using System;
using System.Collections.Generic;
using UnoGame.GameObject;

namespace UnoGame.GameLogic
{
    public class CardDeckLogic
    {
        private CardDeck deck;

        public CardDeckLogic(CardDeck cardDeck)
        {
            deck = cardDeck;
        }

        public List<Card> DealCards(int numCards)
        {
            if (deck.GetDeck().Count < numCards)
            {
                // Handle the case where there are not enough cards in the deck
                return null;
            }

            List<Card> dealtCards = deck.GetDeck().GetRange(0, numCards);
            deck.GetDeck().RemoveRange(0, numCards);

            return dealtCards;
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
        }

        public Card ChooseFirstCard()
        {
            ShuffleDeck(); // Ensure the deck is shuffled
            List<Card> deckCards = deck.GetDeck();
            if (deckCards.Count > 0)
            {
                Card firstCard = deckCards[0];
                deckCards.RemoveAt(0);
                return firstCard;
            }
            return null; // Handle the case where the deck is empty
        }
    }
}