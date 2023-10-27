namespace UnoGame.Rules;
using UnoGame.GameObject;

public class CustomRules : RulesBase
{
    public TurnDirection.Enums.TurnDirection TurnOrder { get; set; }
    
    public int InitialCardCount { get; set; }

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
        
        Console.WriteLine("Enter the number of initial cards to deal:");
        InitialCardCount = GetUserChoice(1, int.MaxValue);
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