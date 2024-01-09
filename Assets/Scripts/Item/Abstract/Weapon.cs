using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : EquipmentItem
{
    public float additionalAtk;
    protected void Awake()
    {
        itemType = ItemType.weapon;
    }
    public override void Use(Player player)
    {

    }

    public override void Equip(Player player)
    {
        if (isEquip == false)
        {
            player.EquipWeapon = this;
            isEquip = true;
        }
        else
        {
            if(player.EquipWeapon != null)
            {
                player.EquipWeapon = this;
                isEquip = true;
            }
            else
            {
                player.EquipWeapon = null;
                isEquip = false;
            }
        }
    }
}
