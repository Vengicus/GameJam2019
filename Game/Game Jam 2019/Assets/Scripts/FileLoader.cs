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
                tilePrefab = Resources.Load("Prefabs/Tile", typeof(GameObject)) as GameObject;
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

    private static Dictionary<EnemyType, List<GameObject>> enemyPrefabs;
    public static Dictionary<EnemyType, List<GameObject>> EnemyPrefabs
    {
        get
        {
            if (enemyPrefabs == null)
            {
                enemyPrefabs = new Dictionary<EnemyType, List<GameObject>>();
                foreach(EnemyType val in System.Enum.GetValues(typeof(EnemyType)))
                {
                    enemyPrefabs.Add(val, new List<GameObject>());
                }
                List<GameObject> prefabs = new List<GameObject>(Resources.LoadAll("Prefabs/Enemies", typeof(GameObject)).Cast<GameObject>());
                
                foreach(GameObject obj in prefabs)
                {
                    EnemyController control = obj.GetComponent<EnemyController>();
                    if (control != null)
                    {
                        EnemyType type = control.EnemyType;
                        enemyPrefabs[type].Add(obj);
                    }
                }
            }
            return enemyPrefabs;
        }
    }

    private static GameObject keyPrefab;
    public static GameObject KeyPrefab
    {
        get
        {
            if (keyPrefab == null)
            {
                keyPrefab = Resources.Load("Prefabs/Key", typeof(GameObject)) as GameObject;
            }
            return keyPrefab;
        }
    }

    private static GameObject playerBulletPrefab;
    public static GameObject PlayerBulletPrefab
    {
        get
        {
            if (playerBulletPrefab == null)
            {
                playerBulletPrefab = Resources.Load("Prefabs/Projectiles/PlayerBullet", typeof(GameObject)) as GameObject;
            }
            return playerBulletPrefab;
        }
    }

    private static GameObject doorPrefab;
    public static GameObject DoorPrefab
    {
        get
        {
            if (doorPrefab == null)
            {
                doorPrefab = Resources.Load("Prefabs/Door", typeof(GameObject)) as GameObject;
            }
            return doorPrefab;
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
        using (StreamReader reader = new StreamReader(pathToFile))
        {
            string data = reader.ReadToEnd();
            reader.Close();
            output.AddRange(data.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries));
        }
        return output;
    }

    public static GameObject GetRandomEnemyPrefab(EnemyType type)
    {
        if (EnemyPrefabs != null && EnemyPrefabs.Count > 0 && EnemyPrefabs[type] != null)
        {
            List<GameObject> applicableEnemies = EnemyPrefabs[type];
            int randomEnemyIndex = Random.Range(0, applicableEnemies.Count);
            return applicableEnemies[randomEnemyIndex];
        }
        return new GameObject("ERROR NO PREFABS FOUND");
    }
    

    /*
    public static List<string> GetOnlyLevelData(this List<string> data)
    {
        return data.Where(line => !line.Contains("=")).ToList();
    }

    public static string GetTextFileAttribute(this List<string> data, string attributeName)
    {
        return data.Where(line => line.Contains(attributeName)).First().Split('=')[1].Trim();
    }*/
}
