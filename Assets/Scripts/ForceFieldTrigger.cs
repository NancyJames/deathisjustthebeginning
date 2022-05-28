using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceFieldTrigger : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] float speed;
    float currentDuration;
    BoxCollider2D parentCollider;
    bool triggered=false;

    private void Awake()
    {
        parentCollider = transform.parent.gameObject.GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!triggered && collision.CompareTag("Player"))
        {
            triggered = true;
            StartCoroutine(Push());
        }
    }

    IEnumerator Push()
    {
        while(currentDuration < duration)
        {
            parentCollider.size += new Vector2(speed * Time.deltaTime, 0f);
            currentDuration += Time.deltaTime;
            yield return new WaitForEndOfFrame();
           
        }
        Destroy(gameObject.transform.parent.gameObject);
    }
}
