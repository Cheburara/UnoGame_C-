namespace UnoGame.GameObject
{
    public class Player
    {
        public string Name { get; set; }
        public PlayerType Type { get; set; }

        public Player(string name, PlayerType type)
        {
            Name = name;
            Type = type;
        }
    }

    public enum PlayerType
    {
        Human,
        AI
    }
}