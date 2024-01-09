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
    public GameObject firstHierarchy;// empty�� �ι�°�� ��ġ�ؾ��ϴϱ� ���������� �־������ν� ������ �ڷ� �̷��
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
    public void OnPointerEnter(PointerEventData eventData)//�˾�
    {
        if(baseSlot.item != null)
        {
            UIManager.instance.popupItemInfo.transform.position = Input.mousePosition;
            UIManager.instance.popupItemInfo.GetComponentInChildren<TextMeshProUGUI>().text = UIManager.instance.ShowItemText(baseSlot.item);
            UIManager.instance.popupItemInfo.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)//�˾�
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
        foreach (RaycastResult result in results)//���̽�ֵ鿡�� ����ã��
        {
            if(result.gameObject.TryGetComponent(out BaseSlot temp))//�־��ֱ�
            {
                findSlot = temp;
            }
        }
        //////////////��������� ���̸� ���� �����ϴ� ���� 
        ////����� �� �����϶�
        if (findSlot is BaseSlot && findSlot.itemSlotObject.GetComponent<QuickSlot>() != null && eventData.pointerDrag.gameObject.GetComponent<ItemSlot>().baseSlot.item.itemType == ItemType.consumable)
            //���ε彽���� ���̽������̰� , �������̰�, �Ҹ�ǰ�� ����������� �־���
        { 
            eventData.pointerDrag.gameObject.transform.SetParent(firstHierarchy.transform);
            rectTransform.localPosition = Vector2.zero;
            BaseSlot targetSlot = findSlot;// �� ������ �ش罽����ġ
            Item tempItem = targetSlot.item;
            targetSlot.SetItem(baseSlot.item);
            baseSlot.empty.SetActive(false);
            if(baseSlot.item.itemType == ItemType.consumable)
            {
                findSlot.countText.text = ((ConsumableItem)baseSlot.item).ItemCount.ToString();
                //�����Կ� �ű涧�� ������Ƽ�� ���� �ٲ�°� �ƴ϶� �����ֽ�ȭ�� ����ߴ�
            }
            findSlot.itemSlotObject.GetComponent<QuickSlot>().ownerSlot = eventData.pointerDrag.gameObject.GetComponent<ItemSlot>().baseSlot;
            //���ʽ����� ī��Ʈ �ؽ�Ʈ�� ������ �ؾ��ϴ� ���ʽ������� �־���
            return;
        }
        else if (findSlot is BaseSlot && findSlot.itemSlotObject.GetComponent<QuickSlot>() != null && eventData.pointerDrag.gameObject.GetComponent<ItemSlot>().baseSlot.item.itemType != ItemType.consumable)
            //��� �����Կ� �巡���ҷ��� �Ҷ�
        {
            GoBack(eventData);
        }
        /////����� �����۽����̳� ��񽽷Գ����� ��ȣ�ۿ� �ڵ�
        if (findSlot is BaseSlot && findSlot.itemSlotObject.GetComponent<ItemSlot>().equipSlotType == EquipSlotType.None)
            //������ ã������ ��񽽷��� �ƴѵ�,
        {
            if (findSlot.item == null)//��� ���� ������ ������
            {
                Swap(findSlot, eventData, false);
                return;
            }
            else if ((findSlot.item is ConsumableItem || eventData.pointerDrag.gameObject.GetComponent<ItemSlot>().baseSlot.item is ConsumableItem) && eventData.pointerDrag.gameObject.GetComponent<ItemSlot>().equipSlotType == EquipSlotType.None)//���� �ϳ��� �Ҹ�ǰ�̰�, �������� ���â�� �ƴϸ�
            {
                Swap(findSlot, eventData, false);
                return;
            }
            else if ((findSlot.item is ConsumableItem || eventData.pointerDrag.gameObject.GetComponent<ItemSlot>().baseSlot.item is ConsumableItem) && eventData.pointerDrag.gameObject.GetComponent<ItemSlot>().equipSlotType != EquipSlotType.None)//���� �ϳ��� �Ҹ�ǰ, ������ ���â
            {
                GoBack(eventData);
                return;
            }
            else if ((((EquipmentItem)findSlot.item).itemType != ((EquipmentItem)eventData.pointerDrag.gameObject.GetComponent<ItemSlot>().baseSlot.item).itemType) && eventData.pointerDrag.gameObject.GetComponent<ItemSlot>().equipSlotType != EquipSlotType.None) //�� ��� �������� Ÿ���� ���� �ʴٸ�
            {
                GoBack(eventData);
                return;
            }
            Swap(findSlot, eventData, false);//��� �ɵ�?
        }
        // findSlot�� ����� �����̰�
        // �̺�Ʈ ������ ������ �� �� ����� ������ �ǹ�
        else if (findSlot is BaseSlot && findSlot.itemSlotObject.GetComponent<ItemSlot>().equipSlotType != EquipSlotType.None)// �� ������Ʈ�� ��񽽷��̸� �����ǰ� �����ؾ��� �̺κ�
        {
            ItemSlot findItemSlot = findSlot.itemSlotObject.GetComponent<ItemSlot>();
            switch (findItemSlot.equipSlotType)
            {
                case EquipSlotType.weaponSlot:
                    if (eventData.pointerDrag.gameObject.GetComponent<ItemSlot>().baseSlot.item is Weapon)//��񽽷�Ÿ�԰� ������ Ÿ���� ��ġ�ϴ°�? �˻�
                    {
                        Swap(findSlot, eventData,true);
                    }
                    else { GoBack(eventData); }//��ġ���� ������ �ٽ� ����ġ�� ����
                    break;
                case EquipSlotType.armorSlot:
                    if (eventData.pointerDrag.gameObject.GetComponent<ItemSlot>().baseSlot.item is Armor)
                    {
                        Swap(findSlot, eventData, true);
                    }
                    else { GoBack(eventData); }//��ġ���� ������ �ٽ� ����ġ�� ����
                    break;
                default: 
                    break;
            }
        }
        else if (findSlot == null)// �巡�׳������� �ٷε� ������Ʈ�� ������ �ƴѰ�쿡
        {
            GoBack(eventData);
        }
        //Debug.LogWarning(eventData.pointerDrag.gameObject.GetComponent<Image>().raycastTarget);
    }
    public void Swap(BaseSlot findSlot, PointerEventData eventData, bool isEquipSlot)//�ΰ� ���� �Ұ��̶� �����͸� �ٲ��ִ� �Լ�
    {
        if(findSlot.item != null)
        {
            if (eventData.pointerDrag.gameObject.GetComponent<ItemSlot>().baseSlot.item.itemType == ItemType.consumable && findSlot.item.itemType != ItemType.consumable)
                //�巡���ؼ� ������ ���Ծ������� �Һ�Ÿ���̰� ���潽���� �Һ񰡾ƴҶ�,
            {
                eventData.pointerDrag.gameObject.GetComponent<ItemSlot>().baseSlot.countText.text = null;
            }
            //�ݴ��� ���
            else if (eventData.pointerDrag.gameObject.GetComponent<ItemSlot>().baseSlot.item.itemType != ItemType.consumable && findSlot.item.itemType == ItemType.consumable)
            {
                findSlot.countText.text = null;
            }
        }
        findSlot.itemSlotObject.GetComponent<Image>().raycastTarget = true;//��󽽷��� ���̺�
        if (findSlot.item == null)
        {
            eventData.pointerDrag.gameObject.GetComponent<Image>().raycastTarget = false;//���� ������ ���̺� ���߿� if������ �Űܾ���
        }
        eventData.pointerDrag.gameObject.transform.SetParent(firstHierarchy.transform);
        rectTransform.localPosition = Vector2.zero;
        BaseSlot targetSlot = findSlot;// �� ������ �ش罽����ġ
        Item tempItem = targetSlot.item;
        //Debug.LogWarning("��� ������ : " + tempItem);
        targetSlot.SetItem(baseSlot.item);
        baseSlot.SetItem(tempItem);


        // ��� ���� �÷��̾ �����κ�
        if (isEquipSlot)
        {
            SoundManager.instance.soundObjectPool.Pop(SoundManager.instance.equipSound, GameManager.instance.player.transform);

            ((EquipmentItem)findSlot.item).Equip(GameManager.instance.player);//������ �����ڵ�
        }
        else if(isEquipSlot == false && findSlot.item is EquipmentItem)//������������ �巡�װ� �ƴϰ�, �׳� �󽽷� �巡�� Ȥ�� ���ִ½��� �巡��// �׸��� �����Ӻ� �������� �ƴϿ�����
        {
            if(((EquipmentItem)findSlot.item).isEquip)//����ִ¾ְ� �������Կ��� �����°Ÿ� ��������������
            {
                ((EquipmentItem)findSlot.item).Equip(GameManager.instance.player);//������ �����ڵ�
            }
        }
    }
    public void GoBack(PointerEventData eventData)// �巡�� ������ �ƹ��͵� ���������� ����ġ�� ���� �Լ�
    {
        eventData.pointerDrag.gameObject.transform.SetParent(firstHierarchy.transform);
        rectTransform.localPosition = Vector2.zero;
        baseSlot.empty.SetActive(false);
    }
}
