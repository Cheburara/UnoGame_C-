using System;
using System.Collections.Generic;

namespace UnoGame.GameObject
{
    public class CardDeck
    {
        private const int TotalCardsInDeck = 112;
        private List<Card> deck;

        public CardDeck()
        {
            // Initialize the deck and populate it
            deck = new List<Card>();
            InitializeDeck();
        }

        public List<Card> GetDeck()
        {
            return deck;
        }

        private void InitializeDeck()
        {
            // Loop through all card colors and values to create Uno cards
            foreach (Enums.CardColor color in Enum.GetValues(typeof(Enums.CardColor)))
            {
                if (color != Enums.CardColor.Black)
                {
                    // Numbered cards
                    foreach (Enums.CardValue value in Enum.GetValues(typeof(Enums.CardValue)))
                    {
                        if (value != Enums.CardValue.Wild && value != Enums.CardValue.WildDrawFour)
                            // value != Enums.CardValue.WildShuffleHands && value != Enums.CardValue.WildCustomizable)
                        {
                            Enums.CardScore score = Enums.CardScore.Zero; // Default to zero for numbered cards
                            if (value != Enums.CardValue.Zero)
                            {
                                score = (Enums.CardScore)Enum.Parse(typeof(Enums.CardScore), value.ToString());
                            }

                            deck.Add(new Card { Color = color, Value = value, Score = score });
                            // Add a second copy of cards with values 1-9
                            if (value != Enums.CardValue.Zero)
                            {
                                deck.Add(new Card { Color = color, Value = value, Score = score });
                            }
                        }
                    }

                    // Special cards (Reverse, Skip)
                    deck.Add(new Card { Color = color, Value = Enums.CardValue.Reverse, Score = Enums.CardScore.Reverse });
                    deck.Add(new Card { Color = color, Value = Enums.CardValue.Skip, Score = Enums.CardScore.Skip });
                }
                else
                {
                    // Special black cards
                    deck.Add(new Card { Color = color, Value = Enums.CardValue.Wild, Score = Enums.CardScore.Wild });
                    deck.Add(new Card { Color = color, Value = Enums.CardValue.Wild, Score = Enums.CardScore.Wild });
                    deck.Add(new Card { Color = color, Value = Enums.CardValue.Wild, Score = Enums.CardScore.Wild });
                    deck.Add(new Card { Color = color, Value = Enums.CardValue.Wild, Score = Enums.CardScore.Wild });
                    deck.Add(new Card { Color = color, Value = Enums.CardValue.WildDrawFour, Score = Enums.CardScore.WildDrawFour });
                    deck.Add(new Card { Color = color, Value = Enums.CardValue.WildDrawFour, Score = Enums.CardScore.WildDrawFour });
                    deck.Add(new Card { Color = color, Value = Enums.CardValue.WildDrawFour, Score = Enums.CardScore.WildDrawFour });
                    deck.Add(new Card { Color = color, Value = Enums.CardValue.WildDrawFour, Score = Enums.CardScore.WildDrawFour });
                    // deck.Add(new Card { Color = color, Value = Enums.CardValue.WildShuffleHands, Score = Enums.CardScore.WildShuffleHands });
                    // deck.Add(new Card { Color = color, Value = Enums.CardValue.WildCustomizable, Score = Enums.CardScore.WildCustomizable });
                    // deck.Add(new Card { Color = color, Value = Enums.CardValue.WildCustomizable, Score = Enums.CardScore.WildCustomizable });
                    // deck.Add(new Card { Color = color, Value = Enums.CardValue.WildCustomizable, Score = Enums.CardScore.WildCustomizable });
                    //
                }
            }
        }

        public void ShuffleDeck()
        {
            Random random = new Random();
            int n = deck.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                Card card = deck[k];
                deck[k] = deck[n];
                deck[n] = card;
            }
        }
    }
}
