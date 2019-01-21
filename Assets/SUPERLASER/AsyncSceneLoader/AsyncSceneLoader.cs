using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsyncSceneLoader : MonoBehaviour
{
    [SerializeField] private Image loadingIndicator;
    private static string targetScene;

    private void Start()
    {
        StartCoroutine(LoadNextScene());
        DontDestroyOnLoad(gameObject);
    }

    public static void AsyncLoadScene(string targetScene)
    {
        SceneManager.LoadScene("AsyncLoadingScene");
        AsyncSceneLoader.targetScene = targetScene;
    }

    private IEnumerator LoadNextScene()
    {
        loadingIndicator.fillAmount = 0;
        
        Scene newScene = SceneManager.GetSceneByName(targetScene);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(targetScene);

        while (!asyncOperation.isDone)
        {
            loadingIndicator.fillAmount = asyncOperation.progress * 1.111f;
            yield return null;
        }
    }
}
