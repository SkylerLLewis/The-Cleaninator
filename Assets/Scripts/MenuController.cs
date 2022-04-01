using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    CanvasGroup canvasGroup;

    void Awake() {
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
    }
    
    public void Play() {
        Debug.Log("LOADING");
        StartCoroutine(LoadScene("Game"));
    }

    IEnumerator LoadScene(string scene, float duration=1) {
        //yield return StartCoroutine(FadeLoadingScreen(1,duration));

        AsyncOperation async = SceneManager.LoadSceneAsync(scene);
        while (!async.isDone) {
            yield return null;
        }

        //yield return StartCoroutine(FadeLoadingScreen(0,duration));
        //gameObject.SetActive(false);
    }

    IEnumerator FadeLoadingScreen(float targetValue, float duration) {
        float startValue = canvasGroup.alpha;
        float time = 0;
        while (time < duration) {
            canvasGroup.alpha = Mathf.Lerp(startValue, targetValue, time/duration);
            time += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = targetValue;
    }
}
