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
    /// ���س�����Ĭ�ϵ�����ģʽ���Զ�ж�ص�ǰ������
    /// </summary>
    public void LoadScene(string sceneName, System.Action onComplete = null)
    {
        _monoEventSubComponent.Publish(new LoadNewLevel());
        StartCoroutine(LoadSceneAsync(sceneName, LoadSceneMode.Single, onComplete));
    }

    /// <summary>
    /// ���س�����Additive ģʽ��
    /// </summary>
    public void LoadSceneAdditive(string sceneName, System.Action onComplete = null)
    {
        _monoEventSubComponent.Publish(new LoadNewLevel());
        StartCoroutine(LoadSceneAsync(sceneName, LoadSceneMode.Additive, onComplete));
    }

    /// <summary>
    /// ж�� Additive ���صĳ���
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
            Debug.LogWarning($"����ж�س��� {sceneName} ���ó���δ���ء�");
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
