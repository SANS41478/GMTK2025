using UnityEngine;
/// <summary>
///     ������Ч�������������ػ��յĽű�
///     �� 1~4 �����Ŷ�Ӧ��Ч����֧���ֶ�ֹͣ�����ź��Զ����ա�
///     �� P ���鿴�����״̬��
/// </summary>
public class AudioManagerTest : MonoBehaviour
{

    private void Update()
    {

        // ���ո����������
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Play BGM");
            AudioManager.Instance.PlayMusic("bgm");
        }
        // ���س���ֹͣ����
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("Stop BGM");
            AudioManager.Instance.StopMusic();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("Play 1");
            AudioManager.Instance.PlaySFX("1", 0.2f);
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