using UnoGame.GameObject;

namespace UnoGame.GameLogic
{
    public class PlayerAction
    {
        private CardDeckLogic _cardDeckLogic;
        private PlayerHand _playerHand;

        public PlayerAction(CardDeckLogic cardDeckLogic, PlayerHand playerHand)
        {
            this._cardDeckLogic = cardDeckLogic;
            this._playerHand = playerHand;
        }

        public bool CanPlayCard(Player player, Card card, Enums.CardColor currentColor)
        {
            // Check if the card is valid to play
            return IsValidCardToPlay(player, card, currentColor);
        }

        public Card DrawCard(Player player)
        {
            Card drawnCard = _cardDeckLogic.DrawCard();
            _playerHand.AddCardToHand(drawnCard);
            return drawnCard;

            // Handle the case when the deck is empty (you may need to implement this in CardDeckLogic)
        }

        public bool PlayCard(Player player, Card card, Enums.CardColor chosenColor)
        {
            if (CanPlayCard(player, card, chosenColor))
            {
                if (card.Value == Enums.CardValue.Wild || card.Value == Enums.CardValue.WildDrawFour)
                {
                    // It's a Wild card, so prompt the player to choose a color
                    Console.Write("Select a color (Red, Blue, Green, Yellow): ");
                    string colorChoice = Console.ReadLine();

                    if (Enum.TryParse(colorChoice, out Enums.CardColor selectedColor))
                    {
                        // Update the current color based on the chosen color
                        _cardDeckLogic.SetCurrentColor(selectedColor);
                    }
                    else
                    {
                        Console.WriteLine("Invalid color choice. Using default color.");
                    }
                }

                _playerHand.RemoveCardFromHand(card);

                // You might want to handle special card effects here (e.g., Skip, Reverse)
                // You can implement additional logic to handle these special cases
                return true;
            }

            return false;
        }

        public bool IsValidCardToPlay(Player player, Card card, Enums.CardColor currentColor)
        {
            Card topDiscard = _cardDeckLogic.GetTopDiscardCard();
            Enums.CardValue currentValue = _cardDeckLogic.GetCurrentValue();

            // Convert card.Color and topDiscard.Color to strings for comparison
            string cardColor = card.Color.ToString();
            string topDiscardColor = topDiscard.Color.ToString();

            // Convert card.Value and topDiscard.Value to strings for comparison
            string cardValue = card.Value.ToString();
            string topDiscardValue = topDiscard.Value.ToString();

            if (cardColor == currentColor.ToString() ||
                cardValue == currentValue.ToString() ||
                cardValue == Enums.CardValue.Wild.ToString() ||
                cardValue == Enums.CardValue.WildDrawFour.ToString())
            {
                return true;
            }

            if (cardColor == topDiscardColor)
            {
                return true;
            }

            if (cardValue == topDiscardValue)
            {
                return true;
            }

            // Check for wild cards with the chosen color
            if ((cardValue == Enums.CardValue.Wild.ToString() || cardValue == Enums.CardValue.WildDrawFour.ToString()) && cardColor == currentColor.ToString())
            {
                return true;
            }

            return false;
        }
        public void ChooseWildCardColor(Card wildCard)
        {
            if (wildCard.Value == Enums.CardValue.Wild || wildCard.Value == Enums.CardValue.WildDrawFour)
            {
                // It's a Wild card, so prompt the player to choose a color
                Console.Write("Select a color (Red, Blue, Green, Yellow): ");
                string colorChoice = Console.ReadLine();

                if (Enum.TryParse(colorChoice, out Enums.CardColor selectedColor))
                {
                    // Update the current color based on the chosen color
                    _cardDeckLogic.SetCurrentColor(selectedColor);
                }
                else
                {
                    Console.WriteLine("Invalid color choice. Using default color.");
                }
            }
        }
    }
}
