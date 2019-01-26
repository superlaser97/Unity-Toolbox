using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SUPERLASER
{
    public class AsyncSceneLoader : MonoBehaviour
    {
        [SerializeField] private float minLoadingDuration = 0f;
        private Timer minLoadingTimer = new Timer();

        AsyncOperation asyncOperation;

        private static string targetScene;

        private void Start()
        {
            StartCoroutine(LoadNextScene());
        }

        public static void AsyncLoadScene(string targetScene)
        {
            AsyncSceneLoader.targetScene = targetScene;
            SceneManager.LoadScene("AsyncLoadingScene");
        }

        private IEnumerator LoadNextScene()
        {
            Scene newScene = SceneManager.GetSceneByName(targetScene);
            asyncOperation = SceneManager.LoadSceneAsync(targetScene);
            asyncOperation.allowSceneActivation = false;
            minLoadingTimer.SetTimer(minLoadingDuration);

            while ((1 - (minLoadingTimer.TimeLeft / minLoadingDuration) < 1))
            {
                float loadingProgress = 1 - (minLoadingTimer.TimeLeft / minLoadingDuration);

                yield return null;
            }
            asyncOperation.allowSceneActivation = true;
        }
    }
}
