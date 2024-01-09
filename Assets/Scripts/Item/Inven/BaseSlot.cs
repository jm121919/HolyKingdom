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
            if (item.itemType == ItemType.consumable && itemSlotObject.GetComponent<ItemSlot>() != null)//�����۽��Կ��� �־��ٶ�, �Ҹ��������, �Ҹ�������� ó������� Set�� �����ְ� ī��Ʈ������
                //���� ���ڵ�� ó���ѹ��� ����
            {
                itemImage.sprite = targetItem.sprite;
                countText.text = ((ConsumableItem)item).ItemCount.ToString();//ó���������Ӻ� ������ ����
                //ó���̶� ������Ƽ ����� �ȵǼ� �����ֽ�ȭ�� �������� ����
                empty.SetActive(item == null);
                return;
            }
            itemImage.sprite = targetItem.sprite;
        }
        else//������� ���� ������ߴ�
        {
            itemImage.sprite = null;
        }
        empty.SetActive(item == null);
    }

}
