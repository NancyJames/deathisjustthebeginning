using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTrigger : MonoBehaviour
{

    Transform t;
    bool triggered = false;
    [SerializeField] float finishedSize;
    [SerializeField] float speed;
    SpriteRenderer parentRenderer;
    BoxCollider2D parentCollider;

    private void Awake()
    {
        t = transform.parent;
        parentRenderer = t.GetComponent<SpriteRenderer>();
        parentCollider = t.GetComponent<BoxCollider2D>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!triggered && collision.CompareTag("Player"))
        {
            triggered = true;
            StartCoroutine(PutUpWall());
        }
    }

    IEnumerator PutUpWall()
    {
       while(parentRenderer.size.y<finishedSize)
        {
            float newY = Time.deltaTime * speed;
            Vector2 newSize = new Vector2(0, newY);
            parentRenderer.size += newSize;
            parentCollider.size += newSize;
            parentCollider.offset += new Vector2(0, newY / 2);
            yield return new WaitForEndOfFrame();
        }
    }
}
