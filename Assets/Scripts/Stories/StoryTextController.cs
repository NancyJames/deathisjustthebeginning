using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using HR.Utilities.Events;
using HR.Utilities.Variables;

public class StoryTextController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI storyDisplayField;
    [SerializeField] float storyDisplayTime;
    [SerializeField] GameEvent finishedShowingStory;
    [SerializeField] BoolVariable storyIsPlaying;
    CanvasGroup cg;
    StoryPoint_SO story;
    Coroutine showingStory = null;

    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();
        cg.alpha = 0;
    }
    public void ShowStory()
    {
        
        story = GameManager.instance.GetCurrentStory();
        if(showingStory!=null && !story.HasBeenSeen())
        {
            StopCoroutine(showingStory);
        }
        if(story!=null && !story.HasBeenSeen())
        {
           showingStory= StartCoroutine(ShowStoryRoutine());
        }
        

    }
    IEnumerator ShowStoryRoutine()
    {
        storyIsPlaying.Set(true);
        storyDisplayField.text = "";
        cg.alpha = 1;
        //storyDisplayField.text = story.GetStory();
        string storyText = story.GetStory();
        StringBuilder sb = new StringBuilder();
        foreach(char c in storyText)
        {
            storyDisplayField.text = sb.Append(c.ToString()).ToString();
            yield return new WaitForSeconds(0.025f);
        }
        yield return new WaitForSeconds(storyDisplayTime);
        cg.alpha = 0;
        storyIsPlaying.Set(false);
        finishedShowingStory?.Raise();
    }
}
