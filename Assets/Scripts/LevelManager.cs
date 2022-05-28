using HR.Utilities.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

enum Level
{
    Depression,
    Denial,
    Anger,
    Regret,
    Acceptance
}

public class LevelManager : MonoBehaviour
{
    [SerializeField] Level[] levelsToMoveTo;
    [SerializeField] Level levelExit;
    
    
    SceneLoader sceneLoader;

    private void Awake()
    {       
        sceneLoader = FindObjectOfType<SceneLoader>();
    }


    public void LoadNextLevel()
    {
        sceneLoader.LoadLevel(levelExit.ToString());
    }

    public void LoadRandomLevel()
    {
        int index = Random.Range(0, levelsToMoveTo.Length);
        sceneLoader.LoadLevel(levelsToMoveTo[index].ToString());
    }
}
