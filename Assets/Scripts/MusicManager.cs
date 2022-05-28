using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class MusicManager : MonoBehaviour
{
    AudioSource myAS;
    [SerializeField]  float musicTransitionSecs = 2f;
    
    bool sceneLoaded = false;
    private void Awake()
    {
        myAS = GetComponent<AudioSource>();
    }

    public void SceneLoaded()
    {
        sceneLoaded = true;
    }

    public void SceneUnloaded()
    {
        sceneLoaded = false;
        if(FindObjectOfType<LevelManager>().ChangeTrack() ||!myAS.isPlaying)
        {
            StartCoroutine(ChangeTrack());
        }
        

        
    }


    IEnumerator ChangeTrack()
    {
        float delta;
        float volume = myAS.volume;
        while (volume > Mathf.Epsilon)
        {
            delta = Time.deltaTime / (musicTransitionSecs / 2);
            volume -= delta;
            myAS.volume = volume;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitUntil(() => sceneLoaded = true);
        //make sure volume is 0 when switching the trakc

        AudioClip levelMusic = FindObjectOfType<LevelManager>().GetLevelMusic();
        myAS.clip = levelMusic;
        myAS.Play();
        while(volume<=1)
        {
            delta = Time.deltaTime /( musicTransitionSecs/2);
            volume += delta;
            myAS.volume = volume;
            yield return new WaitForEndOfFrame();
        }
    }
}
