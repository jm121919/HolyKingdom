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

    private void OnEnable()// ������ �������� ī��Ʈ�� �ȵ�
    {
        
    }
    private void OnDisable()
    {
        
    }
}
