using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : GlobalGameManager
{
    private Level level;
    public Level Level
    {
        get
        {
            if(level == null)
            {
                level = GetComponent<Level>();
            }
            return level;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        LoadLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLevel()
    {
        Level.LoadLevel("exampleLevel.txt");
    }

    public void LoadNewLevel()
    {
        Nullify();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name, LoadSceneMode.Single);
    }
}
