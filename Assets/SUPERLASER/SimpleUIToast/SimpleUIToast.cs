using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SimpleUIToast : MonoBehaviour
{
    [SerializeField] private GameObject simpleUIToastBox_Prefab;
    private const string PREFAB_PATH = "SUPERLASER/SimpleUIToast/SimpleUIToastCanvas";

    private static SimpleUIToast instance;
    public static SimpleUIToast Instance
    {
        get
        {
            if (instance)
                return instance;
            else
            {
                GameObject prefab = Resources.Load<GameObject>(PREFAB_PATH);
                if (prefab)
                    return Instantiate(prefab).GetComponent<SimpleUIToast>();
                else
                {
                    Debug.LogError($"Cant find SimpleUIToast at {PREFAB_PATH}");
                    Debug.LogError("Read README.docx, you might have forgotten to copy something");
                    return null;
                }
            }
        }
    }

    private void Start()
    {
        if (instance)
            Destroy(gameObject);
        else
            instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public static void ShowToast(string text, float duration = 1.5f)
    {
        Instance.StartCoroutine(Instance.DelayShowToast(text, duration));
    }

    private IEnumerator DelayShowToast(string text, float duration = 1.5f)
    {
        yield return new WaitForEndOfFrame();
        SimpleUIToastBox uiTooltipBox = Instantiate(Instance.simpleUIToastBox_Prefab, Instance.transform).GetComponent<SimpleUIToastBox>();
        uiTooltipBox.ShowTooltip(text, duration);
    }
}
