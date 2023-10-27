using System;

namespace UnoGame.GameMenu
{
    public class EndMenu
    {
        public void DisplayEndMessage(string winnerName)
        {
            Console.Clear(); // Clear the console screen
            PrintSeparator(); 
            Console.WriteLine("Uno Game Ended");
            PrintSeparator(); 
            Console.WriteLine($"Winner: {winnerName}\n");
            Console.WriteLine("1. Return to Main Menu");
            Console.WriteLine("2. Exit\n");

            Console.Write("Enter your choice: ");
        }
        
        private void PrintSeparator()
        {
            Console.WriteLine("-----------------------------------------------------------");
        }
    }
}