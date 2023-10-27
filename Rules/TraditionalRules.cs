namespace UnoGame.Rules;
using UnoGame.GameObject;
using UnoGame.GameLogic;
public class TraditionalRules : RulesBase
{
    public TurnDirection.Enums.TurnDirection TurnOrder { get; } = TurnDirection.Enums.TurnDirection.Clockwise;
    
    public int InitialCardCount { get; } = 7;
}
    
    