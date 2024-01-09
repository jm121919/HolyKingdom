using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestNpc : MonoBehaviour
{
    public Quest curNpcQuest;
    public GameObject questIconObj;
    private void Start()
    {
        UIManager.instance.questNpc = this;
    }
}
