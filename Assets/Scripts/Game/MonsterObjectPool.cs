using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterObjectPool : MonoBehaviour
{
    //TO DO LIST
    //1 ���͸� �������� ����
    //2 Ǯ ������Ʈ�� �θ�� false�� ����
    //3 �ʿ��Ҷ� ��Ƽ�긦 ���ְ� Ư����ġ�� ����

    public List<GameObject> monsterPrefabList = new List<GameObject>();
    public GameObject skeletonPrefab;
    public GameObject bearPrefab;
    Dictionary<MonsterType,Queue<GameObject>> poolDic = new Dictionary<MonsterType, Queue<GameObject>>();
    public Vector3 spawnPosition1;
    public Vector3 spawnPosition2;
    float monsterCount = 15;
    public Action onMoveScene;

    public void Start()
    {
        spawnPosition1 = new Vector3(149.4f, -20f, 0);//�ո���
        spawnPosition2 = new Vector3(169.07f, -20f, 24.41f);//�ո���
    }

    public void InitPool()//���͵�Ÿ���� ���������� �����ؼ� �����ũ�� ������ ���� �ν����Ϳ� �̸� ������ ���־�� �Ѵ�
        //���Ӹ޴������� �ΰ��� ����������
    {
        monsterPrefabList.Add(skeletonPrefab);
        monsterPrefabList.Add(bearPrefab);
        foreach (GameObject monsterPrefab in monsterPrefabList)
        {
            poolDic.Add(monsterPrefab.GetComponent<Monster>().type, new Queue<GameObject>());
            for (int i = 0; i < monsterCount; i++)
            {
                GameObject temp = Instantiate(monsterPrefab);
                temp.SetActive(false);
                onMoveScene += () => { temp.SetActive(false); };
                temp.transform.SetParent(this.transform);
                poolDic[monsterPrefab.GetComponent<Monster>().type].Enqueue(temp);

            }
        }
    }

    public void PopObj(MonsterType monsterType)
    {

        if (poolDic.ContainsKey(monsterType))
        {
            GameObject popObj = poolDic[monsterType].Dequeue();
            popObj.transform.position = new Vector3(Random.Range(spawnPosition1.x, spawnPosition2.x), spawnPosition1.y, Random.Range(spawnPosition1.z, spawnPosition2.z));
            popObj.SetActive(true);
        }
    }

    public void ReturnPool(MonsterType monsterType,GameObject returnObj)
    {
        Debug.LogWarning("���� Ǯ �Լ�");
        poolDic[monsterType].Enqueue(returnObj);
        returnObj.SetActive(false);
    }
}
