using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class BaseSlot : MonoBehaviour
{
    public Item item;
    public Image itemImage;
    public GameObject itemSlotObject;
    public GameObject empty;
    public TextMeshProUGUI countText;
    private void Start()
    {
        item = null;
    }

    public void SetItem(Item targetItem)
    {
        item = targetItem;
        if(item != null)
        {
            if (item.itemType == ItemType.consumable && itemSlotObject.GetComponent<ItemSlot>() != null)//아이템슬롯에서 넣어줄때, 소모아이템을, 소모아이템은 처음말고는 Set을 안해주고 카운트만증가
                //따라서 이코드는 처음한번만 실행
            {
                itemImage.sprite = targetItem.sprite;
                countText.text = ((ConsumableItem)item).ItemCount.ToString();//처음에컨슈머블 아이템 세팅
                //처음이라 프로퍼티 사용이 안되서 정보최신화를 수동으로 했음
                empty.SetActive(item == null);
                return;
            }
            itemImage.sprite = targetItem.sprite;
        }
        else//비었을때 세팅 꼭해줘야댐
        {
            itemImage.sprite = null;
        }
        empty.SetActive(item == null);
    }

}
