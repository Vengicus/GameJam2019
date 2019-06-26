using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class FileLoader
{
    private static GameObject tilePrefab;
    public static GameObject TilePrefab
    {
        get
        {
            if (tilePrefab == null)
            {
                tilePrefab = Resources.Load("Prefabs/testTile", typeof(GameObject)) as GameObject;
            }
            return tilePrefab;
        }
    }

    private static GameObject playerPrefab;
    public static GameObject PlayerPrefab
    {
        get
        {
            if (playerPrefab == null)
            {
                playerPrefab = Resources.Load("Prefabs/Player", typeof(GameObject)) as GameObject;
            }
            return playerPrefab;
        }
    }

    private static GameObject enemyPrefab;
    public static GameObject EnemyPrefab
    {
        get
        {
            if (enemyPrefab == null)
            {
                enemyPrefab = Resources.Load("Prefabs/Enemy", typeof(GameObject)) as GameObject;
            }
            return enemyPrefab;
        }
    }

    private static List<Sprite> tileSprites;
    public static List<Sprite> TileSprites
    {
        get
        {
            if (tileSprites == null)
            {
                tileSprites = new List<Sprite>(Resources.LoadAll("Sprites/Tiles", typeof(Sprite)).Cast<Sprite>());
            }
            return tileSprites;
        }
    }


    public static string GAME_DATA_PATH
    {
        get
        {
            return Application.dataPath;
        }
    }

    public static List<string> ReadFileAllLines(string pathToFile)
    {
        List<string> output = new List<string>();
        using(StreamReader reader = new StreamReader(pathToFile))
        {
            string data = reader.ReadToEnd();
            reader.Close();
            output.AddRange(data.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries));
        }
        return output;
    }
}
