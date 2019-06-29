using System.ComponentModel;

public enum LevelTileBehavior
{
    [Description("-")]
    Nothing = 0,
    [Description("1")]
    Floor,
    [Description("2")]
    Wall,
    [Description("3")]
    WallSide,
    [Description("p")]
    PlayerStart,
    [Description("e")]
    EnemySmall,
    [Description("E")]
    EnemyIntermediate,
    [Description("b")]
    EnemyLarge,
    [Description("B")]
    EnemyBoss,
    [Description("t")]
    Trap,
    [Description("v")]
    LightWallDown,
    [Description("^")]
    LightWallUp,
    [Description("<")]
    LightWallLeft,
    [Description(">")]
    LightWallRight,
    [Description("i")]
    Item,
    [Description("k")]
    Key,
    [Description("d")]
    Door,
    // FOR CODE USE, DO NOT DIRECTLY USE
    [Description("*")]
    LitTile_Intensity4,
    [Description("**")]
    LitTile_Intensity3,
    [Description("***")]
    LitTile_Intensity2,
    [Description("****")]
    LitTile_Intensity1,
}