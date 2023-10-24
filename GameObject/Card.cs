using System;

namespace UnoGame.GameObject
{
    public class Enums
    {
        public enum CardColor
        {
            Red,
            Blue,
            Yellow,
            Green,
            Black,
        }

        public enum CardValue
        {
            Zero,
            One,
            Two,
            Three,
            Four,
            Five,
            Six,
            Seven,
            Eight,
            Nine,
            Reverse,
            Skip,
            Wild,
            WildDrawFour,
            WildShuffleHands,
            WildCustomizable,
        }

        public enum CardScore
        {
            Zero = 0,
            One = 1,
            Two = 2,
            Three = 3,
            Four = 4,
            Five = 5,
            Six = 6,
            Seven = 7,
            Eight = 8,
            Nine = 9,
            Reverse = 20,
            Skip = 20,
            Wild = 50,
            WildDrawFour = 50,
            WildShuffleHands = 40,
            WildCustomizable = 40,
        }
    }

    public class Card
    {
        public Enums.CardColor Color { get; set; }
        public Enums.CardValue Value { get; set; }
        public Enums.CardScore Score { get; set; }

        public string DisplayValue
        {
            get
            {
                if (Value == Enums.CardValue.Wild)
                {
                    return Value.ToString();
                }
                return Color + " " + Value;
            }
        }
    }
}