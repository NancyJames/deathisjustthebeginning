using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HR.Utilities.Events;
using HR.Utilities.Variables;


public class LevelPortal : MonoBehaviour
{
    [SerializeField] GameEvent portalTrigger;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        portalTrigger?.Raise();
    }


}
