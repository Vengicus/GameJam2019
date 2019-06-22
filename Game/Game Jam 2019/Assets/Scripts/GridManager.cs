using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class GridManager : MonoBehaviour
{

    public GameObject floor_tile;
    public GameObject level;

    // Use this for initialization
    void Start()
    {
        string[][] textFile = ReadLevel("exampleLevel.txt");
        float x = level.transform.position.x - level.GetComponent<Renderer>().bounds.size.x / 2;
        float y = level.transform.position.y - level.GetComponent<Renderer>().bounds.size.y / 2 - 0.7f;

        LayoutGrid(textFile, x, y);

    }

    // Update is called once per frame
    void Update()
    {

    }

    /*
     * Reads in the level from a text file
     * Code from http://answers.unity3d.com/questions/577889/create-level-based-on-xmltxt-file.html
     * */
    string[][] ReadLevel(string file)
    {
        string levelText = System.IO.File.ReadAllText(file);
        string[] fileLines = Regex.Split(levelText, "\r\n");
        int rows = fileLines.Length;

        string[][] level = new string[rows][];
        for (int i = 0; i < fileLines.Length; i++)
        {
            string[] lineInfo = Regex.Split(fileLines[i], " ");
            level[i] = lineInfo;
        }
        return level;
    }

    // Reads the text files in (upside down)
    void LayoutGrid(string[][] textFile, float startX, float startY)
    {
        for (int i = 0; i < textFile.Length; i++)
        {
            for (int j = 0; j < textFile[i].Length; j++)
            {
                Instantiate(floor_tile);
                floor_tile.transform.position = new Vector3(startX + j * 2, startY + i * 2, -4);
                if (textFile[i][j] == "1")
                {
                    floor_tile.GetComponent<SpriteRenderer>().color = Color.gray;
                    Color tmp = floor_tile.GetComponent<SpriteRenderer>().color;
                    tmp.a = 0.2f;
                    floor_tile.GetComponent<SpriteRenderer>().color = tmp;
                }
                else if (textFile[i][j] == "0")
                {
                    floor_tile.GetComponent<SpriteRenderer>().color = Color.black;
                    Color tmp = floor_tile.GetComponent<SpriteRenderer>().color;
                    tmp.a = 0.0f;
                    floor_tile.GetComponent<SpriteRenderer>().color = tmp;
                }

            }
        }
    }
}

