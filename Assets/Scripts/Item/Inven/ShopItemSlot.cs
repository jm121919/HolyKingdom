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
            Shop.pickItem = baseSlot.item;//선택중인 아이템으로 연결
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
            UIManager.instance.shopPriceText.text = "가격 : " + baseSlot.item.price;//아이템 가격
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
