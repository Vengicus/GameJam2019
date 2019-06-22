using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;

public class MapImporter : MonoBehaviour
{

    public string fileNameToLoad;

    public int mapWidth;
    public int mapHeight;

    public GameObject Floor;
    public GameObject Wall;
    public GameObject Door;

    private int[,] tiles;


    void Awake()
    {
        fileNameToLoad = exampleLevel.txt;
        tiles = Load(Application.dataPath + "\\" + fileNameToLoad);

        mapWidth = 51;
        mapHeight = 16;

    BuildMap();
    }

    void BuildMap()
    {
        Debug.Log("Building Map...");
        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                if (tiles[i, j] == 0)
                {
                    GameObject TilePrefab = TilePrefab = Instantiate(Floor, new Vector3(j - mapWidth, mapHeight - i, 0), Quaternion.identity) as GameObject;
                    TilePrefab.transform.parent = GameObject.FindGameObjectWithTag("Room").transform;

                }
                else
                if (tiles[i, j] == 1)
                {
                    GameObject TilePrefab = TilePrefab = Instantiate(Wall, new Vector3(j - mapWidth, mapHeight - i, 1), Quaternion.identity) as GameObject;
                    TilePrefab.transform.parent = GameObject.FindGameObjectWithTag("Room").transform;
                }
                else
                if (tiles[i, j] == 2)
                {
                    GameObject TilePrefab = TilePrefab = Instantiate(Door, new Vector3(j - mapWidth, mapHeight - i, 2), Quaternion.identity) as GameObject;
                    TilePrefab.transform.parent = GameObject.FindGameObjectWithTag("Room").transform;
                }
            }
        }
        Debug.Log("Building Completed!");
    }

    private int[,] Load(string filePath)
    {
        try
        {
            Debug.Log("Loading File...");
            using (StreamReader sr = new StreamReader(filePath))
            {
                string input = sr.ReadToEnd();
                string[] lines = input.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
                int[,] tiles = new int[lines.Length, mapWidth];
                Debug.Log("Parsing...");
                for (int i = 0; i < lines.Length; i++)
                {
                    string st = lines[i];
                    string[] nums = st.Split(new[] { ',' });
                    if (nums.Length != mapWidth)
                    {

                    }
                    for (int j = 0; j < Mathf.Min(nums.Length, mapWidth); j++)
                    {
                        int val;
                        if (int.TryParse(nums[j], out val))
                        {
                            tiles[i, j] = val;
                        }
                        else
                        {
                            tiles[i, j] = 1;
                        }
                    }
                }
                Debug.Log("Parsing Completed!");
                return tiles;
            }
        }
        catch (IOException e)
        {
            Debug.Log(e.Message);
        }
        return null;
    }
}
