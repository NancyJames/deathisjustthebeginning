using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HR.Utilities.Events;
public class EventTrigger : MonoBehaviour
{
    [SerializeField] GameEvent eventToTrigger;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            eventToTrigger?.Raise();
        }
    }

}
