using UnityEngine;


/// <summary>
/// 测试音效并发播放与对象池回收的脚本
/// 按 1~4 键播放对应音效，不支持手动停止，播放后自动回收。
/// 按 P 键查看对象池状态。
/// </summary>
public class AudioManagerTest : MonoBehaviour
{

    void Update()
    {

        // 按空格键播放音乐
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Play BGM");
            AudioManager.Instance.PlayMusic("bgm");
        }
        // 按回车键停止音乐
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("Stop BGM");
            AudioManager.Instance.StopMusic();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            Debug.Log("Play 1");
            AudioManager.Instance.PlaySFX("1",0.2f);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("Play 2");
            AudioManager.Instance.PlaySFX("2");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("Play 3");
            AudioManager.Instance.PlaySFX("3");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("Play 4");
            AudioManager.Instance.PlaySFX("4");
        }


    }
}


