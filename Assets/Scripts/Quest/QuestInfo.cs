using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestInfo", menuName = "Scriptable Object/QuestInfo", order = 0)]

public class QuestInfo : ScriptableObject
{
    public string questName;
    public string questChatText;
    public int questIndex;
    public int questNeedCount;
}
