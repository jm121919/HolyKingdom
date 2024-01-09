using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum ConverState
{
    None,
    shop,
    quest
}
public class UIManager : Singleton<UIManager>
{
    //게임오브젝트 연결시켜줄 변수
    public Player player;
    public GameObject mainCanvas;
    public GameObject mainUIObject;
    public GameObject invenUIObject;//인벤 유아이 전체
    public GameObject equipUIObject;//장착부분
    public GameObject InfoUIObject;//정보부분
    public GameObject shopUIObject;//상점유아이 옵젝
    public GameObject quickSlotParent;//퀵슬롯부모
    public GameObject playerInvenUIObject;//아이템인벤만
    public GameObject loadingSceneUIObject;//로딩화면
    public GameObject mainmanuUIObject;//메인메뉴
    public TextMeshProUGUI clockText;
    public TextMeshProUGUI loadingProgressText;
    public TextMeshProUGUI loadingConverText;
    public Texture2D cursorIcon;
    public GameObject endingObj;
    //퀘스트 정보연결
    public QuestNpc questNpc;
    public TextMeshProUGUI questTitleText;
    public TextMeshProUGUI questInText;
    //슬라이더 
    public Slider hpSlider;
    public Slider mpSlider;
    public Slider expSlider;
    public Slider loadSlider;
    //팝업 오브젝트
    public GameObject popupGoldLow;
    public GameObject popupItemInfo;
    public GameObject escMenuObj;
    //대화창 관련 오브젝트
    public GameObject converUIObj;
    public GameObject converTextObj;
    public GameObject speakerTextObj;
    //상점과 플레이어 정보연결 관련 변수
    public Shop shop;
    public Image shopItemImage;
    public TextMeshProUGUI shopStatText;
    public TextMeshProUGUI shopPlayerMoneyText;
    public TextMeshProUGUI shopPriceText;
    public TextMeshProUGUI invenGoldText;
    public TextMeshProUGUI playerFaceLvText;
    //밤낮조절 변수
    public Light worldLight;
    public bool isNight;
    public Material daySkybox;
    public Material nightSkybox;
    //무기모드 관련 변수
    public Image WeaponModeUI;
    public Sprite greatSwordSprite;
    public Sprite katanaSprite;
    //유아이변수
    public ConverState curConverState;
    public bool isInvenActive;
    public bool isShopActive;
    public bool isConverActive;
    public bool isEscMenuActive;
    public bool IsUIActive//UI가 켜져있으면, 카메라 막고 커서를 풀기위해
    {
        get { return isInvenActive || isShopActive || isConverActive || isEscMenuActive; }
    }
    public List<BaseSlot> allQandISlots;//퀵슬롯과 인벤슬롯 합친것
    //텍스트 출력 저장소
    private List<string> loadingTextsList;
    //이벤트 오브젝트
    public MonsterObjectPool monsterObjectPool;
    // Start is called before the first frame update
    void Start()
    {
        SetLoadingText();
        Cursor.SetCursor(cursorIcon, Vector2.zero, CursorMode.Auto);
        SetAllQandISlots();
        SetUI();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.isSceneInGame)
        {
            DayNightControll();
            CheckUIKeyDownActive();
            ShowUI();
        }
    }

    public void Ending()
    {
        endingObj.SetActive(true);
        StartCoroutine(EndingCo());
    }
    IEnumerator EndingCo()
    {
        while (true)
        {
            endingObj.GetComponentInChildren<TextMeshProUGUI>().color = new Color(Random.Range(0f,1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            yield return new WaitForSeconds(0.1f);
        }
    }

    void SetLoadingText()
    {
        loadingTextsList = new List<string>()
        {
            "유명한 자들이 우리에게 오는 경우도 있고, 명성을 얻기 위해 우릴 찾아오는 자들도 있지. 그들 사이에는 아무런 차이가 없다네. - 코드락 화이트메인",
            "네가 다가올 때 갑자기 바람의 방향이 바뀌는 것을 느낄 수 있었지. 결국 우리 둘은 서로 칼을 섞게 될 것을 알 수 있었다. - 머서 프레이",
            "이것이 바로 놈들이 바라던 일이란 것을 알게 될 것이다. 이곳의 자원을 우회시키던 놈들, 바로 탈모어 말이지! - 툴리우스 장군",
            "타락하고 죽어가는 제국의 손에 절대로 스카이림을 넘기지 않겠다. 스카이림은 나의 것이 아니지. 하지만 나는 스카이림의 것이다. - 울프릭 스톰클록",
            "우리의 언어를 알아듣지도 못하는군. 그러면서 감히 용의 이름을 쓰다니, 오만하기 짝이 없구나! - 알두인, 세계의 포식자"
        };
    }
    void SetAllQandISlots()
    {
        allQandISlots = new List<BaseSlot>();
        BaseSlot[] temp = equipUIObject.GetComponentsInChildren<BaseSlot>();
        foreach (BaseSlot baseSlot in temp)
        {
            if(baseSlot.itemSlotObject.GetComponent<ItemSlot>().equipSlotType == EquipSlotType.None)
                //장비슬롯은 안가져옴
            {
                allQandISlots.Add(baseSlot);
            }
        }
        temp = quickSlotParent.GetComponentsInChildren<BaseSlot>();
        foreach (BaseSlot baseSlot in temp)
        {
            allQandISlots.Add(baseSlot);
        }
    }

    void ShowUI()
    {
        clockText.text = DateTime.Now.ToString("MM/dd HH시:mm분:ss초");//현제시간 표시
    }
    void SetUI()//유아이들을 전부 active true 상태로 인스펙터에 놓아야함
    {
        // SetActive관련으로 전체 큰 유아이 오브젝트를 처음에 켜져있고 false로 UIManager에서 꺼주면
        // 게임시작할때 유니티가 유아이들의 스크립트에 대한걸 알고 있음
        // 하지만 처음부터 false가 되어있으면 유니티는 나중에 켜져도 스크립트의 정보를 죽어도 모름
        // 자식껏도 전달되어 유니티가 스크립트 들을 알고있음 추가에 무리가 없음 즉 켜주고 꺼주기를 자식에서
        // 안해도댐
        invenUIObject.SetActive(false);// 유니티에 처음존재를 알려야지만 상호작용이 가능해지기때문에 켜논상태로 시작될때 꺼주기만한다
        //이과정을 안거치고 다른물체와 상호작용을하면 유니티는 존재를 몰라 오류가 생긴다
        shopUIObject.SetActive(false);
        mainUIObject.SetActive(false);
        loadingSceneUIObject.SetActive(false) ;
        WeaponModeUI.gameObject.SetActive(false);
        converUIObj.gameObject.SetActive(false);
    }
    void CheckUIKeyDownActive()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (invenUIObject.activeSelf)
            {
                invenUIObject.SetActive(false);
                isInvenActive = false;
                popupItemInfo.gameObject.SetActive(false);//팝업 남아있음 꺼줌
            }
            else
            {
                SoundManager.instance.soundObjectPool.Pop(SoundManager.instance.uiPopSound, transform);

                invenUIObject.SetActive(true);
                invenGoldText.text = GameManager.instance.player.Gold.ToString();//켜질때 한번 넣어줘야 초기값 최신화, 나머지는 프로퍼티가해줌
                isInvenActive = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.U))//디버깅용 필수로 바꿔야함 상점유아이 팝업
        {
            if (shopUIObject.activeSelf)
            {
                shopUIObject.SetActive(false);
                isShopActive = false;
            }
            else
            {
                shopUIObject.SetActive(true);
                shopPlayerMoneyText.text = "보유금액 : " + GameManager.instance.player.Gold;//켜질때 한번 넣어줘야 초기값 최신화, 나머지는 프로퍼티가해줌
                isShopActive = true;
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (escMenuObj.activeSelf)
            {
                escMenuObj.SetActive(false);
                isEscMenuActive = false;
            }
            else
            {
                escMenuObj.SetActive(true);
                isEscMenuActive = true;
            }
        }
    }

    void DayNightControll()//직접 데이 듀라션 스크립트랑 상호작용 맞춰야함
    {
        if (isNight)//밤에서 낮
        {
            worldLight.intensity += Time.deltaTime / 100;//밤낮 조절
            if (worldLight.intensity >= 1)
            {
                isNight = false;
                RenderSettings.skybox = daySkybox;
            }
        }
        else//낮에서 밤
        {
            worldLight.intensity -= Time.deltaTime / 100;//밤낮 조절
            if (worldLight.intensity <= 0)
            {
                isNight = true;
                RenderSettings.skybox = nightSkybox;
            }
        }
        }

    public void Xbutton(string uiName)
    {
        switch (uiName)
        {
            case "ShopUI":
                shopUIObject.SetActive(!isShopActive);
                isShopActive = !isShopActive;
                break;
            case "InventoryUI":
                invenUIObject.SetActive(!isInvenActive);
                isInvenActive = !isInvenActive;
                break;
            default:
                break;
        }

    }

    public void GameStartButton()
    {
        StartCoroutine(LoadSceneAsync("InGame"));
    }

    public void GameExitButton()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    public void BackMenuButton()
    {
        escMenuObj.SetActive(false);
        isEscMenuActive = false;
        mainmanuUIObject.SetActive(true);
        monsterObjectPool.onMoveScene();
        SceneManager.LoadScene("MainMenu");
    }
    IEnumerator LoadSceneAsync(string sceneName)
    {
        loadingSceneUIObject.SetActive(true);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;
        float timer = 0;
        loadingConverText.text = loadingTextsList[UnityEngine.Random.Range(0, 5)];
        while (true)
        {
            yield return null;
            timer += Time.deltaTime;//로딩시간 조절 나누기로 해줌
            loadingProgressText.text = "로딩중.. " + (loadSlider.value * 100) + "%";
            if (asyncLoad.progress < 0.9f)
            {
                loadSlider.value = Mathf.Lerp(0, 1f, timer);
                if(loadSlider.value >= asyncLoad.progress)
                {
                    timer = 0;
                }
            }
            else//로딩전부끝나고 씬이동하는부분
            {
                loadSlider.value = Mathf.Lerp(loadSlider.value, 1f, timer);
                if(loadSlider.value >=0.999f)
                {
                    loadingProgressText.text = "로딩중.. " + 100 + "%";
                    asyncLoad.allowSceneActivation = true;
                    break;
                }
            }
        }
        yield return new WaitForSeconds(2f);//로딩끝나고 씬이동되는 마지막코드 여기서  비활성화걸어야댐
        loadingSceneUIObject.SetActive(false);
        mainmanuUIObject.SetActive(false);
    }
    public void PurchaseButton()//아이템 실제 추가
    {
        if (Shop.pickItem != null)
        {
            if (player.Gold >= Shop.pickItem.price)//아이템 사는버튼 골드가 있으면
            {
                SoundManager.instance.soundObjectPool.Pop(SoundManager.instance.coindropSound, GameManager.instance.player.transform);
                if (Shop.pickItem.itemType == ItemType.consumable && shop.CheckInvenConsum())//소모템이고 인벤이 이미 있을시에 갯수만 추가
                {
                    player.Gold -= Shop.pickItem.price;

                }
                else if (Shop.pickItem.itemType == ItemType.consumable) //소모품의 첫 구매면
                {
                    shop.clone = Instantiate(Shop.pickItem, UIManager.instance.player.invenObj.transform);//인스턴트를 해서 원본은 보존하고 클론을 실제 인벤으로 전달, 인벤의 데이터가 바뀌어도 원본틀은 유지
                    ((ConsumableItem)shop.clone).ItemCount++;//여기만 다름 이유는 첫구매에 카운트 증가를 여기서 해줄수 밖에 없음
                    player.inventory.AddItem(shop.clone);
                    player.Gold -= Shop.pickItem.price;
                }
                else//장비면 
                {
                    //실제 인벤 오브젝트에 클론만들어진걸 넣어줘서 보존해준다
                    shop.clone = Instantiate(Shop.pickItem, UIManager.instance.player.invenObj.transform);//인스턴트를 해서 원본은 보존하고 클론을 실제 인벤으로 전달, 인벤의 데이터가 바뀌어도 원본틀은 유지
                    player.inventory.AddItem(shop.clone);
                    player.Gold -= Shop.pickItem.price;
                }
            }
            else if (player.Gold < Shop.pickItem.price)
            {
                UIManager.instance.popupGoldLow.SetActive(true);
            }
        }
    }

    public void ConverButton(bool isOkbutton)
    {
        isConverActive = false;
        converUIObj.SetActive(false);
        if (isOkbutton)//수락
        {
            if(curConverState == ConverState.shop)
            {
                shopUIObject.SetActive(true);
            }
            if(curConverState == ConverState.quest)
            {
                //퀘스트 클리어로 돌리고 클리어면 자동 다음퀘스트 수주
                QuestManager.instance.PlayerCurQuest.State = QuestState.IsClear;
            }
            curConverState = ConverState.None;
        }
        else if(!isOkbutton)//거절
        {
            //아무것도안함 아직
        }
        mainUIObject.SetActive(true);
        converUIObj.GetComponent<ConverController>().okButton.gameObject.SetActive(false);
        converUIObj.GetComponent<ConverController>().noButton.gameObject.SetActive(false);
    }

    public string ShowItemText(Item item)
    {
        if(item != null)
        {
            switch (item.itemType)
            {
                case ItemType.consumable:
                    return
                    "이름 : " + item.itemName + "\n" +
                    "타입 : " + "소모품" + "\n" +
                    "설명 : " + item.itemInfo + "\n";
                case ItemType.armor:
                    return
                    "이름 : " + item.itemName + "\n" +
                    "타입 : " + "방어구" + "\n" +
                    "체력 증가량 : " + ((Armor)item).additionalHp + "\n" +
                    "설명 : " + item.itemInfo + "\n";
                case ItemType.weapon:
                    return
                    "이름 : " + item.itemName + "\n" +
                    "타입 : " + "무기" + "\n" +
                    "공격력 증가량 : " + ((Weapon)item).additionalAtk + "\n" +
                    "설명 : " + item.itemInfo + "\n";
                default:
                    return null;
            }
        }
        return null;
    }
}
