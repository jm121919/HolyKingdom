using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
//public class Node
//{
//    public List<Node> nextNodes = new List<Node>();
//    public Quest quest;
//    public Node(Quest quest)
//    {
//        this.quest = quest;
//    }
//}

//public class QuestLinkedList//명시적으로 퀘스트를 담는다는걸 알기위해 퀘스트제네릭으로함
//{
//    //TO DO LIST
//    //1 노드들은 다음노드를 여러개 가르킬수있음
//    //2 시작노드와 끝노드가 의미가 없어짐 그럼
//    //3 배열에 노드를 추가할때마다 정보를 저장해놨다가 원할때 꺼내써야댐
//    //4 AT함수를 필히 구현해야함
//    public List<Node> nodeList = new List<Node>();
//    public void PushBack(Quest initQuest)//퀘스트본체와, 다음 퀘스트들을 가지고있다
//    {
//        Node initTemp = new Node(initQuest);

//        nodeList.Add(initTemp);//리스트에 총 목록을 저장해야 찾을 수 있음

//        if (initQuest.nextQuest != null)
//        {
//            for (int i = 0; i < initQuest.nextQuest.Count; i++)//다음 가르키는 카운트가 없다면 연결이 끊김
//            {
//                Debug.LogWarning(initQuest.nextQuest[i].questInfo.questName);
//                initTemp.nextNodes.Add(new Node(initQuest.nextQuest[i]));
//            }
//        }
//    }

//    public Quest Find(float index)
//    {
//        foreach (Node node in nodeList)
//        {
//            if (node.quest.questInfo.questIndex == index)
//            {
//                return node.quest;
//            }
//        }
//        return null;
//    }
//}

public class QuestManager : Singleton<QuestManager>
{
    //public QuestLinkedList questLinkedList;
    public List<Quest> questList;
    public Dictionary<int, Quest> questDictionary = new Dictionary<int, Quest>();
    private Quest playerCurQuest;
    public Quest PlayerCurQuest
    {
        get { return playerCurQuest; }
        set 
        { 
            playerCurQuest = value;
            UIManager.instance.questTitleText.text = playerCurQuest.questInfo.questName;
            UIManager.instance.questInText.text = playerCurQuest.questInfo.questChatText;
        }
    }
        

    private void Start()
    {
        //questLinkedList = new QuestLinkedList();

        //foreach (Quest quest in questList)
        //{
        //    questLinkedList.PushBack(quest);
        //}

        for (int i = 0; i < questList.Count; i++)
        {
            questDictionary.Add(questList[i].questInfo.questIndex, questList[i]);
        }
    }
}
