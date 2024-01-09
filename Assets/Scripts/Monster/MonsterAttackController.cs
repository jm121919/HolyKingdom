using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackController : MonoBehaviour
{
    private Animator animator;
    public Collider weaponCol;
    void Start()
    {
        weaponCol = GetComponent<Collider>();
        animator = GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        WeaponColFalse();
    }
    public void WeaponColFalse()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            weaponCol.enabled = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Player foundPlayer;
        if (other.gameObject.TryGetComponent(out foundPlayer))
        {
            weaponCol.enabled = false;
            foundPlayer.Hp -= GetComponentInParent<Monster>().atk;
            Debug.LogWarning("플레이어 남은 체력 : " + foundPlayer.Hp);
        }
    }
}
