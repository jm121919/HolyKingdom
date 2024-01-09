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
    public TextMeshProUGUI speakerNameText;//컴포넌트
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
        shopEnterTextsList.Add("안녕하신가 모험가여 이곳은 모든 잡화와 장비를 취급하는 상점이네, 성문 바깥은 야생동물이 많으니 여기서 " +
    "포션과 장비를 갖추어서 나가는게 좋을거일세, 참고로 포션은 20% 할인중에 있네");
        shopEnterTextsList.Add("둘러볼것인가? (버튼을 눌러서 진행)");
        QuestEnterTextsList.Add("안녕하신가 이세계의 모험가여 우리는 지금 전쟁중에 있는 상황이라네, 바깥에서" +
            "오크 군단과 여러 몬스터가 출몰하여 성이 혼란에 빠졌는데");
        QuestEnterTextsList.Add("자네가 오크대장을 잡아주겠나? (버튼을 눌러서 진행)");
    }

    public void StartCoChat(string name)//영어 이름으로 받아야함
    {
        if(name == "shop")
        {
            StartCoroutine(Chat("상점주인", textListDic[name]));
            UIManager.instance.curConverState = ConverState.shop;
        }
        if(name == "quest")
        {
            StartCoroutine(Chat("국왕", textListDic[name]));
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
                yield return new WaitForSeconds(0.0125f);//지연시간
            }
            yield return new WaitForSeconds(1f);
            if(i == 0)
            converText.text = null;
        }
        okButton.gameObject.SetActive(true);
        noButton.gameObject.SetActive(true);
    }
}
