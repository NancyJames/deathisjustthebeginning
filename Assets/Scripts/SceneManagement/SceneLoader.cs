using HR.Utilities.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    Fader fader;
    [SerializeField] float fadeOutTime;
    [SerializeField] float fadeInTime;
    [SerializeField] GameEvent sceneLoaded;
    [SerializeField] GameEvent sceneUnloaded;

    private void Awake()
    {
        fader = FindObjectOfType<Fader>();
    }
    private void Start()
    {
        sceneUnloaded?.Raise();
        sceneLoaded?.Raise();
    }

    public void LoadLevel(string levelToLoad)
    {
        StartCoroutine(LoadLevelRoutine(levelToLoad));
    }

    public IEnumerator LoadLevelRoutine(string levelToLoad)
    {
        sceneUnloaded?.Raise();
        yield return fader.FadeOut(fadeOutTime);
        yield return SceneManager.LoadSceneAsync(levelToLoad);
        sceneLoaded?.Raise();
        yield return fader.FadeIn(fadeInTime);

        
    }
}
