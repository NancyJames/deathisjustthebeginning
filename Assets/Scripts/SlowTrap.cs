using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTrap : MonoBehaviour
{
    [SerializeField] float amountToSlow = 1f;
    [SerializeField] float timeToSlow = 3f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().Slow(amountToSlow, timeToSlow);
        }
    }
}
