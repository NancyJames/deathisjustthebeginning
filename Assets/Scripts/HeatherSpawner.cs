using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatherSpawner : MonoBehaviour
{
    [SerializeField] GameObject heather;
    [SerializeField] Transform[] waypoints;
    [SerializeField] float fadeTime=1f;
    int index = 0;
    SpriteRenderer heatherRenderer;

    private void Awake()
    {
        heatherRenderer = heather.GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        heatherRenderer.color = new Color(1, 1, 1, 1);
        if(waypoints.Length>0)
        {
            heather.transform.position = waypoints[0].position;
            index++;
        } 
    }

    public void MemoryTriggered()
    {
        if(index<waypoints.Length)
        {
            StartCoroutine(MoveHeather());
        }
    }

    IEnumerator MoveHeather()
    {
        float time=0;
        float delta;
        while(time<fadeTime)
        {
            delta= fadeTime * Time.deltaTime;
            time += Time.deltaTime;
            heatherRenderer.color = new Color(1, 1, 1, heatherRenderer.color.a - delta);
            yield return new WaitForEndOfFrame();
        }
        heather.transform.position = waypoints[index].position;
        index++;
        while (time >= 0)
        {
            delta = fadeTime * Time.deltaTime;
            time -= Time.deltaTime;
            heatherRenderer.color = new Color(1, 1, 1, heatherRenderer.color.a + delta);
            yield return new WaitForEndOfFrame();
        }
    }

}
