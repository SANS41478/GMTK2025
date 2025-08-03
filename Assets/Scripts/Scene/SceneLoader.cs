using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    // ����
    public static SceneLoader Instance { get; private set; }

    private void Awake()
    {
        // ��ֻ֤��һ��ʵ��
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// �л����������Ⱪ¶�Ĺ�������
    /// </summary>
    /// <param name="sceneName">Ŀ�곡������</param>
    /// <param name="onComplete">�л���ɺ�Ļص�</param>
    public void LoadScene(string sceneName, System.Action onComplete = null)
    {
        StartCoroutine(LoadSceneAsync(sceneName, onComplete));
    }

    /// <summary>
    /// �첽���س���Э��
    /// </summary>
    private IEnumerator LoadSceneAsync(string sceneName, System.Action onComplete)
    {
        // �ɼ�����ض���������
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            // �ɼ��������������operation.progress
            yield return null;
        }

        // ������ɻص�
        onComplete?.Invoke();
    }
}
