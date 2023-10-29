using UnoGame.GameLogic;
namespace UnoGame.GameObject
{
    public class Player
    {
        public string Name { get; set; }
        public PlayerType Type { get; set; }
        
        public PlayerHand Hand { get; set; }
        
        public Player(string name, PlayerType type)
        {
            Name = name;
            Type = type;
            Hand = new PlayerHand();
        }
    }

    public enum PlayerType
    {
        Human,
        AI
    }
}