using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipmentItem : Item
{
    public bool isEquip;
    public abstract void Equip(Player player);
}
