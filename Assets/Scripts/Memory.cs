using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HR.Utilities.Events;

public class Memory : MonoBehaviour
{
    [SerializeField] bool isTrap;
    [SerializeField] GameEvent goodMemoryEvent;
    [SerializeField] GameEvent badMemoryEvent;
    [SerializeField] Color visitedColor;
    bool alreadyTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!alreadyTriggered && collision.CompareTag("Player"))
        {
            GetComponent<SpriteRenderer>().color = visitedColor;
            alreadyTriggered = true;
            if (isTrap)
            {
                badMemoryEvent?.Raise();
                Collider2D platform = Physics2D.OverlapCircle(collision.transform.position, 4f, LayerMask.GetMask("Ground"));
                if (platform != null && platform.TryGetComponent(out RotatingPlatform trap))
                {
                    trap.TriggerTrap();
                }
            }
            else
            {
                goodMemoryEvent?.Raise();
            }
            
        }
    }
}
