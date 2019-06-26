using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// Represents a level
/// </summary>
public class Level : MonoBehaviour
{

    public List<List<GameObject>> Tiles { get; set; }

    // GENERATE TILES
    // If it's not a regular tile, i.e. an enemy etc, generate a regular tile floor there
    // Regular tiles = numeric
    // Special tiles = alphabetic

    public void LoadLevel(string levelFileName)
    {
        Tiles = new List<List<GameObject>>();

        string levelFilePath = string.Concat(FileLoader.GAME_DATA_PATH, "/", levelFileName);
        List<string> data = FileLoader.ReadFileAllLines(levelFilePath);
        var enumVals = Enum.GetValues(typeof(LevelTileBehavior)).Cast<LevelTileBehavior>().ToList();

        GameObject tilesParentObject = new GameObject("Tiles");

        int xPos = 0, yPos = 0;
        foreach (string line in data)
        {
            List<GameObject> rowOfTiles = new List<GameObject>();
            foreach(char text in line)
            {
                string tileText = text.ToString();
                LevelTileBehavior tileBehavior = enumVals.Where(behavior => behavior.GetDescription() == tileText).First();
                Vector2 position = new Vector2(yPos, -xPos);
                // GENERATE NEW TILE HERE //
                GameObject newTile = Instantiate(FileLoader.TilePrefab, position, Quaternion.identity) as GameObject;
                Tile tileScript = newTile.GetComponent<Tile>();
                tileScript.Initialize(tileBehavior, position);
                newTile.transform.parent = tilesParentObject.transform;
                rowOfTiles.Add(newTile);
                yPos++;
            }
            xPos++;
            yPos = 0;
            Tiles.Add(rowOfTiles);
        }
    }
}
