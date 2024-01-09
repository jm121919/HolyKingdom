using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrkBoss : Monster
{
    protected override void Death()
    {
        SetStatDefault();
        Destroy(gameObject);
        UIManager.instance.Ending();
    }

    private void OnEnable()// 보스는 생성몬스터 카운트에 안들어감
    {
        
    }
    private void OnDisable()
    {
        
    }
}
