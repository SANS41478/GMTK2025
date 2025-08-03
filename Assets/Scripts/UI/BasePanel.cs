using UnityEngine;
using UnityEngine.Events;
public abstract class BasePanel : MonoBehaviour
{
    //���뵭�����ٶ�
    private readonly float alphaSpeed = 10;
    //������Ƶ��뵭���Ļ����� ���
    private CanvasGroup canvasGroup;

    //���Լ������ɹ�ʱ Ҫִ�е�ί�к���
    private UnityAction hideCallBack;

    //�Ƿ�ʼ��ʾ
    private bool isShow;

    protected virtual void Awake()
    {
        //һ��ʼ��ȡ����� ���ص� ��� ���û�� ����ͨ������ Ϊ�����һ��
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Init();
    }

    // Update is called once per frame
    private void Update()
    {
        //����
        if (isShow && canvasGroup.alpha != 1)
        {
            canvasGroup.alpha += alphaSpeed * Time.deltaTime;
            if (canvasGroup.alpha >= 1)
                canvasGroup.alpha = 1;
        }
        //����
        else if (!isShow)
        {
            canvasGroup.alpha -= alphaSpeed * Time.deltaTime;
            if (canvasGroup.alpha <= 0)
            {
                canvasGroup.alpha = 0;
                //Ӧ���ù����� ɾ���Լ�
                hideCallBack?.Invoke();
            }
        }
    }

    /// <summary>
    ///     ��Ҫ���� ��ʼ�� ��ť�¼������ȵ�����
    /// </summary>
    public abstract void Init();

    /// <summary>
    ///     ��ʾ�Լ�ʱ  ��������
    /// </summary>
    public virtual void ShowMe()
    {
        isShow = true;
        canvasGroup.alpha = 0;
    }

    /// <summary>
    ///     �����Լ�ʱ ��������
    /// </summary>
    public virtual void HideMe(UnityAction callBack)
    {
        isShow = false;
        canvasGroup.alpha = 1;
        //��¼ ����� �������ɹ����ִ�еĺ���
        hideCallBack = callBack;
    }
}