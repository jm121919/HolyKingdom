using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConsumableItem : Item
{
    private int itemCount;//아이템 갯수
    public int ItemCount
    {
        get { return itemCount; }
        set //아이템 갯수 정보 최신화 리팩토링 코드
        { 
            //그 아이템이 들어있는 객체의 countText 오브젝트에 접근해야함
            itemCount = value;
            foreach (BaseSlot baseslot in UIManager.instance.allQandISlots)
            {
                if (baseslot.item == this)
                {
                    baseslot.countText.text = ((ConsumableItem)baseslot.item).itemCount.ToString();//택스트 최신화
                }
            }    
            
        }
    }
    private void Awake()
    {
        itemCount = 0;
        itemType = ItemType.consumable;
    }
}
