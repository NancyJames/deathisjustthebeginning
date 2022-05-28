using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HR.Utilities.Variables;

public class Enemy : MonoBehaviour
{
    private Action<Enemy> killMethod;
    [SerializeField] float minMoveSpeed=100;
    [SerializeField] float maxMoveSped = 300;
    [SerializeField] FloatVariable playerX;
    [SerializeField] FloatVariable playerHealth;
    [SerializeField] float damage;
    [SerializeField] float attackCooldown = 0.5f;
    float currentAttackCooldown;
    Transform t;
    Rigidbody2D myBody;
    float speed;

    public void Spawn(Action<Enemy> kill, Transform parent)
    {
        t = transform;
        t.SetParent(parent, false);
        t.localPosition = new Vector3(0, 0, 0);
        killMethod = kill;
        myBody = GetComponent<Rigidbody2D>();
        speed= UnityEngine.Random.Range(minMoveSpeed, maxMoveSped);
    }
    public void KillMe()
    {
        killMethod(this);
    }

    private void FixedUpdate()
    {
        currentAttackCooldown -= Time.deltaTime;
        
        float newXVel = Mathf.Sign(playerX.Get() - t.position.x) * speed * Time.deltaTime;
        if(currentAttackCooldown>0)
        {
            //moveaway;
            newXVel = -newXVel;
        }
        myBody.velocity = new Vector2(newXVel, myBody.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.gameObject.CompareTag("Player") && playerHealth.Get()-damage>=0)
        {
            playerHealth.Decrement(damage);
            currentAttackCooldown = attackCooldown;
        }
    }
}
