using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HR.Utilities.Variables;

public class PowerUp : MonoBehaviour
{
    [SerializeField] FloatVariable boostedStat;
    [SerializeField] FloatVariable maxBoostedStat;
    [SerializeField] float amount;
    [SerializeField] FloatVariable debuffedStat;
    [SerializeField] float debuffAmount;
    [SerializeField] float respawnTime=0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            float increment = Mathf.Clamp(amount, 0, maxBoostedStat.Get() - boostedStat.Get());
            boostedStat.Increment(increment);
            if(debuffedStat!=null && debuffedStat.Get()>0)
            {
                debuffedStat.Decrement(debuffAmount);
            }
            if(respawnTime==0)
            {
                Destroy(gameObject);
            }
            else
            {
                Invoke(nameof(Respawn), respawnTime);
                gameObject.SetActive(false);
            }
            
           
        }
    }

    private void Respawn()
    {

        gameObject.SetActive(true);
    }


}
