using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterObjectPool : MonoBehaviour
{
    //TO DO LIST
    //1 몬스터를 여러마리 생성
    //2 풀 오브젝트에 부모로 false로 담음
    //3 필요할때 엑티브를 켜주고 특정위치에 켜줌

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
        spawnPosition1 = new Vector3(149.4f, -20f, 0);//앞마당
        spawnPosition2 = new Vector3(169.07f, -20f, 24.41f);//앞마당
    }

    public void InitPool()//몬스터들타입은 프리팹으로 생성해서 어웨이크로 넣을수 없고 인스펙터에 미리 세팅이 되있어야 한다
        //게임메니저에서 인게임 갔을때해줌
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
        Debug.LogWarning("리턴 풀 함수");
        poolDic[monsterType].Enqueue(returnObj);
        returnObj.SetActive(false);
    }
}
