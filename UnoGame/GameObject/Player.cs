using UnoGame.GameLogic;
using UnoGame.DAL.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnoGame.GameObject
{
    public class Player
    {
        [Key]
        public int PlayerId { get; set; }
        
        [Required] 
        public string Name { get; set; }
        
        public Guid GameEntityId { get; set; }
        [ForeignKey("GameEntityId")]
        public GameEntity GameEntity { get; set; }
        
        public List<PlayerHandSummaryEntity> HandSummaries { get; set; }
        public PlayerType Type { get; set; }
        public PlayerHand Hand { get; set; }

        public Player(string name, PlayerType type)
        {
            Name = name;
            Type = type;
            Hand = new PlayerHand();
        }

        public bool IsTurnSkipped { get; set; }
        public void SkipTurn()
        {
            IsTurnSkipped = true;
        }

        public virtual void PlayCard(Card card)
        {
            Hand.PlayCard(card);
        }
        public void DrawCard(Card card)
        {
            Hand.DrawCard(card);
        }
        public List<Card> GetCardsInHand()
        {
            return new List<Card>(Hand.GetCards());
        }
        
    }

    public class PlayerHand
    {
        [Key]
        public int PlayerHandId { get; set; }

        // Foreign key to associate with a player
        public int PlayerId { get; set; }

        [ForeignKey("PlayerId")]
        public Player Player { get; set; }
        
        private List<Card> cards;

        public PlayerHand()
        {
            cards = new List<Card>();
        }
        public void AddCards(List<Card> cards)
        {
            this.cards.AddRange(cards);
        }
        public void Clear()
        {
            cards.Clear();
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
        
        public void AddCardToHand(Card card)
        {
            // Implement logic to add a card to the hand
            cards.Add(card);
        }

        public List<Card> GetCards()
        {
            return new List<Card>(cards);
        }

        public int GetCardCount()
        {
            return cards.Count;
        }

        public bool HasEmptyHand()
        {
            return cards.Count == 0;
        }
    }

    public enum PlayerType
    {
        Human,
        AI
    }

    public class AiPlayer : Player
    {
        private CardDeckLogic _cardDeckLogic;
        private Card _playedCard;
        private Enums.CardColor _chosenColor;
        public AiPlayer(string name, CardDeckLogic cardDeckLogic) : base(name, PlayerType.AI)
        {
            _cardDeckLogic = cardDeckLogic;
            _chosenColor = Hand.GetCardCount() > 0 ? Enums.CardColor.Red : Enums.CardColor.Black;

        }

        public override void PlayCard(Card topCard)
        {
            List<Card> playedCards = new List<Card>();
            _playedCard = ChooseCardToPlay(topCard, playedCards);

            if (_playedCard != null)
            {
                // Check if the selected card is Wild or WildDrawFour
                if (_playedCard.Value == Enums.CardValue.Wild || _playedCard.Value == Enums.CardValue.WildDrawFour)
                {
                    // Use the stored chosen color instead of choosing it again
                    _playedCard.Color = ChooseColorForWildCard();
                }

                // Console.WriteLine($"AI {Name} played a card: {_playedCard}");
                Hand.PlayCard(_playedCard);

                _cardDeckLogic.UpdateCurrentColorAndValue(_playedCard);
            }
            else
            {
                // Draw a card only if no playable card is found
                DrawCardFromDeck(this);

            }
        }
        public Card GetLastPlayedCard()
        {
            return _playedCard;
        }

        public virtual Card ChooseCardToPlay(Card topCard, List<Card> playedCards)
        {
            List<Card> hand = GetCardsInHand();
            bool playableCardsFound = false;

            // Check if there are playable cards in hand
            if (hand.Any(card => card.CanBePlayedOn(topCard)))
            {
                Console.WriteLine("Playable cards found in hand: " + string.Join(", ", hand.Where(card => card.CanBePlayedOn(topCard)).Select(card => card.ToString())));

                playableCardsFound = true;
            }

            if (playableCardsFound)
            {

                // Prioritize playing Wild Draw Four cards if available
                Card wildDrawFourCard = GetWildDrawFourCard();
                if (wildDrawFourCard != null && wildDrawFourCard.CanBePlayedOn(topCard))
                {
                    Console.WriteLine($"Wild Draw Four card found: {wildDrawFourCard}");
                    Enums.CardColor chosenColor = ChooseColorForWildCard();
                    wildDrawFourCard.Color = chosenColor;
                    return wildDrawFourCard;
                }

                // Prioritize playing Wild cards if available
                Card wildCard = GetWildCard();
                if (wildCard != null && wildCard.CanBePlayedOn(topCard))
                {
                    Console.WriteLine($"Wild card found: {wildCard}");
                    Enums.CardColor chosenColor = ChooseColorForWildCard();
                    wildCard.Color = chosenColor;
                    return wildCard;
                }

                // Prioritize playing Skip and Reverse cards if available
                Card skipOrReverseCard = GetSkipOrReverseCard(topCard);
                if (skipOrReverseCard != null)
                {
                    return skipOrReverseCard;
                }

                // Prioritize playing cards of the current color or value
                Card matchingCard = hand.FirstOrDefault(card =>
                    card.CanBePlayedOn(topCard) && (card.Color == topCard.Color || card.Value == topCard.Value) &&
                    !playedCards.Contains(card));

                if (matchingCard != null)
                {
                    return matchingCard;
                }
            }
            if (!playableCardsFound)
            { 
                DrawCardFromDeck(this);
            }

            HandleNoSpecificCardAvailable();

            return null;
        }

        private void HandleNoSpecificCardAvailable()
        {
            if (_cardDeckLogic.IsDeckEmpty())
            {
                Console.WriteLine($"{Name}'s turn is skipped. The deck is empty, and no playable cards are available.");
                SkipTurn();
            }
            else
            {
                Console.WriteLine($"{Name} cannot play a specific card. Drawing a card from the deck...");
                DrawCardFromDeck(this);

            }
        }
        public Enums.CardColor ChooseColorForWildCard()
        {
            List<Card> cardsInHand = GetCardsInHand();

            if (cardsInHand == null || cardsInHand.Count == 0)
            {
                Console.WriteLine($"AI {Name}'s hand is empty or null.");
                return Enums.CardColor.Red; 
            }

            // Count the occurrences of each color only once per turn
            if (_chosenColor == Enums.CardColor.Black)
            {
                var colorOccurrences = cardsInHand
                    .Where(card => card.Color != Enums.CardColor.Black) // Exclude cards with no color (Wild cards)
                    .GroupBy(card => card.Color)
                    .ToDictionary(group => group.Key, group => group.Count());

                // Find the color with the maximum occurrences
                _chosenColor = colorOccurrences.OrderByDescending(kv => kv.Value).FirstOrDefault().Key;

                Console.WriteLine($"Choosing color based on the most frequent color in hand: {_chosenColor}");
            }

            return _chosenColor;
        }


        private Card GetWildDrawFourCard()
        {
            List<Card> cardsInHand = GetCardsInHand();
            if (cardsInHand != null && cardsInHand.Count > 0)
            {
                return cardsInHand.FirstOrDefault(card => card != null && card.Value == Enums.CardValue.WildDrawFour);
            }

            return null;
        }

        private Card GetWildCard()
        {
            List<Card> cardsInHand = GetCardsInHand();

            if (cardsInHand != null && cardsInHand.Count > 0)
            {
                // Filter out null cards and cards with a value other than Wild
                return cardsInHand.FirstOrDefault(card => card != null && card.Value == Enums.CardValue.Wild);
            }

            return null;
        }

        private Card GetSkipOrReverseCard(Card topCard)
        {
            List<Card> cardsInHand = GetCardsInHand();

            if (cardsInHand != null && cardsInHand.Count > 0)
            {
                // Filter out null cards and cards with a value other than Skip or Reverse
                return cardsInHand.FirstOrDefault(card =>
                    (card != null && (card.Value == Enums.CardValue.Skip || card.Value == Enums.CardValue.Reverse)) &&
                    card.CanBePlayedOn(topCard));
            }

            return null;
        }

        private void DrawCardFromDeck(Player currentPlayer)
        {
            List<Card> playedCards = new List<Card>();
            int drawAttempts = 0;
            int maxDrawAttempts = 3; // You can adjust this limit as needed

            while (drawAttempts < maxDrawAttempts)
            {
                Card drawnCard = _cardDeckLogic.DrawCard(currentPlayer);

                if (drawnCard == null)
                {
                    Console.WriteLine($"AI {Name} could not draw a card as the deck is empty.");
                    break; // Exit the loop if the deck is empty
                }

                Console.WriteLine($"AI {Name} drew a card: {drawnCard}");

                // Add the drawn card to the player's hand
                Hand.DrawCard(drawnCard);

                if (_cardDeckLogic.IsPlayableCard(drawnCard))
                {
                    Console.WriteLine($"AI {Name} drew and played a playable card: {drawnCard}");
                    base.PlayCard(drawnCard); // Call the base class's PlayCard method
                    return; // Exit the loop once a playable card is drawn and played
                }
                else
                {
                    Console.WriteLine($"AI {Name} drew an unplayable card: {drawnCard}");
                }

                drawAttempts++;
            }

            // Ensure that the recursive call is made only if no playable card is drawn
            if (drawAttempts == maxDrawAttempts)
            {
                Console.WriteLine($"AI {Name} is drawing another card...");
                DrawCardFromDeck(this);

            }
        }

    }

}