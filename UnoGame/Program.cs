using System;
using UnoGame.GameMenu;

namespace UnoGame
{
    class Program
    {
        
        static void Main(string[] args)
        {
            MainMenu mainMenu = new MainMenu();

            while (true)
            {
                Console.Clear();
                mainMenu.Display();

                int choice = mainMenu.GetUserChoice(1, 3);

                switch (choice)
                {
                    case 1:
                        NewGame newGameMenu = new NewGame();
                        newGameMenu.Display();
                        break;
                    case 2:
                        LoadGame loadGameMenu = new LoadGame();
                        loadGameMenu.Display();
                        break;
                    case 3:
                        ExitGame exitGame = new ExitGame();
                        exitGame.Display();
                        break;
                }
            }
        }
    }
}