using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HR.Utilities.Events;

public class Memory : MonoBehaviour
{
    [SerializeField] bool isTrap;
    [SerializeField] GameEvent goodMemoryEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(isTrap)
            {
                Collider2D platform = Physics2D.OverlapCircle(collision.transform.position, 2f, LayerMask.GetMask("Ground"));
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
