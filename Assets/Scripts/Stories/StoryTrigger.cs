using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HR.Utilities.Events;

public class StoryTrigger : MonoBehaviour
{
    [SerializeField] StoryPoint_SO story;
    [SerializeField] GameEvent storyTriggered;
    [SerializeField] float memoryCooldown = 6f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !story.HasBeenSeen())
        {
            GameManager.instance.SetCurrentStory(story);
            storyTriggered?.Raise();
            if(story.IsMemory())
            {
                Invoke("ResetBeenSeen", memoryCooldown);
            }
        }
    }

    private void ResetBeenSeen()
    {
        story.ResetBeenSeen();
    }

    
}
