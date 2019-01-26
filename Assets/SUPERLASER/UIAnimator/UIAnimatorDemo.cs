using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SUPERLASER;
using UnityEngine.UI;
using System;

public class UIAnimatorDemo : MonoBehaviour
{
    [Header("UI Set 1")]
    [SerializeField] private UIAnimator bottomPanel;
    [SerializeField] private UIAnimator leftPanel;
    [SerializeField] private UIAnimator rightPanel;
    [SerializeField] private List<UIAnimator> leftBoxAnim;
    [SerializeField] private List<UIAnimator> rightBoxAnim;
    [SerializeField] private List<UIAnimator> btmBoxesAnim;
    [SerializeField] private UIAnimator centerPanel;
    [SerializeField] private UIAnimator loadingSpinner;

    private void Start()
    {
        foreach (UIAnimator anim in btmBoxesAnim)
        {
            anim.gameObject.GetComponent<Button>().onClick.AddListener(delegate { anim.Animate_Scale(UIAnimator.Location.INITIAL, true); });
        }

        foreach (UIAnimator anim in leftBoxAnim)
        {
            anim.gameObject.GetComponent<Button>().onClick.AddListener(delegate { anim.Animate_Scale(UIAnimator.Location.INITIAL, true); });
        }

        foreach (UIAnimator anim in rightBoxAnim)
        {
            anim.gameObject.GetComponent<Button>().onClick.AddListener(delegate { anim.Animate_Scale(UIAnimator.Location.INITIAL, true); });
        }
    }

    public void Button1Click()
    {
        StartCoroutine(AnimateUISet1());
    }

    private IEnumerator AnimateUISet1()
    {
        bottomPanel?.Animate_Pos_ToOpposite();
        yield return new WaitForSeconds(0.05f);
        leftPanel?.Animate_Pos_ToOpposite();
        yield return new WaitForSeconds(0.05f);
        rightPanel?.Animate_Pos_ToOpposite();
        yield return new WaitForSeconds(0.05f);
        centerPanel?.Animate_Pos_ToOpposite();

        foreach (UIAnimator anim in btmBoxesAnim)
        {
            yield return new WaitForEndOfFrame();
            anim?.Animate_Scale_ToOpposite();
        }

        foreach (UIAnimator anim in leftBoxAnim)
        {
            yield return new WaitForEndOfFrame();
            anim?.Animate_Scale_ToOpposite();
        }

        foreach (UIAnimator anim in rightBoxAnim)
        {
            yield return new WaitForEndOfFrame();
            anim?.Animate_Scale_ToOpposite();
        }

        loadingSpinner.CurrRotState = loadingSpinner.CurrRotState ==
            UIAnimator.RotationState.RUNNING ? UIAnimator.RotationState.STOPPED : UIAnimator.RotationState.RUNNING;
    }
}