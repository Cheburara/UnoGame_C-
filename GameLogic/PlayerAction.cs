using UnoGame.GameObject;
namespace UnoGame.GameLogic
{
    public class PlayerAction
    {
        private CardDeckLogic cardDeckLogic;
        private PlayerHand playerHand;

        public PlayerAction(CardDeckLogic cardDeckLogic, PlayerHand playerHand)
        {
            this.cardDeckLogic = cardDeckLogic;
            this.playerHand = playerHand;
        }

        public PlayerAction(CardDeckLogic cardDeckLogic)
        {
            this.cardDeckLogic = cardDeckLogic;
        }

        public bool PlayCard(Player player, Card card)
        {
            if (IsValidCardToPlay(player, card))
            {
                return true;
            }

            return false; 
        }
        
        public Card DrawCard(Player player)
        {
            Card drawnCard = cardDeckLogic.DrawCard();
            playerHand.AddCardToHand(drawnCard); // Add the drawn card to the player's hand
            return drawnCard;
        }
        
        public void AddCardToHand(Player player, Card card)
        {
            playerHand.AddCardToHand(card);
        }

        private bool IsValidCardToPlay(Player player, Card card)
        {
            Card topDiscard = cardDeckLogic.GetTopDiscardCard();

            // Check if the card matches the current color, value, or is a Wild card
            if (card.Color == cardDeckLogic.GetCurrentColor()||
                card.Value == cardDeckLogic.GetCurrentValue()||
                card.Value == Enums.CardValue.Wild ||
                card.Value == Enums.CardValue.WildDrawFour)
            {
                return true; // The card can be played
            }

            // Check if the card matches the current color on top of the discard pile
            if (card.Color == topDiscard.Color)
            {
                return true; // The card matches the current color
            }

            return false; // The card cannot be played
        }

    }
}