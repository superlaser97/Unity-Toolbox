using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SUPERLASER;

public class SimpleUIToastBox : MonoBehaviour
{
    [SerializeField] private UIAnimator animator;
    [SerializeField] private TextMeshProUGUI tooltipText;

    public void ShowTooltip(string text, float duration = 1.5f)
    {
        StartCoroutine(InternalShowTooltip(text, duration));
    }

    private IEnumerator InternalShowTooltip(string text, float duration)
    {
        animator.Animate_Scale(UIAnimator.Location.END);
        tooltipText.text = text;
        yield return new WaitForSeconds(duration);
        animator.Animate_Scale(UIAnimator.Location.INITIAL);
        yield return null;
    }
}
