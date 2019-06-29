using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private int x, y;
    public int XPos
    {
        get { return x; }
    }
    public int YPos
    {
        get { return y; }
    }

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
        x = (int)position.x;
        y = (int)position.y;

        SetBehavior(behavior);
    }

    public void SetBehavior(LevelTileBehavior behavior)
    {
        this.behavior = behavior;
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Vector2 position = new Vector2(XPos, -YPos);

        switch (behavior)
        {
            case LevelTileBehavior.Nothing:
                renderer.sprite = null;
                GetComponent<BoxCollider2D>().isTrigger = true;
                tag = Tags.TILE;
                break;
            case LevelTileBehavior.Floor:
                // We have no "floor" prefab yet
                break;
            case LevelTileBehavior.Wall:
                renderer.sprite = FileLoader.TileSprites.Where(spr => spr.name == "frontwall").First();
                tag = Tags.WALL;
                break;
            case LevelTileBehavior.WallSide:
            case LevelTileBehavior.LightWallRight:
            case LevelTileBehavior.LightWallLeft:
                renderer.sprite = FileLoader.TileSprites.Where(spr => spr.name == "sidewall").First();
                tag = Tags.WALL;
                break;
            case LevelTileBehavior.PlayerStart:
                tag = Tags.TILE;
                renderer.sprite = null;
                GetComponent<BoxCollider2D>().isTrigger = true;
                Instantiate(FileLoader.PlayerPrefab, new Vector3(position.x, position.y, -3), Quaternion.identity);
                break;
            case LevelTileBehavior.EnemySmall:
            case LevelTileBehavior.EnemyIntermediate:
            case LevelTileBehavior.EnemyLarge:
            case LevelTileBehavior.EnemyBoss:
                tag = Tags.TILE;
                renderer.sprite = null;
                GetComponent<BoxCollider2D>().isTrigger = true;
                Instantiate(GetEnemyPrefab(behavior), new Vector3(position.x, position.y, -3), Quaternion.identity);
                break;
            case LevelTileBehavior.Trap:
                // Trap tile logic
                break;
            case LevelTileBehavior.LightWallDown:
            case LevelTileBehavior.LightWallUp:
                renderer.sprite = FileLoader.TileSprites.Where(spr => spr.name == "lightwall").First();
                break;
            case LevelTileBehavior.Item:
                // Item tile logic
                tag = Tags.TILE;
                break;
            case LevelTileBehavior.Key:
                tag = Tags.TILE;
                renderer.sprite = null;
                GetComponent<BoxCollider2D>().isTrigger = true;
                Instantiate(FileLoader.KeyPrefab, new Vector3(position.x, position.y, -1), Quaternion.identity);
                break;
            case LevelTileBehavior.Door:
                renderer.sprite = null;
                Vector3 positionAndLayering = new Vector3(position.x, position.y, -1);
                GameObject doorPref = Instantiate(FileLoader.DoorPrefab, positionAndLayering, Quaternion.identity);
                doorPref.transform.parent = transform;
                break;
            case LevelTileBehavior.LitTile_Intensity1:
                renderer.sprite = FileLoader.TileSprites.Where(spr => spr.name == "lightIntensity1").First();
                tag = Tags.RELOAD_ZONE;
                break;
            case LevelTileBehavior.LitTile_Intensity2:
                renderer.sprite = FileLoader.TileSprites.Where(spr => spr.name == "lightIntensity2").First();
                tag = Tags.TILE;
                break;
            case LevelTileBehavior.LitTile_Intensity3:
                tag = Tags.TILE;
                renderer.sprite = FileLoader.TileSprites.Where(spr => spr.name == "lightIntensity3").First();
                break;
            case LevelTileBehavior.LitTile_Intensity4:
                tag = Tags.TILE;
                renderer.sprite = FileLoader.TileSprites.Where(spr => spr.name == "lightIntensity4").First();
                break;
            default: break;
        }
    }

    public void AssignLightTile(LevelTileBehavior lightIntensity)
    {
        // Only place light on empty tiles
        if(EvaluateEmptyTile())
        {
            // We don't want to make a brighter tile dark, so make sure that we only override behavior if it is a bright boy
            if ((int)lightIntensity >= (int)Behavior)
                SetBehavior(lightIntensity);
        }
    }

    private bool EvaluateEmptyTile()
    {
        if (Behavior == LevelTileBehavior.Nothing ||
            Behavior == LevelTileBehavior.Key ||
            Behavior == LevelTileBehavior.EnemySmall ||
            Behavior == LevelTileBehavior.EnemyIntermediate ||
            Behavior == LevelTileBehavior.EnemyLarge ||
            Behavior == LevelTileBehavior.EnemyBoss ||
            Behavior == LevelTileBehavior.PlayerStart ||
            Behavior == LevelTileBehavior.Item ||
            Behavior == LevelTileBehavior.LitTile_Intensity1 ||
            Behavior == LevelTileBehavior.LitTile_Intensity2 ||
            Behavior == LevelTileBehavior.LitTile_Intensity3 ||
            Behavior == LevelTileBehavior.LitTile_Intensity4)
        {
            return true;
        }
        return false;
    }

    
    
    private GameObject GetEnemyPrefab(LevelTileBehavior tileType, string name = "")
    {
        EnemyType enemyType = EnemyType.Small;
        switch(tileType)
        {
            case LevelTileBehavior.EnemyIntermediate:
                enemyType = EnemyType.Medium;
                break;
            case LevelTileBehavior.EnemyLarge:
                enemyType = EnemyType.Large;
                break;
            case LevelTileBehavior.EnemyBoss:
                enemyType = EnemyType.Boss;
                break;
        }
        /*if(!string.IsNullOrWhiteSpace(name))
        {
            // Once we can specify enemies put logic here
        }*/
        return FileLoader.GetRandomEnemyPrefab(enemyType);
    }

}
