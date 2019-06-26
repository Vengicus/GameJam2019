using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private LevelTileBehavior behavior;
    public LevelTileBehavior Behavior
    {
        get
        {
            return behavior;
        }
    }

    // INSERT ENEMIES HERE IF THEY SHOULD BE ADDED

    public void Initialize(LevelTileBehavior behavior, Vector2 position)
    {
        this.behavior = behavior;

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        
        switch(behavior)
        {
            case LevelTileBehavior.Nothing:
                renderer.sprite = null;
                Destroy(GetComponent<BoxCollider2D>());
                break;
            case LevelTileBehavior.Floor:
                renderer.sprite = FileLoader.TileSprites.Where(spr => spr.name == "frontwall").First();
                break;
            case LevelTileBehavior.Wall:
                renderer.sprite = FileLoader.TileSprites.Where(spr => spr.name == "sidewall").First();
                break;
            case LevelTileBehavior.WallSide:

                break;
            case LevelTileBehavior.PlayerStart:
                renderer.sprite = null;
                Destroy(GetComponent<BoxCollider2D>());
                Instantiate(FileLoader.PlayerPrefab, position, Quaternion.identity);
                break;
            case LevelTileBehavior.EnemySmallOrIntermediate:
                renderer.sprite = null;
                Destroy(GetComponent<BoxCollider2D>());
                Instantiate(FileLoader.EnemyPrefab, position, Quaternion.identity);
                break;
            case LevelTileBehavior.EnemyLarge:

                break;
            case LevelTileBehavior.Trap:

                break;
            case LevelTileBehavior.LightStart:

                break;
            case LevelTileBehavior.LightEnd:

                break;
            case LevelTileBehavior.Item:

                break;
            case LevelTileBehavior.Key:

                break;
            case LevelTileBehavior.Door:

                break;
            default: break;
        }
    }
}
