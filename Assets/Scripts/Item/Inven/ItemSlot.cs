using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public enum EquipSlotType
{
    None = 0,
    weaponSlot,
    armorSlot,
}
public class ItemSlot : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject firstHierarchy;// empty가 두번째로 위치해야하니까 먼저공간에 넣어줌으로써 순위를 뒤로 미룬다
    private RectTransform rectTransform;
    public BaseSlot baseSlot;
    public EquipSlotType equipSlotType;

    public void Update()
    {
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
    public void OnPointerEnter(PointerEventData eventData)//팝업
    {
        if(baseSlot.item != null)
        {
            UIManager.instance.popupItemInfo.transform.position = Input.mousePosition;
            UIManager.instance.popupItemInfo.GetComponentInChildren<TextMeshProUGUI>().text = UIManager.instance.ShowItemText(baseSlot.item);
            UIManager.instance.popupItemInfo.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)//팝업
    {
        UIManager.instance.popupItemInfo.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        rectTransform = GetComponent<RectTransform>();
        baseSlot.empty.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (baseSlot.itemSlotObject.activeSelf)
        {
            eventData.pointerDrag.gameObject.transform.SetParent(UIManager.instance.mainCanvas.transform);
            eventData.pointerDrag.gameObject.transform.position = Input.mousePosition;

        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GraphicRaycaster raycaster;
        PointerEventData pointerEventData;
        EventSystem eventSystem;
        raycaster = UIManager.instance.mainCanvas.GetComponent<GraphicRaycaster>();
        eventSystem = GetComponent<EventSystem>();

        pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerEventData, results);

        BaseSlot findSlot = null;
        foreach (RaycastResult result in results)//레이쏜애들에서 슬롯찾기
        {
            if(result.gameObject.TryGetComponent(out BaseSlot temp))//넣어주기
            {
                findSlot = temp;
            }
        }
        //////////////여기까지는 레이를 쏴서 검출하는 과정 
        ////여기는 퀵 슬롯일때
        if (findSlot is BaseSlot && findSlot.itemSlotObject.GetComponent<QuickSlot>() != null && eventData.pointerDrag.gameObject.GetComponent<ItemSlot>().baseSlot.item.itemType == ItemType.consumable)
            //파인드슬롯이 베이스슬롯이고 , 퀵슬롯이고, 소모품을 끌어놓을때만 넣어짐
        { 
            eventData.pointerDrag.gameObject.transform.SetParent(firstHierarchy.transform);
            rectTransform.localPosition = Vector2.zero;
            BaseSlot targetSlot = findSlot;// 잘 가져옴 해당슬롯위치
            Item tempItem = targetSlot.item;
            targetSlot.SetItem(baseSlot.item);
            baseSlot.empty.SetActive(false);
            if(baseSlot.item.itemType == ItemType.consumable)
            {
                findSlot.countText.text = ((ConsumableItem)baseSlot.item).ItemCount.ToString();
                //퀵슬롯에 옮길때는 프로퍼티로 값이 바뀌는게 아니라서 정보최신화를 해줘야댐
            }
            findSlot.itemSlotObject.GetComponent<QuickSlot>().ownerSlot = eventData.pointerDrag.gameObject.GetComponent<ItemSlot>().baseSlot;
            //오너슬롯의 카운트 텍스트에 접근을 해야하니 오너슬롯으로 넣어줌
            return;
        }
        else if (findSlot is BaseSlot && findSlot.itemSlotObject.GetComponent<QuickSlot>() != null && eventData.pointerDrag.gameObject.GetComponent<ItemSlot>().baseSlot.item.itemType != ItemType.consumable)
            //장비를 퀵슬롯에 드래그할려고 할때
        {
            GoBack(eventData);
        }
        /////여기는 아이템슬롯이나 장비슬롯끼리의 상호작용 코드
        if (findSlot is BaseSlot && findSlot.itemSlotObject.GetComponent<ItemSlot>().equipSlotType == EquipSlotType.None)
            //슬롯이 찾아졌고 장비슬롯은 아닌데,
        {
            if (findSlot.item == null)//대상 슬롯 아이템 없으면
            {
                Swap(findSlot, eventData, false);
                return;
            }
            else if ((findSlot.item is ConsumableItem || eventData.pointerDrag.gameObject.GetComponent<ItemSlot>().baseSlot.item is ConsumableItem) && eventData.pointerDrag.gameObject.GetComponent<ItemSlot>().equipSlotType == EquipSlotType.None)//둘중 하나라도 소모품이고, 전슬롯이 장비창이 아니면
            {
                Swap(findSlot, eventData, false);
                return;
            }
            else if ((findSlot.item is ConsumableItem || eventData.pointerDrag.gameObject.GetComponent<ItemSlot>().baseSlot.item is ConsumableItem) && eventData.pointerDrag.gameObject.GetComponent<ItemSlot>().equipSlotType != EquipSlotType.None)//둘중 하나라도 소모품, 전슬롯 장비창
            {
                GoBack(eventData);
                return;
            }
            else if ((((EquipmentItem)findSlot.item).itemType != ((EquipmentItem)eventData.pointerDrag.gameObject.GetComponent<ItemSlot>().baseSlot.item).itemType) && eventData.pointerDrag.gameObject.GetComponent<ItemSlot>().equipSlotType != EquipSlotType.None) //두 상대 아이템의 타입이 같지 않다면
            {
                GoBack(eventData);
                return;
            }
            Swap(findSlot, eventData, false);//없어도 될듯?
        }
        // findSlot은 대상의 슬롯이고
        // 이벤트 데이터 슬롯은 그 전 대상의 슬롯을 의미
        else if (findSlot is BaseSlot && findSlot.itemSlotObject.GetComponent<ItemSlot>().equipSlotType != EquipSlotType.None)// 뒤 오브젝트가 장비슬롯이면 장착되게 구현해야함 이부분
        {
            ItemSlot findItemSlot = findSlot.itemSlotObject.GetComponent<ItemSlot>();
            switch (findItemSlot.equipSlotType)
            {
                case EquipSlotType.weaponSlot:
                    if (eventData.pointerDrag.gameObject.GetComponent<ItemSlot>().baseSlot.item is Weapon)//장비슬롯타입과 아이템 타입이 일치하는가? 검사
                    {
                        Swap(findSlot, eventData,true);
                    }
                    else { GoBack(eventData); }//일치하지 않으면 다시 원위치로 돌림
                    break;
                case EquipSlotType.armorSlot:
                    if (eventData.pointerDrag.gameObject.GetComponent<ItemSlot>().baseSlot.item is Armor)
                    {
                        Swap(findSlot, eventData, true);
                    }
                    else { GoBack(eventData); }//일치하지 않으면 다시 원위치로 돌림
                    break;
                default: 
                    break;
            }
        }
        else if (findSlot == null)// 드래그끝났을때 바로뒤 오브젝트가 슬롯이 아닌경우에
        {
            GoBack(eventData);
        }
        //Debug.LogWarning(eventData.pointerDrag.gameObject.GetComponent<Image>().raycastTarget);
    }
    public void Swap(BaseSlot findSlot, PointerEventData eventData, bool isEquipSlot)//두개 레이 불값이랑 데이터를 바꿔주는 함수
    {
        if(findSlot.item != null)
        {
            if (eventData.pointerDrag.gameObject.GetComponent<ItemSlot>().baseSlot.item.itemType == ItemType.consumable && findSlot.item.itemType != ItemType.consumable)
                //드래그해서 가져온 슬롯아이템이 소비타입이고 상대방슬롯은 소비가아닐때,
            {
                eventData.pointerDrag.gameObject.GetComponent<ItemSlot>().baseSlot.countText.text = null;
            }
            //반대의 경우
            else if (eventData.pointerDrag.gameObject.GetComponent<ItemSlot>().baseSlot.item.itemType != ItemType.consumable && findSlot.item.itemType == ItemType.consumable)
            {
                findSlot.countText.text = null;
            }
        }
        findSlot.itemSlotObject.GetComponent<Image>().raycastTarget = true;//대상슬롯의 레이불
        if (findSlot.item == null)
        {
            eventData.pointerDrag.gameObject.GetComponent<Image>().raycastTarget = false;//그전 슬롯의 레이불 나중에 if안으로 옮겨야함
        }
        eventData.pointerDrag.gameObject.transform.SetParent(firstHierarchy.transform);
        rectTransform.localPosition = Vector2.zero;
        BaseSlot targetSlot = findSlot;// 잘 가져옴 해당슬롯위치
        Item tempItem = targetSlot.item;
        //Debug.LogWarning("대상 아이템 : " + tempItem);
        targetSlot.SetItem(baseSlot.item);
        baseSlot.SetItem(tempItem);


        // 장비 실제 플레이어에 장착부분
        if (isEquipSlot)
        {
            SoundManager.instance.soundObjectPool.Pop(SoundManager.instance.equipSound, GameManager.instance.player.transform);

            ((EquipmentItem)findSlot.item).Equip(GameManager.instance.player);//아이템 장착코드
        }
        else if(isEquipSlot == false && findSlot.item is EquipmentItem)//장착슬롯으로 드래그가 아니고, 그냥 빈슬롯 드래그 혹은 차있는슬롯 드래그// 그리고 컨슈머블 아이템은 아니여야함
        {
            if(((EquipmentItem)findSlot.item).isEquip)//들고있는애가 장착슬롯에서 가져온거면 장착해제시켜줌
            {
                ((EquipmentItem)findSlot.item).Equip(GameManager.instance.player);//아이템 장착코드
            }
        }
    }
    public void GoBack(PointerEventData eventData)// 드래그 했을때 아무것도 없었을때의 원위치로 가는 함수
    {
        eventData.pointerDrag.gameObject.transform.SetParent(firstHierarchy.transform);
        rectTransform.localPosition = Vector2.zero;
        baseSlot.empty.SetActive(false);
    }
}
