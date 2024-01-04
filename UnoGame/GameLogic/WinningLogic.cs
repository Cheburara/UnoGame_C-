using UnoGame.GameObject;
using System.Collections.Generic;

namespace UnoGame.GameLogic
{
     public class WinningLogic
    {
        
        public static bool CheckForWin(Player player)
        {
            if (player.Hand.HasEmptyHand())
            {
                return true;
            }

            // You can add additional winning conditions based on your game rules here.
            if (CheckUnoCondition(player))
            {
                return true;
            }

            return false;
        }

        private static bool CheckUnoCondition(Player player)
        {
            // Example: Check if the player has one card left and shouts 'Uno!'
            return player.Hand.GetCardCount() == 1;
        }

        public static void AnnounceWinner(Player player)
        {
            Console.WriteLine($"{player.Name} has won!");

            // You can add additional logic here after announcing the winner, if needed.
        }
    }
    }