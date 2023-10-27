using UnoGame.GameObject;
using UnoGame.GameMenu;
namespace UnoGame.GameLogic;

public class PlayerTurnLogic
{
    private CardDeckLogic cardDeckLogic;
    private Player[] players;
    private WinningLogic winningLogic;
    private int currentPlayerIndex;
    private CoreLogic gameLogic; 
    private EndMenu endMenu; 
    
    public event Action<Player> GameEnded;
    
    public PlayerTurnLogic(CardDeckLogic cardDeckLogic, Player[] players, WinningLogic winningLogic, EndMenu endMenu)
    {
        this.cardDeckLogic = cardDeckLogic;
        this.players = players;
        this.winningLogic = winningLogic;
        this.endMenu = endMenu;
        this.currentPlayerIndex = 0;
    }

    public void PlayTurn()
    {
        Enums.CardColor currentColor = cardDeckLogic.GetCurrentColor();
        Enums.CardValue currentValue = cardDeckLogic.GetCurrentValue();

        Player currentPlayer = players[currentPlayerIndex];
        Console.WriteLine($"{currentPlayer.Name}'s turn");

        DisplayPlayerHand(currentPlayer);

        currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;
        
        if (WinningLogic.CheckForWin(currentPlayer))
        {
            Console.WriteLine($"{currentPlayer.Name} has won!");
            GameEnded?.Invoke(currentPlayer);
        }
        // Implement the game logic for a player's turn here

        // Update the current player's index, e.g., currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;
    }

    private void DisplayPlayerHand(Player player)
    {
        Console.WriteLine("Your Hand:");
        foreach (Card card in player.Hand.GetCardsInHand())
        {
            Console.WriteLine(card);
        }
    }
}