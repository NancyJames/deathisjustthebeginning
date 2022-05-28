using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class TutorialTipDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI storyDisplayField;
    [SerializeField] float tipDisplayTime;
    CanvasGroup cg;
    TutorialTip_SO tip;
    Coroutine showingTip = null;

    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();
        cg.alpha = 0;
    }
    public void ShowTip()
    {
        tip = GameManager.instance.GetCurrentTip();
        if (showingTip != null && !tip.HasBeenSeen())
        {
            StopCoroutine(showingTip);
        }
        if (tip != null && !tip.HasBeenSeen())
        {
            showingTip = StartCoroutine(ShowStoryRoutine());
        }

    }
    IEnumerator ShowStoryRoutine()
    {
        storyDisplayField.text = "";
        cg.alpha = 1;
        storyDisplayField.text = tip.GetTip();
        yield return new WaitForSeconds(tipDisplayTime);
        cg.alpha = 0;
    }
}
