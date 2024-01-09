using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum QuestState
{
    None,
    IsStartAble,
    IsClear,
}

[Serializable]
public class Quest 
{
    public QuestInfo questInfo;
    public QuestState state;
    public QuestState State
    {
        get { return state; }
        set
        {
            if (value == QuestState.IsClear)
            {
                QuestManager.instance.PlayerCurQuest = QuestManager.instance.questDictionary[QuestManager.instance.PlayerCurQuest.questInfo.questIndex + 1];
            }
            state = value;
        }
    }
    //public Quest nextQuest;//인스펙터로 직렬화해서 초기화 해줬음 , 타협을 위하여 그냥 다음퀘스트로 변경
}
