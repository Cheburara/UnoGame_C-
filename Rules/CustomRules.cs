namespace UnoGame.Rules;
using UnoGame.GameObject;

public class CustomRules
{
    public TurnDirection.Enums.TurnDirection TurnOrder { get; set; }

    public void Display()
    {
        Console.WriteLine("Select turn direction for custom rules:");
        Console.WriteLine("1. Clockwise");
        Console.WriteLine("2. Counterclockwise");
        int turnDirectionChoice = GetUserChoice(1, 2);

        if (turnDirectionChoice == 1)
        {
            TurnOrder = TurnDirection.Enums.TurnDirection.Clockwise;
        }
        else
        {
            TurnOrder = TurnDirection.Enums.TurnDirection.Counterclockwise;
        }
    }

    private int GetUserChoice(int min, int max)
        {
            int choice;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out choice) && choice >= min && choice <= max)
                {
                    return choice;
                }
                Console.WriteLine("Invalid input. Please enter a valid choice.");
            }
    }
}