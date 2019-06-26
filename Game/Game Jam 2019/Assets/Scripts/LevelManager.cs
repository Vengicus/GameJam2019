using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        LoadLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadLevel()
    {
        Level level = new Level();
        level.LoadLevel("exampleLevel.txt");
    }
}
