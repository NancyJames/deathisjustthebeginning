using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HR.Utilities.Events;

public class Memory : MonoBehaviour
{
    [SerializeField] bool isTrap;
    [SerializeField] GameEvent goodMemoryEvent;
    bool alreadyTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!alreadyTriggered && collision.CompareTag("Player"))
        {
            alreadyTriggered = true;
            if (isTrap)
            {
                Collider2D platform = Physics2D.OverlapCircle(collision.transform.position, 4f, LayerMask.GetMask("Ground"));
                if(platform==null)
                {
                    Debug.Log("no platofmr");
                }
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
