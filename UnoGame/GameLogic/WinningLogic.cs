using UnoGame.GameObject;

namespace UnoGame.GameLogic
{
     public class WinningLogic
    {
        public static bool CheckForWin(Player player)
        {
            if (player.Hand.HasEmptyHand())
            {
                Console.WriteLine($"{player.Name} has won!");
                return true;
            }
            return false;
        }
    }
}