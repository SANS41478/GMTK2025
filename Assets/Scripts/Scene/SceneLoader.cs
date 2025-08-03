using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using Space.EventFramework;
using Space.GlobalInterface.EventInterface;

[RequireComponent(typeof(MonoEventSubComponent))]
public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }
    public struct  LoadNewLevel : IEventData
    {
        
    }
    private MonoEventSubComponent _monoEventSubComponent;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        _monoEventSubComponent = GetComponent<MonoEventSubComponent>();
    }

    /// <summary>
    /// 加载场景（默认单场景模式，自动卸载当前场景）
    /// </summary>
    public void LoadScene(string sceneName, System.Action onComplete = null)
    {
        _monoEventSubComponent.Publish(new LoadNewLevel());
        StartCoroutine(LoadSceneAsync(sceneName, LoadSceneMode.Single, onComplete));
    }

    /// <summary>
    /// 加载场景（Additive 模式）
    /// </summary>
    public void LoadSceneAdditive(string sceneName, System.Action onComplete = null)
    {
        _monoEventSubComponent.Publish(new LoadNewLevel());
        StartCoroutine(LoadSceneAsync(sceneName, LoadSceneMode.Additive, onComplete));
    }

    /// <summary>
    /// 卸载 Additive 加载的场景
    /// </summary>
    public void UnloadScene(string sceneName, System.Action onComplete = null)
    {
        _monoEventSubComponent.Publish(new LoadNewLevel());
        if (SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            StartCoroutine(UnloadSceneAsync(sceneName, onComplete));
        }
        else
        {
            Debug.LogWarning($"尝试卸载场景 {sceneName} 但该场景未加载。");
            onComplete?.Invoke();
        }
    }

    private IEnumerator LoadSceneAsync(string sceneName, LoadSceneMode mode, System.Action onComplete)
    {
        _monoEventSubComponent.Publish(new LoadNewLevel());
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, mode);
        while (!operation.isDone)
        {
            yield return null;
        }
        onComplete?.Invoke();
    }

    private IEnumerator UnloadSceneAsync(string sceneName, System.Action onComplete)
    {
        _monoEventSubComponent.Publish(new LoadNewLevel());
        AsyncOperation operation = SceneManager.UnloadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            yield return null;
        }
        onComplete?.Invoke();
    }
}
