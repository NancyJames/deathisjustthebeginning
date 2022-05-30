using HR.Utilities.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HR.Utilities.Variables;

enum Level
{
    Depression,
    Denial,
    Anger,
    Regret,
    Acceptance,
    Credits
}

public class LevelManager : MonoBehaviour
{
    [SerializeField] Level[] levelsToMoveTo;
    [SerializeField] Level levelExit;
    [SerializeField] AudioClip music=null;
    [SerializeField] bool dontChangeTrack=false;
    [SerializeField] BoolVariable storyPlaying;
    [SerializeField] GameEvent waitingToLoadEvent;
    [SerializeField] IntVariable highestLevel;
    [SerializeField] IntVariable timesVisited;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] Transform background;
    bool waitingToLoad = false;

    
    
    SceneLoader sceneLoader;

    private void Awake()
    {       
        sceneLoader = FindObjectOfType<SceneLoader>();
       
    }

    public AudioClip GetLevelMusic()
    {
        return music;
    }

    public void MoveToSpawnPoint()
    {
        if(spawnPoints.Length>0)
        {
            bool turnOnTrail=false;
            int index = System.Math.Clamp(timesVisited.Get(), 0, spawnPoints.Length - 1);
            GameObject player = GameObject.FindWithTag("Player");
            TrailRenderer tr = player.GetComponentInChildren<TrailRenderer>();
            if(tr!=null)
            {
                if(tr.enabled)
                {
                    tr.enabled = false;
                    turnOnTrail = true;
                }
            }
            player.transform.position = spawnPoints[index].position;
            if(background!=null)
            {
                background.position = new Vector3(spawnPoints[index].position.x, background.position.y, background.position.z);
            }
            if(turnOnTrail)
            {
                tr.enabled = true;
            }
            
            
        }
        timesVisited.Increment();
    }

    public void OverrideChangeTrack()
    {
        dontChangeTrack = false;
    }

    public bool ChangeTrack()
    {
        return !dontChangeTrack;
    }


    public void LoadNextLevel()
    {
        string levelToLoad;
    
        if(highestLevel.Get()>(int)levelExit)
        {
            //we've visited this level before, return to the last level we visited;
            levelToLoad = ((Level)highestLevel.Get()).ToString();
        }
        else
        {
            levelToLoad = levelExit.ToString();
            highestLevel.Set((int)levelExit);
        }
        sceneLoader.LoadLevel(levelToLoad);
    }

    public void LoadRandomLevel()
    {
        if(storyPlaying.Get())
        {
            waitingToLoad = true;
            waitingToLoadEvent?.Raise();
            return;
        }
        LoadRandomLevelNow();

    }
    void LoadRandomLevelNow()
    {
        waitingToLoad = false;
        int index = Random.Range(0, levelsToMoveTo.Length);
        sceneLoader.LoadLevel(levelsToMoveTo[index].ToString());
    }

    public void ProcessStoryFinished()
    {
        if(waitingToLoad)
        {
            waitingToLoad = false;
            LoadRandomLevelNow();  
        }
    }

    


}
