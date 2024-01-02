using UnoGame.GameLogic;
using UnoGame.GameMenu;

namespace UnoGame.GameObject
{
    public class Player
    {
        public string Name { get; set; }
        public PlayerType Type { get; set; }
        public PlayerHand Hand { get; set; }

        public Player(string name, PlayerType type)
        {
            Name = name;
            Type = type;
            Hand = new PlayerHand();
        }

        public void PlayCard(Card card)
        {
            Hand.PlayCard(card);
        }

        public void DrawCard(Card card)
        {
            Hand.DrawCard(card);
        }

        public List<Card> GetCardsInHand()
        {
            return Hand.GetCards();
        }
    }

    public class PlayerHand
    {
        private List<Card> cards;

        public PlayerHand()
        {
            cards = new List<Card>();
        }

        public void PlayCard(Card card)
        {
            // Implement logic to play a card
            cards.Remove(card);
        }

        public void DrawCard(Card card)
        {
            // Implement logic to draw a card
            cards.Add(card);
        }

        public List<Card> GetCards()
        {
            return cards;
        }

        public bool HasEmptyHand()
        {
            return cards.Count == 0;
        }

        // Add more methods if necessary
    }

    public enum PlayerType
    {
        Human,
        AI
    }
}