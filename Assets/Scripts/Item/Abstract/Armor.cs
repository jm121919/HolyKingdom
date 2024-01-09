using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Armor : EquipmentItem
{
    public float additionalHp;
    protected void Awake()
    {
        itemType = ItemType.armor;
    }
    public override void Use(Player player)
    {
    }
    public override void Equip(Player player)
    {
        if (isEquip == false)
        {
            if (player.EquipArmor != null)
            {
                player.EquipArmor.Use(player);
            }
            player.EquipArmor = this;
        }
        else
        {
            player.EquipArmor = null;
        }
        isEquip = !isEquip;
    }
}

