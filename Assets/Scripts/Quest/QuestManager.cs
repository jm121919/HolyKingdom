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

//public class QuestLinkedList//��������� ����Ʈ�� ��´ٴ°� �˱����� ����Ʈ���׸�������
//{
//    //TO DO LIST
//    //1 ������ ������带 ������ ����ų������
//    //2 ���۳��� ����尡 �ǹ̰� ������ �׷�
//    //3 �迭�� ��带 �߰��Ҷ����� ������ �����س��ٰ� ���Ҷ� ������ߴ�
//    //4 AT�Լ��� ���� �����ؾ���
//    public List<Node> nodeList = new List<Node>();
//    public void PushBack(Quest initQuest)//����Ʈ��ü��, ���� ����Ʈ���� �������ִ�
//    {
//        Node initTemp = new Node(initQuest);

//        nodeList.Add(initTemp);//����Ʈ�� �� ����� �����ؾ� ã�� �� ����

//        if (initQuest.nextQuest != null)
//        {
//            for (int i = 0; i < initQuest.nextQuest.Count; i++)//���� ����Ű�� ī��Ʈ�� ���ٸ� ������ ����
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
