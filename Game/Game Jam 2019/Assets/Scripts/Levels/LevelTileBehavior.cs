using System.ComponentModel;

public enum LevelTileBehavior
{
    [Description("0")]
    Nothing,
    [Description("1")]
    Floor,
    [Description("2")]
    Wall,
    [Description("3")]
    WallSide,
    [Description("p")]
    PlayerStart,
    [Description("e")]
    EnemySmallOrIntermediate,
    [Description("E")]
    EnemyLarge,
    [Description("t")]
    Trap,
    [Description("l")]
    LightStart,
    [Description("L")]
    LightEnd,
    [Description("i")]
    Item,
    [Description("k")]
    Key,
    [Description("d")]
    Door,
}