using System;

namespace UnoGame.GameMenu
{
    public class MainMenu
    {
        public void Display()
        {
            Console.Clear();
            PrintSeparator(); 
            Console.WriteLine("Welcome to the UNO Game!");
            PrintSeparator(); 
            Console.WriteLine("1. Start New Game");
            Console.WriteLine("2. Load Game");
            Console.WriteLine("3. Exit");
            PrintSeparator(); 
        }

        public int GetUserChoice(int min, int max)
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

        private void PrintSeparator()
        {
            Console.WriteLine("-----------------------------------------------------------");
        }
    }
}