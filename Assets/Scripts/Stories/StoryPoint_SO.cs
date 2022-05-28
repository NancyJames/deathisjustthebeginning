using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Story Point")]

public class StoryPoint_SO : ScriptableObject
{
    [TextArea]
    [SerializeField] string story;
    [SerializeField] bool isMemory = false;
    [NonSerialized]
    bool beenSeen=false;

    public string GetStory()
    {
        beenSeen = true;
        return story;
    }

    public bool HasBeenSeen()
    {
        return beenSeen;
    }

    public bool IsMemory()
    {
        return isMemory;
    }

    public void ResetBeenSeen()
    {
        beenSeen = false;
    }



    
}
