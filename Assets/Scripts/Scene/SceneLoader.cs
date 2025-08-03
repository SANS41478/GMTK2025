using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// ���س�����Ĭ�ϵ�����ģʽ���Զ�ж�ص�ǰ������
    /// </summary>
    public void LoadScene(string sceneName, System.Action onComplete = null)
    {
        StartCoroutine(LoadSceneAsync(sceneName, LoadSceneMode.Single, onComplete));
    }

    /// <summary>
    /// ���س�����Additive ģʽ��
    /// </summary>
    public void LoadSceneAdditive(string sceneName, System.Action onComplete = null)
    {
        StartCoroutine(LoadSceneAsync(sceneName, LoadSceneMode.Additive, onComplete));
    }

    /// <summary>
    /// ж�� Additive ���صĳ���
    /// </summary>
    public void UnloadScene(string sceneName, System.Action onComplete = null)
    {
        if (SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            StartCoroutine(UnloadSceneAsync(sceneName, onComplete));
        }
        else
        {
            Debug.LogWarning($"����ж�س��� {sceneName} ���ó���δ���ء�");
            onComplete?.Invoke();
        }
    }

    private IEnumerator LoadSceneAsync(string sceneName, LoadSceneMode mode, System.Action onComplete)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, mode);
        while (!operation.isDone)
        {
            yield return null;
        }
        onComplete?.Invoke();
    }

    private IEnumerator UnloadSceneAsync(string sceneName, System.Action onComplete)
    {
        AsyncOperation operation = SceneManager.UnloadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            yield return null;
        }
        onComplete?.Invoke();
    }
}
