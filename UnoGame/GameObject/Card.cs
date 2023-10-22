namespace UnoGame.GameObject;

public class Card
{
    public string Color { get; set; }
    public string Value { get; set; }
    public string Score { get; set; }

    public string DisplayValue
    {
        get
        {
            if (Value == "Wild") // Assuming "Wild" is a string in your context
            {
                return Value;
            }
            return Color + " " + Value;
        }
    }
}