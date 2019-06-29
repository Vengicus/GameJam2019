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
    public List<List<Tile>> Tiles { get; set; }

    private Dictionary<Tile, LevelTileBehavior> previousTiles;

    // GENERATE TILES
    // If it's not a regular tile, i.e. an enemy etc, generate a regular tile floor there
    // Regular tiles = numeric
    // Special tiles = alphabetic

    public void LoadLevel(string levelFileName)
    {
        Tiles = new List<List<Tile>>();

        string levelFilePath = string.Concat(FileLoader.GAME_DATA_PATH, "/", levelFileName);
        List<string> data = FileLoader.ReadFileAllLines(levelFilePath);
        
        var enumVals = Enum.GetValues(typeof(LevelTileBehavior)).Cast<LevelTileBehavior>().ToList();

        GameObject tilesParentObject = new GameObject("Tiles");

        int xPos = 0, yPos = 0;
        foreach (string line in data)
        {
            List<Tile> rowOfTiles = new List<Tile>();
            foreach(char text in line)
            {
                string tileText = text.ToString();
                LevelTileBehavior tileBehavior = enumVals.Where(behavior => behavior.GetDescription() == tileText).First();
                Vector2 position = new Vector2(xPos, yPos);
                // GENERATE NEW TILE HERE //
                GameObject newTile = Instantiate(FileLoader.TilePrefab, new Vector3(position.x, -position.y, 0), Quaternion.identity) as GameObject;
                Tile tileScript = newTile.GetComponent<Tile>();
                tileScript.Initialize(tileBehavior, position);
                newTile.transform.parent = tilesParentObject.transform;
                rowOfTiles.Add(tileScript);
                xPos++;
            }
            yPos++;
            xPos = 0;
            Tiles.Add(rowOfTiles);
        }
        SetupLightSources();
    }

    private void SetupLightSources()
    {
        List<Tile> lights = Tiles.SelectMany((list) => list)
            .Where(listItem => listItem.Behavior == LevelTileBehavior.LightWallUp ||
                                listItem.Behavior == LevelTileBehavior.LightWallDown ||
                                listItem.Behavior == LevelTileBehavior.LightWallLeft ||
                                listItem.Behavior == LevelTileBehavior.LightWallRight).ToList();
        foreach(Tile light in lights)
        {
            // We need the 6 blocks in front SO LONG AS THEY ARE EMPTY
            // Block immediately in front is brightest
            // Three blocks adjacent to brightest tile are one less dark
            // Two blocks diagonal to brightest tile are two less dark
            int xIndex = light.XPos,
                yIndex = light.YPos;

            int xDir = 0, yDir = 0;
            switch(light.Behavior)
            {
                case LevelTileBehavior.LightWallUp:
                    yDir = -1;
                    break;
                case LevelTileBehavior.LightWallDown:
                    yDir = 1;
                    break;
                case LevelTileBehavior.LightWallLeft:
                    xDir = -1;
                    break;
                case LevelTileBehavior.LightWallRight:
                    xDir = 1;
                    break;
            }
            // Wall is pointing in an x direction
            if(xDir != 0)
            {
                Tiles[yIndex][xIndex + (1 * xDir)].AssignLightTile(LevelTileBehavior.LitTile_Intensity1);
                Tiles[yIndex - 1][xIndex + (1 * xDir)].AssignLightTile(LevelTileBehavior.LitTile_Intensity2);
                Tiles[yIndex + 1][xIndex + (1 * xDir)].AssignLightTile(LevelTileBehavior.LitTile_Intensity2);
                Tiles[yIndex][xIndex + (2 * xDir)].AssignLightTile(LevelTileBehavior.LitTile_Intensity2);
                Tiles[yIndex - 1][xIndex + (2 * xDir)].AssignLightTile(LevelTileBehavior.LitTile_Intensity2);
                Tiles[yIndex + 1][xIndex + (2 * xDir)].AssignLightTile(LevelTileBehavior.LitTile_Intensity2);
            }
            // Wall is pointing in a y direction
            else
            {
                Tiles[yIndex + (1 * yDir)][xIndex].AssignLightTile(LevelTileBehavior.LitTile_Intensity1);
                Tiles[yIndex + (1 * yDir)][xIndex - 1].AssignLightTile(LevelTileBehavior.LitTile_Intensity2);
                Tiles[yIndex + (1 * yDir)][xIndex + 1].AssignLightTile(LevelTileBehavior.LitTile_Intensity2);
                Tiles[yIndex + (2 * yDir)][xIndex].AssignLightTile(LevelTileBehavior.LitTile_Intensity2);
                Tiles[yIndex + (2 * yDir)][xIndex - 1].AssignLightTile(LevelTileBehavior.LitTile_Intensity2);
                Tiles[yIndex + (2 * yDir)][xIndex + 1].AssignLightTile(LevelTileBehavior.LitTile_Intensity2);
            }
            
        }
    }

    public void EmitPlayerAura(Tile currentPlayerTile/*, int radius*/)
    {
        List<LevelTileBehavior> lightIntensities = new List<LevelTileBehavior>() { LevelTileBehavior.LitTile_Intensity2, LevelTileBehavior.LitTile_Intensity3 };

        /*switch(radius)
        {
            case 1:
                lightIntensities.AddRange(new List<LevelTileBehavior>() );
                break;
            //case 2:
            //case 3:
                //lightIntensities.AddRange(new List<LevelTileBehavior>() { LevelTileBehavior.LitTile_Intensity1, LevelTileBehavior.LitTile_Intensity2, LevelTileBehavior.LitTile_Intensity3, LevelTileBehavior.LitTile_Intensity4 });
                //break;
        }*/


        ResetOriginalTileBehaviors();
        int currentX = currentPlayerTile.XPos;
        int currentY = currentPlayerTile.YPos;
        previousTiles = new Dictionary<Tile, LevelTileBehavior>();
        
        AssignLight(currentY, currentX, lightIntensities[0]);
        AssignLight(currentY - 1, currentX, lightIntensities[1]);
        AssignLight(currentY + 1, currentX, lightIntensities[1]);
        AssignLight(currentY, currentX - 1, lightIntensities[1]);
        AssignLight(currentY, currentX + 1, lightIntensities[1]);
        AssignLight(currentY - 1, currentX - 1, lightIntensities[1]);
        AssignLight(currentY + 1, currentX - 1, lightIntensities[1]);
        AssignLight(currentY + 1, currentX + 1, lightIntensities[1]);
        AssignLight(currentY - 1, currentX + 1, lightIntensities[1]);
        
        /*if(radius >= 2)
        {

        }
        if(radius >= 3)
        {

        }*/
        
    }

    private void AssignLight(int yIndex, int xIndex, LevelTileBehavior intensity)
    {
        if(yIndex >= 0 && xIndex >= 0)
        {
            previousTiles.Add(Tiles[yIndex][xIndex], Tiles[yIndex][xIndex].Behavior);
            Tiles[yIndex][xIndex].AssignLightTile(intensity);
        }
    }

    private void ResetOriginalTileBehaviors()
    {
        if (previousTiles != null)
        {
            foreach (KeyValuePair<Tile, LevelTileBehavior> prevTile in previousTiles)
            {
                if (prevTile.Value != LevelTileBehavior.LitTile_Intensity1 &&
                   prevTile.Value != LevelTileBehavior.LitTile_Intensity2 &&
                   prevTile.Value != LevelTileBehavior.LitTile_Intensity3 &&
                   prevTile.Value != LevelTileBehavior.LitTile_Intensity4 &&
                   prevTile.Value != LevelTileBehavior.Wall &&
                   prevTile.Value != LevelTileBehavior.WallSide &&
                   prevTile.Value != LevelTileBehavior.LightWallDown &&
                   prevTile.Value != LevelTileBehavior.LightWallUp &&
                    prevTile.Value != LevelTileBehavior.LightWallLeft &&
                   prevTile.Value != LevelTileBehavior.LightWallRight)
                    prevTile.Key.SetBehavior(LevelTileBehavior.Nothing);
                else
                    prevTile.Key.SetBehavior(prevTile.Value);
                Tiles[prevTile.Key.YPos][prevTile.Key.XPos] = prevTile.Key;
            }
        }
    }
}
