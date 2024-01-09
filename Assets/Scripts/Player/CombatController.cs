using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CombatController : MonoBehaviour
{
    private Animator animator;
    private Player player;

    public GameObject[] effects;

    //�ڷ�ƾ �̿� ������
    float comboTimeCurrent = 0f;
    float comboTimeMax = 5f;//���� �ȿ� ������ �����޺� ����
    bool CheckAttackable
    {
        get { return player.IsAction == false; }
    }
    public static bool IsCurAttackAnime
    {
        get { return (GameManager.instance.player.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack3Com") || GameManager.instance.player.animator.GetCurrentAnimatorStateInfo(0).IsName("Skill") || GameManager.instance.player.animator.GetCurrentAnimatorStateInfo(0).IsName("DashAttack")); }
    }
    //�ڷ�ƾ �̿� ������
    void Start()
    {
        player = GetComponent<Player>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckAnimeIdleState();
        if (GameManager.instance.player.EquipWeapon != null)
        {
            SkillUse();
            Combat();
        }
    }

    void SkillUse()
    {

        if (Input.GetKeyDown(KeyCode.Q))//1����ų �˱�
        {
            animator.SetFloat("SkillIndex", 1);
            player.ChangeState(Player_State.Skill);
        }
        if (Input.GetKeyDown(KeyCode.E))//2����ų �÷������ɱ�
        {
            animator.SetFloat("SkillIndex", 2);
            player.ChangeState(Player_State.Skill);
        }
        if (Input.GetKeyDown(KeyCode.R))//3����ų �÷����
        {
            animator.SetFloat("SkillIndex", 3);
            player.ChangeState(Player_State.Skill);
        }
        if (Input.GetKeyDown(KeyCode.T))//4����ų ���
        {
            animator.SetFloat("SkillIndex", 4);
            player.ChangeState(Player_State.Skill);
        }
    }
    public void Skill1()
    {
        WeaponController curweapon = GetComponentInChildren<WeaponController>();
        //Instantiate(effects[0], curweapon.transform.position, curweapon.transform.rotation);
        GameObject effect = Instantiate(effects[0], transform);
        effect.transform.Rotate(Vector3.forward * 45,Space.Self);
    }
    void Combat()
    {
        //����Ŭ�� ���� �ڵ�
        //�޺��� �ܰ� �ʱ�ȭ �ڵ�
        if (0 < comboTimeCurrent && comboTimeCurrent <= comboTimeMax)
        {
            comboTimeCurrent -= Time.deltaTime;
        }
        if (Input.GetMouseButtonDown(0) && CheckAttackable && !EventSystem.current.IsPointerOverGameObject())//3�޺� ����
                                                                                                             //������ Ŭ���ϸ� ���ݾ��ϰ� Ʈ���ȯ�̶� ��������
        {
            SoundManager.instance.soundObjectPool.Pop(SoundManager.instance.attackSound, GameManager.instance.player.transform);

            player.ChangeState(Player_State.Attack);//�������
            if (AttackState.attackComboIndex == 3)
            {
                player.animator.SetFloat("AttackCombo", AttackState.attackComboIndex);
                AttackState.attackComboIndex = 1;
                return;
            }
            player.animator.SetFloat("AttackCombo", AttackState.attackComboIndex);
            AttackState.attackComboIndex++;
        }
    }
    void CheckAnimeIdleState()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && IsCurAttackAnime)
        {
            player.ChangeState(Player_State.Idle);
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("DashAttack"))
            {
                AttackState.attackComboIndex = 1;
            }
        }
    }
    public void WeaponColTrue()//�� �ִϸ��̼��� �̺�Ʈ�� ��
    {
        GetComponentInChildren<WeaponController>().weaponCol.enabled = true;
    }
}
