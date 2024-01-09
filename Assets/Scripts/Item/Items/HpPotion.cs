using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpPotion : ConsumableItem
{
    public float hpPlus;
    private void Start()
    {
    }
    public override void Use(Player player)
    {
        ItemCount--;
        player.Hp += hpPlus;
    }
    protected override void SetStat()
    {

    }

}
