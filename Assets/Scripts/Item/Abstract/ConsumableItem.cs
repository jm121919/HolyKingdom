using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConsumableItem : Item
{
    private int itemCount;//������ ����
    public int ItemCount
    {
        get { return itemCount; }
        set //������ ���� ���� �ֽ�ȭ �����丵 �ڵ�
        { 
            //�� �������� ����ִ� ��ü�� countText ������Ʈ�� �����ؾ���
            itemCount = value;
            foreach (BaseSlot baseslot in UIManager.instance.allQandISlots)
            {
                if (baseslot.item == this)
                {
                    baseslot.countText.text = ((ConsumableItem)baseslot.item).itemCount.ToString();//�ý�Ʈ �ֽ�ȭ
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
