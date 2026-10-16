using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance { get; private set; }

    private UIDocument document;
    private ProgressBar loadingProgressBar;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        document = GetComponentInChildren<UIDocument>(true);
    }

    public void LoadByScene(SceneList scene)
    {
        InitLoadingScreen();

        int sceneIndex = (int)scene;
        StartCoroutine(LoadByIndexAsync(sceneIndex));
    }

    private IEnumerator LoadByIndexAsync(int sceneIndex)
    {
        float minLoadTime = 1f;
        float timer = 0f;

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            timer += Time.deltaTime;
            float realProgress = Mathf.Clamp01(op.progress / .9f);
            float timeProgress = Mathf.Clamp01(timer / minLoadTime);
            float targetProgress = Mathf.Min(realProgress, timeProgress);

            loadingProgressBar.value = targetProgress;

            if (op.progress >= .9f && timer >= minLoadTime)
                op.allowSceneActivation = true;

            yield return null;
        }

        document.enabled = false;
    }

    private void InitLoadingScreen()
    {
        document.enabled = true;
        loadingProgressBar = document.rootVisualElement.Q<ProgressBar>("LoadingProgressBar");
        loadingProgressBar.value = 0;
    }
}
