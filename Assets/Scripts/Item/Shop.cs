using UnityEngine;

public class Shop : MonoBehaviour
{
    public Inventory inventory;
    public GameObject inventoryObj;
    public static Item pickItem;
    public Player player;
    public Item clone;//������ Ÿ������
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
                //����Ƽ�� ������������ ó����������� false�� �߱� ������ ������ �˰��־� ��Ȱ��ȭ �Ȱ��� �����͵� �ȴ�
                if (baseslot.item.itemName == pickItem.itemName && baseslot.item.itemType == ItemType.consumable)//�����۰� ���������� ���� ������Ÿ���� ��������̸�
                {
                    ((ConsumableItem)baseslot.item).ItemCount++;//������ �ټ� �÷��ش�
                    return true;
                }
            }
        }
        return false;
    }
}
