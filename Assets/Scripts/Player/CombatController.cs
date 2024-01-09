using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CombatController : MonoBehaviour
{
    private Animator animator;
    private Player player;

    public GameObject[] effects;

    //코루틴 이용 변수들
    float comboTimeCurrent = 0f;
    float comboTimeMax = 5f;//몇초 안에 눌러야 다음콤보 나감
    bool CheckAttackable
    {
        get { return player.IsAction == false; }
    }
    public static bool IsCurAttackAnime
    {
        get { return (GameManager.instance.player.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack3Com") || GameManager.instance.player.animator.GetCurrentAnimatorStateInfo(0).IsName("Skill") || GameManager.instance.player.animator.GetCurrentAnimatorStateInfo(0).IsName("DashAttack")); }
    }
    //코루틴 이용 변수들
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

        if (Input.GetKeyDown(KeyCode.Q))//1번스킬 검기
        {
            animator.SetFloat("SkillIndex", 1);
            player.ChangeState(Player_State.Skill);
        }
        if (Input.GetKeyDown(KeyCode.E))//2번스킬 올려내리꽃기
        {
            animator.SetFloat("SkillIndex", 2);
            player.ChangeState(Player_State.Skill);
        }
        if (Input.GetKeyDown(KeyCode.R))//3번스킬 올려배기
        {
            animator.SetFloat("SkillIndex", 3);
            player.ChangeState(Player_State.Skill);
        }
        if (Input.GetKeyDown(KeyCode.T))//4번스킬 찌르기
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
        //연속클릭 방지 코드
        //콤보중 단계 초기화 코드
        if (0 < comboTimeCurrent && comboTimeCurrent <= comboTimeMax)
        {
            comboTimeCurrent -= Time.deltaTime;
        }
        if (Input.GetMouseButtonDown(0) && CheckAttackable && !EventSystem.current.IsPointerOverGameObject())//3콤보 공격
                                                                                                             //유아이 클릭하면 공격안하게 트루반환이라 반전해줌
        {
            SoundManager.instance.soundObjectPool.Pop(SoundManager.instance.attackSound, GameManager.instance.player.transform);

            player.ChangeState(Player_State.Attack);//변경사항
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
    public void WeaponColTrue()//각 애니메이션의 이벤트에 들어감
    {
        GetComponentInChildren<WeaponController>().weaponCol.enabled = true;
    }
}
