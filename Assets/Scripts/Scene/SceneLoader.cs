using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    // 单例
    public static SceneLoader Instance { get; private set; }

    private void Awake()
    {
        // 保证只有一个实例
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 切换场景，对外暴露的公共方法
    /// </summary>
    /// <param name="sceneName">目标场景名称</param>
    /// <param name="onComplete">切换完成后的回调</param>
    public void LoadScene(string sceneName, System.Action onComplete = null)
    {
        StartCoroutine(LoadSceneAsync(sceneName, onComplete));
    }

    /// <summary>
    /// 异步加载场景协程
    /// </summary>
    private IEnumerator LoadSceneAsync(string sceneName, System.Action onComplete)
    {
        // 可加入加载动画或遮罩
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            // 可加入进度条反馈：operation.progress
            yield return null;
        }

        // 加载完成回调
        onComplete?.Invoke();
    }
}
