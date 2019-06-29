using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Global Game Manager: Persists acrossed scenes
/// </summary>
public class GlobalGameManager : MonoBehaviour
{
    public static string PLAYER_TAG = "Player";

    public static int PlayerLives = 3;

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
            if (_playerObject == null)
            {
                _playerObject = GameObject.FindGameObjectWithTag(PLAYER_TAG).GetComponent<PlayerController>();
            }
            return _playerObject;
        }
    }

    private static List<GameObject> objectsOnScreenWithColliders;
    public static List<GameObject> ObjectsOnScreenWithColliders
    {
        get
        {
            if(objectsOnScreenWithColliders == null)
            {
                objectsOnScreenWithColliders = GetAllGameObjectsOnScreenWithColliders();
            }
            return objectsOnScreenWithColliders;
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
    /// On start of every new scene
    /// </summary>
    private void Start()
    {
        
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
    

    /// <summary>
    /// Gets all game objects in a scene
    /// </summary>
    /// <returns></returns>
    public static List<GameObject> GetAllGameObjectsOnScreenWithColliders()
    {
        return GetAllGameObjectsOnScreenWithColliders(new List<string>());
    }

    /// <summary>
    /// Gets all game objects in a scene, excluding certain tags to ignore
    /// </summary>
    /// <param name="tagsToIgnore"></param>
    /// <returns></returns>
    public static List<GameObject> GetAllGameObjectsOnScreenWithColliders(List<string> tagsToIgnore)
    {
        // Get a list of ALL objects
        List<GameObject> objects = new List<GameObject>();
        objects.AddRange(FindObjectsOfType(typeof(GameObject)) as GameObject[]);

        // If we 
        List<GameObject> objectsToExclude = new List<GameObject>();
        
        foreach (string tag in tagsToIgnore)
        {
            objectsToExclude.AddRange(objects.Where(obj => obj.tag == tag));
        }

        // We ONLY want objects with BoxCollider2Ds
        // We already excluded a certain set of items, we want to add even more objects, we just want objects with BoxColliders
        objectsToExclude.AddRange(objects.Where(newObj => !objectsToExclude.Any(oldObj => oldObj.Equals(newObj)) 
                                                            && newObj.GetComponent<BoxCollider2D>() == null));
        

        foreach(GameObject obj in objectsToExclude)
        {
            objects.Remove(obj);
        }

        return objects;
    }

    public static void RefreshListingOfObjectsOnScreenWithColliders()
    {
        objectsOnScreenWithColliders = GetAllGameObjectsOnScreenWithColliders();
    }

    /// <summary>
    /// Safely destroy object! Remove references this object might still be attached to, then remove it
    /// </summary>
    /// <param name="objectToDestroy"></param>
    public static void DestroyObject(GameObject objectToDestroy)
    {
        objectsOnScreenWithColliders.Remove(objectToDestroy);
        Destroy(objectToDestroy);
    }

    public static void Nullify()
    {
        objectsOnScreenWithColliders = null;
    }
}
