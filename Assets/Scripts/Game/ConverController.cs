using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using UnityEngine.Windows;

public class ConverController : MonoBehaviour
{
    public TextMeshProUGUI speakerNameText;//������Ʈ
    public TextMeshProUGUI converText;
    public Button okButton;
    public Button noButton;
    public List<string> shopEnterTextsList = new List<string>();
    public List<string> QuestEnterTextsList = new List<string>();
    Dictionary<string , List<string>> textListDic = new Dictionary<string , List<string>>();
    private void Start()
    {
        SetText();
    }
    void  SetText()
    {
        textListDic.Add("shop", shopEnterTextsList);
        textListDic.Add("quest", QuestEnterTextsList);
        shopEnterTextsList.Add("�ȳ��ϽŰ� ���谡�� �̰��� ��� ��ȭ�� ��� ����ϴ� �����̳�, ���� �ٱ��� �߻������� ������ ���⼭ " +
    "���ǰ� ��� ���߾ �����°� �������ϼ�, ����� ������ 20% �����߿� �ֳ�");
        shopEnterTextsList.Add("�ѷ������ΰ�? (��ư�� ������ ����)");
        QuestEnterTextsList.Add("�ȳ��ϽŰ� �̼����� ���谡�� �츮�� ���� �����߿� �ִ� ��Ȳ�̶��, �ٱ�����" +
            "��ũ ���ܰ� ���� ���Ͱ� ����Ͽ� ���� ȥ���� �����µ�");
        QuestEnterTextsList.Add("�ڳװ� ��ũ������ ����ְڳ�? (��ư�� ������ ����)");
    }

    public void StartCoChat(string name)//���� �̸����� �޾ƾ���
    {
        if(name == "shop")
        {
            StartCoroutine(Chat("��������", textListDic[name]));
            UIManager.instance.curConverState = ConverState.shop;
        }
        if(name == "quest")
        {
            StartCoroutine(Chat("����", textListDic[name]));
            UIManager.instance.curConverState = ConverState.quest;
        }
    }
    public IEnumerator Chat(string targetSpeakerName, List<string> targetConverTextList)
    {
        UIManager.instance.isConverActive = true;
        UIManager.instance.mainUIObject.SetActive(false);
        speakerNameText.text = targetSpeakerName;
        converText.text = null;
        for (int i = 0; i < targetConverTextList.Count; i++)
        {
            for (int j = 0; j < targetConverTextList[i].Length; j++)
            {
                converText.text += targetConverTextList[i][j];
                yield return new WaitForSeconds(0.0125f);//�����ð�
            }
            yield return new WaitForSeconds(1f);
            if(i == 0)
            converText.text = null;
        }
        okButton.gameObject.SetActive(true);
        noButton.gameObject.SetActive(true);
    }
}
