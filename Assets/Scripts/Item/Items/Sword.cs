using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwordType
{
    Katana,
    GreatSword
}
public class Sword : Weapon
{
    public SwordType swordType;
    private void Start()
    {
    }
    public override void Use(Player player)
    {
        base.Use(player);
    }

    protected override void SetStat()
    {

    }
}
