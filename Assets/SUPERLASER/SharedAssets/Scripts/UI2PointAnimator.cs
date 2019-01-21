using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Low Zhi Heng
/// Date of Creation: 1 Nov 2018
/// Last Updated: 19 Jan 2019
/// </summary>

namespace SUPERLASER
{
    public class UI2PointAnimator : MonoBehaviour
    {
        [SerializeField] private RectTransform targetRect;
        private bool atBeginning = true;

        private Vector2 initialPos;

        [SerializeField] private bool usePositionAnim = true;
        [SerializeField] private Vector2 destPos;
        [Space(10)]
        [SerializeField] private bool useScaleAnim = false;
        public float AnimateSpd { get; set; } = 20f;

        private Coroutine animationCoroutine;

        private void OnEnable()
        {
            if (!targetRect)
                targetRect = GetComponent<RectTransform>();
            initialPos = targetRect.anchoredPosition;
        }

        [ContextMenu("Animate to Beginning")]
        public void AnimateToBeginning(bool destroyGOAfterAnimate = false)
        {
            AnimateTo(true, destroyGOAfterAnimate);
        }

        [ContextMenu("Animate to End")]
        public void AnimateToEnd(bool destroyGOAfterAnimate = false)
        {
            AnimateTo(false, destroyGOAfterAnimate);
        }

        [ContextMenu("Animate")]
        public void Animate(bool destroyGOAfterAnimate = false)
        {
            AnimateTo(!atBeginning, destroyGOAfterAnimate);
        }

        private void AnimateTo(bool toBeginning, bool destroyGOAfterAnimate = false)
        {
            if (usePositionAnim ^ useScaleAnim)
            {
                if (animationCoroutine != null)
                    StopCoroutine(animationCoroutine);

                if (usePositionAnim)
                    animationCoroutine = StartCoroutine(StartAnimatingPos(toBeginning, destroyGOAfterAnimate));
                else if (useScaleAnim)
                    animationCoroutine = StartCoroutine(StartAnimatingScale(toBeginning, destroyGOAfterAnimate));
            }
            else
            {
                Debug.Log("CANT ANIMATE BOTH SCALE AND POS AT THE SAME TIME");
            }
        }

        private IEnumerator StartAnimatingPos(bool toBeginning, bool destroyGOAfterAnimate = false)
        {
            Vector2 targetPos = destPos;
            if (toBeginning)
                targetPos = initialPos;

            while (Mathf.Abs((targetRect.anchoredPosition - targetPos).magnitude) > 1)
            {
                float newXPos = Mathf.Lerp(targetRect.anchoredPosition.x, targetPos.x, Time.deltaTime * AnimateSpd);
                float newYPos = Mathf.Lerp(targetRect.anchoredPosition.y, targetPos.y, Time.deltaTime * AnimateSpd);

                targetRect.anchoredPosition = new Vector2(newXPos, newYPos);
                yield return null;
            }
            targetRect.anchoredPosition = targetPos;
            atBeginning = !atBeginning;

            if (destroyGOAfterAnimate)
                Destroy(gameObject);

            yield return null;
        }

        private IEnumerator StartAnimatingScale(bool toBeginning, bool destroyGOAfterAnimate = false)
        {
            if (!toBeginning)
            {
                transform.localScale = new Vector2(1, 1);
                while (transform.localScale.x > 0.05f)
                {
                    float newScale = Mathf.Lerp(transform.localScale.x, 0, Time.deltaTime * AnimateSpd);
                    transform.localScale = new Vector2(newScale, newScale);
                    yield return null;
                }
                transform.localScale = new Vector2(0, 0);
            }
            else
            {
                transform.localScale = new Vector2(0, 0);
                while (transform.localScale.x < 1 - 0.05f)
                {
                    float newScale = Mathf.Lerp(transform.localScale.x, 1, Time.deltaTime * AnimateSpd);
                    transform.localScale = new Vector2(newScale, newScale);
                    yield return null;
                }
                transform.localScale = new Vector2(1, 1);
            }
            atBeginning = !atBeginning;

            if (destroyGOAfterAnimate)
                Destroy(gameObject);

            yield return null;
        }
    }
}