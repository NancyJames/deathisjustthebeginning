using HR.Utilities.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameEvent startGameEvent;
    [SerializeField] GameObject creditsScreen;
    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        startGameEvent?.Raise();
        FindObjectOfType<SceneLoader>().LoadLevel("Depression");
    }

    public void RestartGame()
    {
        startGameEvent?.Raise();
        //always change track when restarting;
        FindObjectOfType<LevelManager>().OverrideChangeTrack();
        FindObjectOfType<PersistantVariables>().ResetGame();
        FindObjectOfType<SceneLoader>().LoadLevel("Depression");
    }

    public void ShowPause()
    {
        Cursor.visible = true;
        pauseMenu.SetActive(true);
    }

    public void HidePause()
    {
        Cursor.visible = false;
        pauseMenu.SetActive(false);
    }

    public void ShowCredits()
    {
        if(creditsScreen!=null)
        {
            creditsScreen.SetActive(true);
        }
    }

    public void CloseCredits()
    {
        if (creditsScreen != null)
        {
            creditsScreen.SetActive(false);
        }
    }
}
