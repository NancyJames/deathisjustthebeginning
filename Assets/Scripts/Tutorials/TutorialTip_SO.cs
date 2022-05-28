using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial Tip")]
public class TutorialTip_SO : ScriptableObject
{
    [TextArea]
    [SerializeField] string tip;
    [NonSerialized]
    bool beenSeen = false;

    public string GetTip()
    {
        beenSeen = true;
        return tip;
    }

    public bool HasBeenSeen()
    {
        return beenSeen;
    }


}
