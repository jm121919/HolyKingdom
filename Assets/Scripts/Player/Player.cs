using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum Player_State//애니메이터와 직접연결 순서바꾸면 애니메이터 건들여야함
{
    None,
    Idle = 1,
    Walk = 2,
    Run = 3,
    Attack= 4,
    Dodge= 5,
    Jump= 6 ,
    Hit= 7,
    Skill= 8,
}
public class Player : Character
{

    private PlayerStateMachine stateMachine;
    public Player_State curPlayerState;
    private Dictionary<Player_State, PlayerState> playerStateDic;
    //플레이어 레벨에 대응하는 수치를 저장할 딕셔너리들
    public Dictionary<int, int> playerExpTableDic = new Dictionary<int, int>(); 
    public Dictionary<int, int> playerAtkTableDic = new Dictionary<int, int>();
    public Dictionary<int, int> playerHpTableDic = new Dictionary<int, int>();
    public Dictionary<string, Dictionary<int, int>> playerLvUPTableDic = new Dictionary<string, Dictionary<int, int>>();
    /////
    [SerializeField]
    private Weapon equipWeapon;
    private Armor equipArmor;
   
    public GameObject invenObj;
    public GameObject WeaponsParentObj;
    public Inventory inventory;
    public TextMeshProUGUI earnGoldText;


    public RuntimeAnimatorController katanaAnimator;
    public RuntimeAnimatorController greatSwordAnimator;
    //플레이어 변수들
    public int lv;
    private float exp;
    [SerializeField]
    private int gold;

    public Weapon EquipWeapon// 변경될때 액션
    {
        get { return equipWeapon; }
        set
        {
            if (equipWeapon != null)//먼저 껴져있는 무기를 빼줌
            {
                UIManager.instance.WeaponModeUI.gameObject.SetActive(false);
                animator.runtimeAnimatorController = katanaAnimator;
                equipWeapon.gameObject.transform.SetParent(invenObj.transform);
                equipWeapon.gameObject.SetActive(false);
                atk -= equipWeapon.additionalAtk;
                equipWeapon = value;
            }
            if (value != null)
            {
                //무기 실제로 손에 위치하게 하는것과, 무기 애니메이션 변경과, 유아이 이미지 변경까지
                if (((Sword)value).swordType == SwordType.Katana)
                {
                    animator.runtimeAnimatorController = katanaAnimator;
                    UIManager.instance.WeaponModeUI.gameObject.SetActive(true);
                    UIManager.instance.WeaponModeUI.sprite = UIManager.instance.katanaSprite;
                }
                else if (((Sword)value).swordType == SwordType.GreatSword)
                {
                    animator.runtimeAnimatorController = greatSwordAnimator;
                    UIManager.instance.WeaponModeUI.gameObject.SetActive(true);
                    UIManager.instance.WeaponModeUI.sprite = UIManager.instance.greatSwordSprite;
                }
                equipWeapon = value;
                atk += equipWeapon.additionalAtk;
                equipWeapon.gameObject.transform.SetParent(WeaponsParentObj.transform);
                equipWeapon.gameObject.transform.localPosition = Vector3.zero;
                equipWeapon.gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
                equipWeapon.gameObject.transform.localScale = new Vector3(100,100,100);
                equipWeapon.gameObject.SetActive(true);
            }
        }
    }
    public Armor EquipArmor
    {
        get { return equipArmor; }
        set 
        {
            if (value != null)
            {
                equipArmor = value;
                maxHp += equipArmor.additionalHp;
            }
            else if (value == null)
            {
                if(hp == maxHp)
                hp -= equipArmor.additionalHp;

                maxHp -= equipArmor.additionalHp;
                equipArmor = value;
            }
            UIManager.instance.hpSlider.value = ((hp * 100) / maxHp) / 100;//백분위 계산기 백분위 나온걸 100으로 나눠줘야 벨류 0~1까지 가능해진다
        }
    }
    public bool IsAction
    {
        get //무언가 액션중이면 트루반환
        {
            return (curPlayerState == Player_State.Attack || curPlayerState == Player_State.Dodge || curPlayerState == Player_State.Jump || curPlayerState == Player_State.Hit || curPlayerState == Player_State.Skill);
        }
    }
    public float Hp
    {
        get { return hp; }
        set
        {
            float prevHp = hp;
            hp = value;
            if(hp < prevHp)
            {
                ChangeState(Player_State.Hit);
            }

            if(hp >= maxHp)
            {
                hp = maxHp;
            }
            else if(hp <= 0)
            {
                hp = 0;
            }
            UIManager.instance.hpSlider.value = ((hp * 100) / maxHp) / 100;//백분위 계산기 백분위 나온걸 100으로 나눠줘야 벨류 0~1까지 가능해진다
        }
    }
    public float MaxHP
    {
        get { return hp; }
        set 
        {
            if(equipArmor != null)
            {
                maxHp = value + equipArmor.additionalHp;
            }
            else
            {
                maxHp = value;
            }
            hp = maxHp;
            UIManager.instance.hpSlider.value = ((hp * 100) / maxHp) / 100;//백분위 계산기 백분위 나온걸 100으로 나눠줘야 벨류 0~1까지 가능해진다
        }
    }
    public float MP
    { 
        get { return mp; } 
        set 
        { 
            mp = value;
            UIManager.instance.mpSlider.value = ((mp * 100) / maxMp) / 100;//백분위 계산기 백분위 나온걸 100으로 나눠줘야 벨류 0~1까지 가능해진다
        }
    }
    public float Exp
    {
        get { return exp; }
        set 
        { 
            exp = value;
            if(exp >= playerExpTableDic[lv])//레벨업
            {
                LvUP();
                UIManager.instance.playerFaceLvText.text = "Lv :" + lv;
            }
            UIManager.instance.expSlider.value = ((exp * 100) / playerLvUPTableDic["Exp"][lv]) / 100;//백분위 계산기 백분위 나온걸 100으로 나눠줘야 벨류 0~1까지 가능해진다
        }
    }

    public int Gold
    {
        get { return gold; }
        set 
        { 
            gold = value;
            UIManager.instance.shopPlayerMoneyText.text = "보유금액 : " + GameManager.instance.player.Gold;//켜질때 한번 넣어줘야 들어감
            if(UIManager.instance.invenUIObject.activeSelf)
            {
                UIManager.instance.invenGoldText.text = GameManager.instance.player.Gold.ToString();//켜질때 한번 넣어줘야 들어감
            }
            else
            {
                UIManager.instance.invenUIObject.SetActive(true);
                UIManager.instance.invenGoldText.text = GameManager.instance.player.Gold.ToString();//켜져있어야 넣어지니까 꺼져있음켜주고넘
                UIManager.instance.invenUIObject.SetActive(false);
            }

        }
    }
    private void Start()
    {
        QuestManager.instance.PlayerCurQuest = QuestManager.instance.questDictionary[1];
        StateFirstSetting();
        DicSet();
    }
    private void Update()
    {
        stateMachine.UpdateState();
        animator.SetInteger("State", (int)curPlayerState);
        CheckNpc();
    }
    private void FixedUpdate()
    {
        Gravity();
    }

    void Gravity()
    {
        rb.AddForce(Vector3.down * 60f);//중력값
    }
    void DicSet()//레벨업 테이블 값 세팅
    {
        playerLvUPTableDic.Add("Exp", playerExpTableDic);
        playerLvUPTableDic.Add("Atk", playerAtkTableDic);
        playerLvUPTableDic.Add("Hp", playerHpTableDic);
        for (int i = 1; i < 10; i++)
        {
            playerExpTableDic.Add(i, i * 100);// 레벨, 레벨까지의 경험치
            playerAtkTableDic.Add(i, i * 10); // 추가 공격력, 공격력 테이블
            playerHpTableDic.Add(i, i * 20); // 추가 체력
        }
    }
    void LvUP()
    {
        exp = 0;
        lv++;
        atk += playerLvUPTableDic["Atk"][lv];
        MaxHP += playerLvUPTableDic["Hp"][lv];
    }
    private void StateFirstSetting()
    {
        GameManager.instance.player = this;
        UIManager.instance.player = this;
        playerStateDic = new Dictionary<Player_State, PlayerState>()//딕셔너리 초기화
        {
            { Player_State.Idle, new IdleState(this) },
            { Player_State.Walk, new WalkState(this)},
            { Player_State.Run, new RunState(this)},
            { Player_State.Attack, new AttackState(this)},
            { Player_State.Dodge, new DodgeState(this)},
            { Player_State.Jump, new JumpState(this)},
            { Player_State.Hit, new HitState(this)},
            { Player_State.Skill, new SkillState(this)}
        };

        curPlayerState = Player_State.Idle;//처음상태
        stateMachine = new PlayerStateMachine(playerStateDic[curPlayerState]);
    }
    public void ChangeState(Player_State InitState)
    {
        curPlayerState = InitState;
        stateMachine.ChangeState(playerStateDic[InitState]);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Item targetItem))//아이템 주웠을때
        {
            targetItem.gameObject.SetActive(false);
            targetItem.gameObject.transform.SetParent(invenObj.transform);
            inventory.AddItem(targetItem);
        }
    }

    void CheckNpc()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, 3f, 1 << 11);
        if (cols.Length > 0)//앞에 npc있고 f누르면 대화가능
        {
            if (cols[0].gameObject.GetComponent<QuestNpc>())//퀘스트 엔피씨면
            {
                UIManager.instance.questNpc.questIconObj.SetActive(true);
            }
            if (cols[0].gameObject.GetComponent<Shop>())
            {
                UIManager.instance.shop.shopIconObj.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.F) && UIManager.instance.IsUIActive == false)
            {
                UIManager.instance.converUIObj.SetActive(true);
                if (cols[0].gameObject.GetComponent<QuestNpc>())//퀘스트 엔피씨면
                {
                    UIManager.instance.converUIObj.GetComponent<ConverController>().StartCoChat("quest");
                }
                if (cols[0].gameObject.GetComponent<Shop>())
                {
                    UIManager.instance.converUIObj.GetComponent<ConverController>().StartCoChat("shop");
                }
                return;
            }
        }
        else
        {
            UIManager.instance.shop.shopIconObj.SetActive(false);
            UIManager.instance.questNpc.questIconObj.SetActive(false);
        }
    }
}
