using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MpPotion : ConsumableItem
{
    public float mpPlus;
    private void Start()
    {
    }
    public override void Use(Player player)
    {
        ItemCount--;
        player.MP += mpPlus;
    }
    protected override void SetStat()
    {

    }

}
