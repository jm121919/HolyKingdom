using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    None,
    consumable,
    armor,
    weapon
}
public abstract class Item : MonoBehaviour
{
    public Sprite sprite;
    [SerializeField]
    public string itemName;
    [SerializeField]
    public int price;
    [SerializeField]
    public ItemType itemType;
    public string itemInfo;

    public abstract void Use(Player player);
    protected abstract void SetStat();
}
