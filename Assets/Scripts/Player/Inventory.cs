using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum InvenType
{
    Normal,
    Shop
}
public class Inventory : MonoBehaviour
{
    public List<Item> invenList;
    public List<BaseSlot> slots;
    public GameObject slotsObj;
    public InvenType invenType;

    public Shop npc;

    private void Start()
    {
        invenList = new List<Item>();
        slots = new List<BaseSlot>();

        if(invenType == InvenType.Normal )
        {
            slotsObj = UIManager.instance.playerInvenUIObject;
        }
        else if(invenType == InvenType.Shop )
        {
            slotsObj = UIManager.instance.shopUIObject;
        }

        BaseSlot[] tempSlot = slotsObj.GetComponentsInChildren<BaseSlot>();
        
        for(int i = 0; i < tempSlot.Length; i++)// 슬롯가져와서 빈 리스트에 실제 슬롯넣기
        {
            slots.Add(tempSlot[i]);
        }
        if(invenType == InvenType.Shop)//상점인벤이면 추가
        {
            ShopItemSet(npc.inventoryObj);
        }
    }
    

    public void ShopItemSet(GameObject inventoryObj)//npc가 시작할때 호출 상점인벤만
    {
        Item[] tempItems = inventoryObj.GetComponentsInChildren<Item>();
        foreach (Item item in tempItems)//받아오고 받아온만큼 상점에 실제로 넣어준다
        {
            AddItem(item);
            item.gameObject.SetActive(false);//비활성화해줘야 안보임 상점에 다 넣고
        }
    }

    public void AddItem(Item targetItem)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].item == null)
            {
                if (InvenType.Normal == invenType)//플레이어 슬롯
                {
                    // SetActive관련으로 전체 큰 유아이 오브젝트를 처음에 켜져있고 false로 UIManager에서 꺼주면
                    // 게임시작할때 유니티가 유아이들의 스크립트에 대한걸 알고 있음
                    // 하지만 처음부터 false가 되어있으면 유니티는 나중에 켜져도 스크립트의 정보를 죽어도 모름
                    // 자식껏도 전달되어 유니티가 스크립트 들을 알고있음 추가에 무리가 없음 즉 켜주고 꺼주기를 자식에서
                    // 안해도댐
                    //UIManager.instance.equipUIObject.SetActive(true);//시작될때 슬롯 활성화해줘야 데이터 들어감//
                    invenList.Add(targetItem);
                    slots[i].GetComponentInChildren<ItemSlot>().GetComponent<Image>().raycastTarget = true;
                    slots[i].SetItem(targetItem);
                    //UIManager.instance.equipUIObject.SetActive(false);//다넣었으면 슬롯 비활성화해줌
                }
                else if(InvenType.Shop == invenType)//상점인벤 슬롯
                {
                    invenList.Add(targetItem);
                    slots[i].SetItem(targetItem);
                }
                return;
            }
        }
    }
}
