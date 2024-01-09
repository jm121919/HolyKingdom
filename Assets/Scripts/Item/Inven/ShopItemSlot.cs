using UnityEngine;
using UnityEngine.EventSystems;

public class ShopItemSlot : MonoBehaviour,IPointerClickHandler
{
    public BaseSlot baseSlot;
    public void OnPointerClick(PointerEventData eventData)
    {
        ShowItemInfo(baseSlot);

        if(baseSlot.item != null)
        {
            Shop.pickItem = baseSlot.item;//�������� ���������� ����
        }
        else if(baseSlot.item == null)
        {
            Shop.pickItem = null;
        }

    }

    void ShowItemInfo(BaseSlot baseSlot)
    {
        if(baseSlot.item != null)
        {
            UIManager.instance.shopItemImage.enabled = true;
            UIManager.instance.shopStatText.text = UIManager.instance.ShowItemText(baseSlot.item);
            UIManager.instance.shopItemImage.sprite = baseSlot.itemImage.sprite;
            UIManager.instance.shopPriceText.text = "���� : " + baseSlot.item.price;//������ ����
        }
        else if (baseSlot.item == null)
        {
            UIManager.instance.shopItemImage.sprite = null;
            UIManager.instance.shopStatText.text = null;
            UIManager.instance.shopPriceText.text = null;
            UIManager.instance.shopItemImage.enabled = false;
        }
    }
}
