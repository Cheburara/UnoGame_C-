namespace UnoGame.GameMenu
{
    public class ExitGame
    {
        public void Display()
        {
            Console.Clear();
            Console.WriteLine("Thank you for playing UNO!");
            Console.WriteLine("1. Return to Main Menu");
            Console.WriteLine("2. Exit Game");
            int choice = GetUserChoice(1, 2);

            if (choice == 1)
            {
                // Return to the main menu
            }
            else if (choice == 2)
            {
                Environment.Exit(0); // Exit the game
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
}