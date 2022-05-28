using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HR.Utilities.Variables;

public class PowerUp : MonoBehaviour
{
    [SerializeField] FloatVariable boostedStat;
    [SerializeField] float amount;
    [SerializeField] FloatVariable debuffedStat;
    [SerializeField] float debuffAmount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            boostedStat.Increment(amount);
            if(debuffedStat!=null)
            {
                debuffedStat.Decrement(debuffAmount);
            }
            Destroy(gameObject);
        }
    }
}
