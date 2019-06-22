using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Global Game Manager: Persists acrossed scenes
/// </summary>
public class GlobalGameManager : MonoBehaviour
{
    private const string PLAYER_TAG = "Player";

    private static GameState _gameState;
    public static GameState GameState
    {
        get
        {
            return _gameState;
        }
    }

    private static PlayerController _playerObject;
    public static PlayerController PlayerObject
    {
        get
        {
            if(_playerObject == null)
            {
                _playerObject = GameObject.FindGameObjectWithTag(PLAYER_TAG).GetComponent<PlayerController>();
            }
            return _playerObject;
        }
    }


    /// <summary>
    /// Create this script when game begins, this class has access to ALL monobehaviour, and persists across scenes
    /// </summary>
    void Awake()
    {
        UpdateCurrentGameState(GameState.Gameplay);
        DontDestroyOnLoad(this);
    }

    /// <summary>
    /// Update current gamestate
    /// </summary>
    /// <param name="newState"></param>
    public static void UpdateCurrentGameState(GameState newState)
    {
        //Might be used later
        /*if(!GameState.Equals(GameState.Gameplay) && newState.Equals(GameState.Paused))
        {
            throw new System.Exception("Game cannot be paused outside of the gameplay loop");
        }*/
        _gameState = newState;
    }

    /// <summary>
    /// Gets a list of objects that match multiple tags
    /// </summary>
    /// <param name="tags"></param>
    /// <returns></returns>
    public static List<GameObject> FindGameObjectsWithTags(List<string> tags)
    {
        List<GameObject> objects = new List<GameObject>();
        foreach(string tag in tags)
        {
            objects.AddRange(GameObject.FindGameObjectsWithTag(tag));
        }
        return objects;
    }
    

}
