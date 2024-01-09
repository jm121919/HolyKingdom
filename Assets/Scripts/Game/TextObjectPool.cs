using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextObjectPool : MonoBehaviour
{
    //TO DO LIST
    //1 몬스터를 여러마리 생성
    //2 풀 오브젝트에 부모로 false로 담음
    //3 필요할때 엑티브를 켜주고 특정위치에 켜줌

    public List<GameObject> textPrefabList = new List<GameObject>();
    public GameObject goldTextPrefab;
    public GameObject damageTextPrefab;
    Dictionary<TextType, Queue<GameObject>> poolDic = new Dictionary<TextType, Queue<GameObject>>();

    public void InitPool()//몬스터들타입은 프리팹으로 생성해서 어웨이크로 넣을수 없고 인스펙터에 미리 세팅이 되있어야 한다
                          //게임메니저에서 인게임 갔을때해줌
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

    public GameObject PopObj(TextType textPrefabType, Transform monsterTransform)//게임오브젝트 이름 넣어줘야함
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