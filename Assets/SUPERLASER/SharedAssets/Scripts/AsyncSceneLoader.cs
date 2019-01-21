using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsyncSceneLoader : MonoBehaviour
{
    [SerializeField] private GameObject loadingText;
    [SerializeField] private Image loadingIndicator;
    [SerializeField] private Image loadingSceneBG;

    private static string SceneToLoad { get; set; }

    private void Start()
    {
        StartCoroutine(LoadNextScene());
        DontDestroyOnLoad(gameObject);
    }

    private IEnumerator LoadNextScene()
    {
        loadingIndicator.fillAmount = 0;
        
        Scene newScene = SceneManager.GetSceneByName(SceneToLoad);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(SceneToLoad);

        while (!asyncOperation.isDone)
        {
            loadingIndicator.fillAmount = asyncOperation.progress * 1.111f;
            yield return null;
        }
        StartCoroutine(FadeOutBG());
    }

    private IEnumerator FadeOutBG()
    {
        DestroyImmediate(loadingIndicator.gameObject);
        DestroyImmediate(loadingText);
        yield return new WaitForSeconds(0.1f);

        while (loadingSceneBG.color.a >= 0)
        {
            loadingSceneBG.color = new Color(0, 0, 0, loadingSceneBG.color.a - 2 * Time.deltaTime);
            yield return null;
        }

        Destroy(gameObject);
        yield return null;
    }
}
