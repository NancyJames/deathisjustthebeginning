using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HR.Utilities.Variables;

public class PersistantVariables : MonoBehaviour
{
    //this keeps the scriptable objects that aren't used in each scene in memory so their state doesn't reset
    [SerializeField] IntVariable[] levelMonitors;
    [SerializeField] StoryPoint_SO[] storyTriggers;
    [SerializeField] FloatVariable[] playerStats;

    public void ResetGame()
    {
        Time.timeScale = 1;
        foreach(StoryPoint_SO story in storyTriggers)
        {
            story.ResetBeenSeen();
        }
        foreach(IntVariable levelMonitor in levelMonitors)
        {
            levelMonitor.Set(0);
        }

        foreach(FloatVariable stat in playerStats)
        {
            //reset variable
            stat.OnAfterDeserialize();
        }
    }
}
