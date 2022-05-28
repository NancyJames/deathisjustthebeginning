using HR.Utilities.Variables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffDisplay : MonoBehaviour
{
    [SerializeField] GameObject debuffIcon;
    [SerializeField] IntVariable numDebuffs;
    public void Draw()
    {
        Transform t = transform;
        foreach(Transform child in t)
        {
            Destroy(child.gameObject);
        }
        for(int i =0;i<numDebuffs.Get();i++)
        {
            Instantiate(debuffIcon, transform);
        }
    }
}
