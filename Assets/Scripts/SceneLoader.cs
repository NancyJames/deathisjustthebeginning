using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    Fader fader;
    [SerializeField] float fadeOutTime;
    [SerializeField] float fadeInTime;

    private void Awake()
    {
        fader = FindObjectOfType<Fader>();
    }

    public void LoadLevel(string levelToLoad)
    {
        StartCoroutine(LoadLevelRoutine(levelToLoad));
    }

    public IEnumerator LoadLevelRoutine(string levelToLoad)
    {
        yield return fader.FadeOut(fadeOutTime);
        yield return SceneManager.LoadSceneAsync(levelToLoad);
        yield return fader.FadeIn(fadeInTime);
    }
}
