using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SoundObjectPool : MonoBehaviour
{
    [SerializeField]
    GameObject soundComponentPrefab;
    Queue<GameObject> pool = new Queue<GameObject>();
    int clipMaxCount;

    private void Start()
    {
        clipMaxCount = 50;
        Init();
    }
    void Init()
    {
        for (int i = 0; i < clipMaxCount; i++)
        {
            GameObject temp = Instantiate(soundComponentPrefab, transform);
            temp.SetActive(false);
            pool.Enqueue(temp);
        }
    }

    public void Pop(AudioClip audioClip, Transform targetTransform, bool isLoop = false)//�ƹ��͵����ָ� false
    {
        //���� �Ұ��� �޴´ٸ� ������ �����ְ� ���ϵɶ��� �������� 
        GameObject temp = pool.Dequeue();
        temp.SetActive(true);
        temp.transform.SetParent(targetTransform);
        if(isLoop)
        {
            temp.GetComponent<AudioSource>().loop = true;
        }
        temp.GetComponent<SoundComponent>().Play(audioClip);
    }

    public void ReturnPool(GameObject targetObj)
    {
        if(targetObj.GetComponent<AudioSource>().loop == true)
        {
            targetObj.GetComponent<AudioSource>().loop = false;
        }
        targetObj.transform.SetParent(this.transform);
        targetObj.SetActive(false);
        pool.Enqueue(targetObj);
    }
}
