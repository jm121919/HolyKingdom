using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextObjectPool : MonoBehaviour
{
    //TO DO LIST
    //1 ���͸� �������� ����
    //2 Ǯ ������Ʈ�� �θ�� false�� ����
    //3 �ʿ��Ҷ� ��Ƽ�긦 ���ְ� Ư����ġ�� ����

    public List<GameObject> textPrefabList = new List<GameObject>();
    public GameObject goldTextPrefab;
    public GameObject damageTextPrefab;
    Dictionary<TextType, Queue<GameObject>> poolDic = new Dictionary<TextType, Queue<GameObject>>();

    public void InitPool()//���͵�Ÿ���� ���������� �����ؼ� �����ũ�� ������ ���� �ν����Ϳ� �̸� ������ ���־�� �Ѵ�
                          //���Ӹ޴������� �ΰ��� ����������
    {
        textPrefabList.Add(goldTextPrefab);
        textPrefabList.Add(damageTextPrefab);
        foreach (GameObject textPrefab in textPrefabList)
        {
            poolDic.Add(textPrefab.GetComponent<TextPopUpUI>().type, new Queue<GameObject>());
            for (int i = 0; i < textPrefab.GetComponent<TextPopUpUI>().InitCount; i++)
            {
                GameObject temp = Instantiate(textPrefab);
                temp.SetActive(false);
                temp.transform.SetParent(this.transform);
                poolDic[textPrefab.GetComponent<TextPopUpUI>().type].Enqueue(temp);

            }
        }
    }

    public GameObject PopObj(TextType textPrefabType, Transform monsterTransform)//���ӿ�����Ʈ �̸� �־������
    {
        if (poolDic.ContainsKey(textPrefabType))
        {
            GameObject popObj = poolDic[textPrefabType].Dequeue();
            popObj.transform.position = monsterTransform.position;
            popObj.SetActive(true);
            return popObj;
        }

        return null;
    }

    public void ReturnPool(TextType textPrefabType, GameObject returnObj)
    {
        poolDic[textPrefabType].Enqueue(returnObj);
        returnObj.SetActive(false);
        //returnObj.transform.SetParent(this.transform);
    }
}