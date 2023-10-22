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
        }
    }

    public class Card
    {
        public Enums.CardColor Color { get; set; }
        public Enums.CardValue Value { get; set; }
        public int Score { get; set; }

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