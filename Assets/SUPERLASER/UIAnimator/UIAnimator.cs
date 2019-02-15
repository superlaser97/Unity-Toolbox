using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Author: Low Zhi Heng
/// Date of Creation: 1 Nov 2018
/// Last Updated: 19 Jan 2019
/// </summary>

namespace SUPERLASER
{
    public class UIAnimator : MonoBehaviour
    {
        [Header("General Settings")]
        [SerializeField] private RectTransform targetRect;

        public enum Location { INITIAL, END }
        public enum AnimateMode { POSITION, ROTATION, SCALE }
        [SerializeField] private AnimateMode animateMode = AnimateMode.POSITION;
        public AnimateMode AnimMode { get { return animateMode; } }

        [Header("Position Anim Settings")]
        [SerializeField] private Vector2 endPosition;
        [SerializeField] private float posAnimSpd = 20f;
        public float PosAnimSpd
        {
            get { return posAnimSpd; }
            set { posAnimSpd = value; }
        }
        private Location posDir = Location.INITIAL;
        private Vector2 initialPosition;

        [Header("Rotation Anim Settings")]
        [SerializeField] private Vector3 rotSpd;
        public enum RotationState { STOPPED, RUNNING }
        public RotationState CurrRotState { get; set; }

        [Header("Scale Anim Settings")]
        [SerializeField] private float endScale = 0;
        [SerializeField] private float scaleAnimSpd = 20f;
        public float ScaleAnimSpd
        {
            get { return scaleAnimSpd; }
            set { scaleAnimSpd = value; }
        }
        private Location scaleDir = Location.INITIAL;
        private float initialScale;

        private Coroutine animationCoroutine;

        private void OnEnable()
        {
            if (!targetRect)
                targetRect = GetComponent<RectTransform>();

            initialPosition = targetRect.anchoredPosition;
            initialScale = targetRect.localScale.x;
        }

        private void Update()
        {
            if (animateMode == AnimateMode.ROTATION && CurrRotState == RotationState.RUNNING)
            {
                transform.rotation = transform.rotation * Quaternion.Euler(rotSpd.x * Time.deltaTime, rotSpd.y * Time.deltaTime, rotSpd.z * Time.deltaTime);
            }
        }

        public void Animate_Pos_ToOpposite(bool destroyGOAfterAnimate = false)
        {
            if (animateMode != AnimateMode.POSITION)
            {
                Debug.LogWarning("Can't animate, wrong animate mode");
                return;
            }
            if (animationCoroutine != null)
                StopCoroutine(animationCoroutine);

            animationCoroutine = StartCoroutine(
                StartAnimatingPos(posDir == Location.END ? Location.INITIAL : Location.END, 
                destroyGOAfterAnimate));
        }

        public void Animate_Pos(Location location, bool destroyGOAfterAnimate = false)
        {
            if (animateMode != AnimateMode.POSITION)
            {
                Debug.LogWarning("Can't animate, wrong animate mode");
                return;
            }
            if (animationCoroutine != null)
                StopCoroutine(animationCoroutine);

            animationCoroutine = StartCoroutine(StartAnimatingPos(location, destroyGOAfterAnimate));
        }

        public void Animate_Scale_ToOpposite(bool destroyGOAfterAnimate = false)
        {
            if (animateMode != AnimateMode.SCALE)
            {
                Debug.LogWarning("Can't animate, wrong animate mode");
                return;
            }
            if (animationCoroutine != null)
                StopCoroutine(animationCoroutine);
            
            animationCoroutine = StartCoroutine(
                StartAnimatingScale(scaleDir == Location.END ? Location.INITIAL : Location.END,
                destroyGOAfterAnimate));
        }

        public void Animate_Scale(Location location, bool destroyGOAfterAnimate = false)
        {
            if (animateMode != AnimateMode.SCALE)
            {
                Debug.LogWarning("Can't animate, wrong animate mode");
                return;
            }
            if (animationCoroutine != null)
                StopCoroutine(animationCoroutine);

            animationCoroutine = StartCoroutine(StartAnimatingScale(location, destroyGOAfterAnimate));
        }

        private IEnumerator StartAnimatingPos(Location towards, bool destroyGOAfterAnimate = false)
        {
            Vector2 targetPos = new Vector2();

            if (towards == Location.INITIAL)
                targetPos = initialPosition;
            else
                targetPos = endPosition;

            while (Mathf.Abs((targetRect.anchoredPosition - targetPos).magnitude) > 1)
            {
                float newXPos = Mathf.Lerp(targetRect.anchoredPosition.x, targetPos.x, Time.deltaTime * PosAnimSpd);
                float newYPos = Mathf.Lerp(targetRect.anchoredPosition.y, targetPos.y, Time.deltaTime * PosAnimSpd);

                targetRect.anchoredPosition = new Vector2(newXPos, newYPos);
                yield return null;
            }
            targetRect.anchoredPosition = targetPos;
            posDir = towards;

            if (destroyGOAfterAnimate)
                Destroy(gameObject);

            yield return null;
        }

        private IEnumerator StartAnimatingScale(Location towards, bool destroyGOAfterAnimate = false)
        {
            if (towards == Location.END)
            {
                while (Mathf.Abs(transform.localScale.x - endScale) > 0.1f)
                {
                    float newScale = Mathf.Lerp(transform.localScale.x, endScale, Time.deltaTime * ScaleAnimSpd);
                    transform.localScale = new Vector2(newScale, newScale);
                    yield return null;
                }
                transform.localScale = new Vector2(endScale, endScale);
            }
            else
            {
                while (Mathf.Abs(transform.localScale.x - initialScale) > 0.1f)
                {
                    float newScale = Mathf.Lerp(transform.localScale.x, initialScale, Time.deltaTime * ScaleAnimSpd);
                    transform.localScale = new Vector2(newScale, newScale);
                    yield return null;
                }
                transform.localScale = new Vector2(initialScale, initialScale);
            }
            scaleDir = towards;

            if (destroyGOAfterAnimate)
                Destroy(gameObject);

            yield return null;
        }
    }

#if UNITY_EDITOR
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UIAnimator))]
    public class UIAnimatorEditor : Editor
    {
        private UIAnimator editorTarget;

        private void OnEnable()
        {
            editorTarget = (UIAnimator)target;
        }

        public override void OnInspectorGUI()
        {
            GUILayout.Space(10);
            if (Application.isPlaying)
            {
                if (editorTarget.AnimMode == UIAnimator.AnimateMode.POSITION)
                {
                    EditorGUILayout.LabelField("Position Anim Control", EditorStyles.boldLabel);
                    if(GUILayout.Button("Animate to Initial"))
                    {
                        editorTarget.Animate_Pos(UIAnimator.Location.INITIAL);
                    }
                    if (GUILayout.Button("Animate to End"))
                    {
                        editorTarget.Animate_Pos(UIAnimator.Location.END);
                    }
                    if (GUILayout.Button("Animate to Opposite"))
                    {
                        editorTarget.Animate_Pos_ToOpposite();
                    }
                }

                if (editorTarget.AnimMode == UIAnimator.AnimateMode.SCALE)
                {
                    EditorGUILayout.LabelField("Scale Anim Control", EditorStyles.boldLabel);
                    if (GUILayout.Button("Animate to Initial"))
                    {
                        editorTarget.Animate_Scale(UIAnimator.Location.INITIAL);
                    }
                    if (GUILayout.Button("Animate to End"))
                    {
                        editorTarget.Animate_Scale(UIAnimator.Location.END);
                    }
                    if (GUILayout.Button("Animate to Opposite"))
                    {
                        editorTarget.Animate_Scale_ToOpposite();
                    }
                }

                if (editorTarget.AnimMode == UIAnimator.AnimateMode.ROTATION)
                {
                    EditorGUILayout.LabelField("Rotation Anim Control", EditorStyles.boldLabel);
                    if (GUILayout.Button("Animate to Stop"))
                    {
                        editorTarget.CurrRotState = UIAnimator.RotationState.STOPPED;
                    }
                    if (GUILayout.Button("Animate to Running"))
                    {
                        editorTarget.CurrRotState = UIAnimator.RotationState.RUNNING;
                    }
                    if (GUILayout.Button("Animate to Opposite"))
                    {
                        editorTarget.CurrRotState = editorTarget.CurrRotState == 
                            UIAnimator.RotationState.RUNNING ? UIAnimator.RotationState.STOPPED : UIAnimator.RotationState.RUNNING;
                    }
                }
            }

            GUILayout.Space(10);
            base.OnInspectorGUI();
        }
    }
    #endif
}

