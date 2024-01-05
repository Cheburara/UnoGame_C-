using UnoGame.Rules;
using UnoGame.Storage;

namespace UnoGame.GameObject;

public class GameState
{
    public string GameName { get; set; } 
    public StorageType StorageType { get; set; }
    
    public bool UseCustomRules { get; set; }
    public bool UseTraditionalRules { get; set; }
    public TraditionalRules TraditionalRules { get; set; } 
    public CustomRules CustomRules { get; set; }
    
    public TurnDirection.Enums.TurnDirection TurnOrder { get; set; }
    
    public int TotalCardsInDeck { get; set; }
    public int InitialCardCount { get; set; } 
    public int NumberOfPlayers { get; set; }
    public Player[] Players { get; set; }
    public List<Card> Deck { get; set; }
    
    public Card TopCard { get; set; }
    public Dictionary<string, List<Card>> PlayersHands { get; set; }
    
}