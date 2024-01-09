using UnityEngine;

public class Shop : MonoBehaviour
{
    public Inventory inventory;
    public GameObject inventoryObj;
    public static Item pickItem;
    public Player player;
    public Item clone;//프로토 타입패턴
    public GameObject shopIconObj;

    private void Awake()
    {
        UIManager.instance.shop = this;
    }

    public bool CheckInvenConsum()
    {
        foreach (BaseSlot baseslot in player.inventory.slots)
        {
            if(baseslot.item != null)
            {
                //유니티가 상점아이템이 처음만들어지고 false를 했기 때문에 정보를 알고있어 비활성화 된것을 가져와도 된다
                if (baseslot.item.itemName == pickItem.itemName && baseslot.item.itemType == ItemType.consumable)//아이템과 고른아이템이 같고 아이템타입이 컨슘어블이면
                {
                    ((ConsumableItem)baseslot.item).ItemCount++;//아이템 겟수 늘려준다
                    return true;
                }
            }
        }
        return false;
    }
}
