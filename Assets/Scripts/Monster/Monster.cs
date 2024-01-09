using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum MonsterType
{
    None,
    Skeleton,
    Bear
}
public abstract class Monster : Character
{
    public NavMeshAgent agent;
    public MonsterType type;
    public static int monsterTotalCount;
    public static int bearTypeCount;
    public static int skeletonTypeCount;
    [SerializeField]
    private float unitExp;
    public int dropGold;
    public bool isDead;
    public bool isDeadAnimeOver;
    private Collider monCol;
    public Slider hpSlider;
    public float Hp
    {
        get { return hp; }
        set 
        {
            float prevHp = hp;
            hp = value;
            hpSlider.value = ((hp * 100) / maxHp) / 100;
            if (prevHp > hp)
            {
                animator.SetTrigger("Hit");
            }
            if (hp >= maxHp)
            {
                hp = maxHp;
            }
            if(hp <= 0)
            {
                isDead = true;
                monCol.enabled = false;
                hp = 0;
                GameManager.instance.player.Exp += unitExp;
                GameManager.instance.player.Gold += dropGold;
                animator.SetTrigger("Death");
            }
        }
    }
    protected new void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        monCol = GetComponent<Collider>();
    }
    private void OnEnable()
    {
        monsterTotalCount++;
        if (type == MonsterType.Bear)
        {
            bearTypeCount++;
        }
        if (type == MonsterType.Skeleton)
        {
            skeletonTypeCount++;
        }
    }
    private void OnDisable()
    {
        monsterTotalCount--;
        if (type == MonsterType.Bear)
        {
            bearTypeCount--;
        }
        if (type == MonsterType.Skeleton)
        {
            skeletonTypeCount--;
        }
    }
    void Update()
    {
        CheckAnime();
        hpSlider.transform.LookAt(GameManager.instance.mainCamera.transform) ;
    }
    void CheckAnime()
    {
        if (isDead == false)
        {
            float AttackRange = 2f;
            int PlayerLayerMask = 1 << 7;
            if (Physics.Raycast(transform.position, (GameManager.instance.player.transform.position - transform.position).normalized, AttackRange, PlayerLayerMask))
            { 
                animator.SetTrigger("Attack");
            }
        }
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && animator.GetCurrentAnimatorStateInfo(0).IsName("Death") && !isDeadAnimeOver)
        {
            Death();
            isDeadAnimeOver = true;
        }
        animator.SetFloat("Speed", Mathf.Abs(agent.velocity.x)); 
    }

    protected virtual void Death()
    {
        StartCoroutine(PopGoldAndReturnPool(transform));
    }

    protected void SetStatDefault()
    {
        isDead = false;
        monCol.enabled = true;
        hp = maxHp;
        hpSlider.value = 1;
    }

    public void WeaponColTrue()
    {
       GetComponentInChildren<MonsterAttackController>().weaponCol.enabled = true;
    }


    IEnumerator PopGoldAndReturnPool(Transform targetTransform)
    {
        TextType tempType = GameManager.instance.textObjectPool.goldTextPrefab.GetComponent<TextPopUpUI>().type;
        GameObject temp = GameManager.instance.textObjectPool.PopObj(tempType, targetTransform);
        temp.GetComponent<TextPopUpUI>().popupText.text = "+ " + dropGold.ToString() + "gold";
        float yprev = temp.transform.position.y;//기존좌표
        while (temp.activeSelf)
        {
            SoundManager.instance.soundObjectPool.Pop(SoundManager.instance.coindropSound, GameManager.instance.player.transform);

            temp.transform.LookAt(GameManager.instance.mainCamera.transform.position);
            while (temp.transform.position.y <= yprev + 3f)//이미지 천천히 올라가는것
            {
                temp.transform.position = new Vector3(temp.transform.position.x, Mathf.Lerp(temp.transform.position.y, temp.transform.position.y + 3f, Time.deltaTime), temp.transform.position.z);
                yield return null;
            }
            yield return new WaitForSeconds(1f);
            SetStatDefault();//몬스터 스텟 다시 초기화
            GameManager.instance.textObjectPool.ReturnPool(tempType, temp);//텍스트 풀 돌려주기
            GameManager.instance.monsterObjectPool.ReturnPool(type, gameObject);//몬스터 풀돌려주기
        }
    }
}
