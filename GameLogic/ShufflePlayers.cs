using UnoGame.GameObject;

namespace UnoGame.GameLogic;

public class ShufflePlayers
{
    public Player[] Shuffle(Player[] players)
    {
        Random random = new Random();

        // Shuffling the players using the Fisher-Yates shuffle algorithm
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