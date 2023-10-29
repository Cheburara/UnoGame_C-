namespace UnoGame.Rules;
using UnoGame.GameObject;
using UnoGame.GameLogic;
public class TraditionalRules : RulesBase
{
    public TurnDirection.Enums.TurnDirection TurnOrder { get; } = TurnDirection.Enums.TurnDirection.Clockwise;
    public int TotalCardsInDeck { get; set; } = 112;
    public int InitialCardCount { get; set; } = 7;
}
    
    