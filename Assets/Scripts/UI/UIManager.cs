using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    private static UIManager instance = new UIManager();
    public static UIManager Instance => instance;

    //�洢��������
    private Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();
    //Ӧ��һ��ʼ �͵õ����ǵ� Canvas���� 
    private Transform canvasTrans;

    private UIManager()
    {
        //�õ������ϴ����õ� Canvas����
        canvasTrans = GameObject.Find("Canvas").transform;
        //�� Canvas���� ������ ���Ƴ� 
        //���Ƕ���ͨ�� ��̬���� �� ��̬ɾ�� ����ʾ �������� ���Բ�ɾ���� Ӱ�첻��
        GameObject.DontDestroyOnLoad(canvasTrans.gameObject);
    }

    //��ʾ���
    public T ShowPanel<T>() where T : BasePanel
    {
        //����ֻ��Ҫ��֤ ����T������ �� ����� һ��  ��һ�������Ĺ��� �ͷǳ��������ǵ�ʹ��
        string panelName = typeof(T).Name;

        //�Ƿ��Ѿ�����ʾ�ŵĸ������ ����� ���ô��� ֱ�ӷ��ظ��ⲿʹ��
        if (panelDic.ContainsKey(panelName))
            return panelDic[panelName] as T;

        //��ʾ��� ���� ��̬�Ĵ������Ԥ���� ���ø�����
        //���ݵõ��� ���� �������ǵ�Ԥ��������� ֱ�� ��̬������ ����
        GameObject panelObj = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Panel/" + panelName));
        panelObj.transform.SetParent(canvasTrans, false);

        //���� ���ǵõ���Ӧ�����ű� �洢����
        T panel = panelObj.GetComponent<T>();
        //�����ű��洢�� ��Ӧ������ ֮�� ���Է������ǻ�ȡ��
        panelDic.Add(panelName, panel);
        //������ʾ�Լ����߼�
        panel.ShowMe();

        return panel;
    }

    //�������
    //����һ�����ϣ�� ���� ��Ĭ�ϴ�true ���ϣ��ֱ�����أ�ɾ������� ��ô�ʹ�false
    public void HidePanel<T>(bool isFade = true) where T : BasePanel
    {
        //���� �������� �õ���� ����
        string panelName = typeof(T).Name;
        //�жϵ�ǰ��ʾ����� ��û�и����ֵ����
        if (panelDic.ContainsKey(panelName))
        {
            if (isFade)
            {
                panelDic[panelName].HideMe(() =>
                {
                    //��� �����ɹ��� ϣ��ɾ�����
                    GameObject.Destroy(panelDic[panelName].gameObject);
                    //ɾ������ �� �ֵ����Ƴ�
                    panelDic.Remove(panelName);
                });
            }
            else
            {
                //ɾ�����
                GameObject.Destroy(panelDic[panelName].gameObject);
                //ɾ������ �� �ֵ����Ƴ�
                panelDic.Remove(panelName);
            }
        }
    }

    //������
    public T GetPanel<T>() where T : BasePanel
    {
        string panelName = typeof(T).Name;
        if (panelDic.ContainsKey(panelName))
            return panelDic[panelName] as T;

        //���û�� ֱ�ӷ��ؿ�
        return null;
    }
}
