using UnoGame.GameObject;
using UnoGame.GameMenu;

namespace UnoGame.GameLogic;

public class ShufflePlayers
{
    public Player[] Shuffle(Player[] players)
    {
        if (players == null)
        {
            Console.WriteLine("Players array is null. Aborting shuffling.");
            return players;
        }
        
        Random random = new Random();
        
        for (int i = players.Length - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);
            Player temp = players[i];
            players[i] = players[j];
            players[j] = temp;
        }

        return players;
    }
}
