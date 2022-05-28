using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    StoryPoint_SO currentStory = null;
    TutorialTip_SO currentTip = null;


    private void Awake()
    {
        if (instance != null)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public StoryPoint_SO GetCurrentStory()
    {
        return currentStory;
    }

    public void SetCurrentStory(StoryPoint_SO story)
    {
        currentStory = story;
    }

    public TutorialTip_SO GetCurrentTip()
    {
        return currentTip;
    }

    public void SetCurrentTip(TutorialTip_SO tip)
    {
        currentTip = tip;
    }

    

}

