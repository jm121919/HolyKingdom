using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuickSlot : MonoBehaviour,IPointerClickHandler
{
    public int quickSlotNum;//ÀÎ½ºÆåÅÍ¿¡¼­ °¢ ½½·Ô¹øÈ£ ¸Å°ÜÁÜ
    public BaseSlot baseSlot;
    public BaseSlot ownerSlot;
    Dictionary<int, KeyCode> quickSlotNumDic;
    public void OnPointerClick(PointerEventData eventData)
    {
        baseSlot.SetItem(null);
    }
    private void Start()
    {
        quickSlotNumDic = new Dictionary<int, KeyCode>()
        {
            { 0, KeyCode.Alpha0 },
            { 1, KeyCode.Alpha1 },
            { 2, KeyCode.Alpha2 },
            { 3, KeyCode.Alpha3 },
            { 4, KeyCode.Alpha4 },
            { 5, KeyCode.Alpha5 },
            { 6, KeyCode.Alpha6 },
            { 7, KeyCode.Alpha7 },
            { 8, KeyCode.Alpha8 },
            { 9, KeyCode.Alpha9 }
        };

    }
    private void Update()
    {
        QuickSlotUse();
        CheckTextObj();
    }
    void CheckTextObj()
    {
        if (baseSlot.item != null)
        {
            if (baseSlot.item.itemType != ItemType.consumable)
            {
                baseSlot.countText.gameObject.SetActive(false);
            }
            else if (baseSlot.item.itemType == ItemType.consumable)
            {
                baseSlot.countText.gameObject.SetActive(true);
            }
        }
    }

    void QuickSlotUse()
    {
        if (Input.GetKeyDown(quickSlotNumDic[quickSlotNum]) && baseSlot.item != null)//Äü½½·Ô Å° ´­·¶À»¶§
        {
            SoundManager.instance.soundObjectPool.Pop(SoundManager.instance.potionSound,GameManager.instance.player.transform);

            baseSlot.item.Use(GameManager.instance.player);
            QuickSlot[] quickSlots = UIManager.instance.quickSlotParent.GetComponentsInChildren<QuickSlot>();
            if (((ConsumableItem)ownerSlot.item).ItemCount == 0)//0µÇ¸é ¾ø¾ÖÁÜ
            {
                foreach (QuickSlot quickSlot in quickSlots)
                {
                    if (quickSlot.baseSlot.item == ownerSlot.item)
                    quickSlot.baseSlot.SetItem(null);
                }
                ownerSlot.SetItem(null);
                return;
            }
            //Debug.LogWarning(baseSlot.item.itemName + "»ç¿ë" + ownerSlot.name);
        }
    }
}
