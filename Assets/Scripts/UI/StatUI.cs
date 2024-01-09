using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatUI : MonoBehaviour
{
    public TextMeshProUGUI[] statTexts;
    // Start is called before the first frame update
    void Start()
    {
        statTexts = new TextMeshProUGUI[7];
        statTexts = GetComponentsInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        statTexts[0].text = "LV: " + GameManager.instance.player.lv;
        statTexts[1].text = "�̸�: " + GameManager.instance.player.unitName;
        statTexts[4].text = "����ġ: " + GameManager.instance.player.Exp + "/" + GameManager.instance.player.playerExpTableDic[GameManager.instance.player.lv];
        if (GameManager.instance.player.EquipWeapon !=null)
        {
            statTexts[5].text = "��������: " + GameManager.instance.player.EquipWeapon.itemName;
            statTexts[3].text = "���ݷ�: " + GameManager.instance.player.atk + "<color=#aqua>" +" + (" + GameManager.instance.player.EquipWeapon.additionalAtk + ")" + "</color>";
        }
        else
        {
            statTexts[5].text = "��������: " + "����";
            statTexts[3].text = "���ݷ�: " + GameManager.instance.player.atk;
        }
        if (GameManager.instance.player.EquipArmor != null)
        {
            statTexts[6].text = "��������: " + GameManager.instance.player.EquipArmor.itemName;
            statTexts[2].text = "ü��: " + GameManager.instance.player.Hp + "/" + GameManager.instance.player.maxHp + "<color=#aqua>" + " + (" + GameManager.instance.player.EquipArmor.additionalHp + ")" + "</color>";
        }
        else
        {
            statTexts[6].text = "��������: " + "����";
            statTexts[2].text = "ü��: " + GameManager.instance.player.Hp + "/" + GameManager.instance.player.maxHp;
        }
    }
}
