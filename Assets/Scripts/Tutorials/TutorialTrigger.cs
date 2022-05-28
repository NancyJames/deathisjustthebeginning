using HR.Utilities.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] TutorialTip_SO tip;
    [SerializeField] GameEvent tipTriggered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !tip.HasBeenSeen())
        {
            GameManager.instance.SetCurrentTip(tip);
            tipTriggered?.Raise();
        }
    }
}
