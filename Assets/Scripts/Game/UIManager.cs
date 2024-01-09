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
    //���ӿ�����Ʈ ��������� ����
    public Player player;
    public GameObject mainCanvas;
    public GameObject mainUIObject;
    public GameObject invenUIObject;//�κ� ������ ��ü
    public GameObject equipUIObject;//�����κ�
    public GameObject InfoUIObject;//�����κ�
    public GameObject shopUIObject;//���������� ����
    public GameObject quickSlotParent;//�����Ժθ�
    public GameObject playerInvenUIObject;//�������κ���
    public GameObject loadingSceneUIObject;//�ε�ȭ��
    public GameObject mainmanuUIObject;//���θ޴�
    public TextMeshProUGUI clockText;
    public TextMeshProUGUI loadingProgressText;
    public TextMeshProUGUI loadingConverText;
    public Texture2D cursorIcon;
    public GameObject endingObj;
    //����Ʈ ��������
    public QuestNpc questNpc;
    public TextMeshProUGUI questTitleText;
    public TextMeshProUGUI questInText;
    //�����̴� 
    public Slider hpSlider;
    public Slider mpSlider;
    public Slider expSlider;
    public Slider loadSlider;
    //�˾� ������Ʈ
    public GameObject popupGoldLow;
    public GameObject popupItemInfo;
    public GameObject escMenuObj;
    //��ȭâ ���� ������Ʈ
    public GameObject converUIObj;
    public GameObject converTextObj;
    public GameObject speakerTextObj;
    //������ �÷��̾� �������� ���� ����
    public Shop shop;
    public Image shopItemImage;
    public TextMeshProUGUI shopStatText;
    public TextMeshProUGUI shopPlayerMoneyText;
    public TextMeshProUGUI shopPriceText;
    public TextMeshProUGUI invenGoldText;
    public TextMeshProUGUI playerFaceLvText;
    //�㳷���� ����
    public Light worldLight;
    public bool isNight;
    public Material daySkybox;
    public Material nightSkybox;
    //������ ���� ����
    public Image WeaponModeUI;
    public Sprite greatSwordSprite;
    public Sprite katanaSprite;
    //�����̺���
    public ConverState curConverState;
    public bool isInvenActive;
    public bool isShopActive;
    public bool isConverActive;
    public bool isEscMenuActive;
    public bool IsUIActive//UI�� ����������, ī�޶� ���� Ŀ���� Ǯ������
    {
        get { return isInvenActive || isShopActive || isConverActive || isEscMenuActive; }
    }
    public List<BaseSlot> allQandISlots;//�����԰� �κ����� ��ģ��
    //�ؽ�Ʈ ��� �����
    private List<string> loadingTextsList;
    //�̺�Ʈ ������Ʈ
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
            "������ �ڵ��� �츮���� ���� ��쵵 �ְ�, ���� ��� ���� �츱 ã�ƿ��� �ڵ鵵 ����. �׵� ���̿��� �ƹ��� ���̰� ���ٳ�. - �ڵ�� ȭ��Ʈ����",
            "�װ� �ٰ��� �� ���ڱ� �ٶ��� ������ �ٲ�� ���� ���� �� �־���. �ᱹ �츮 ���� ���� Į�� ���� �� ���� �� �� �־���. - �Ӽ� ������",
            "�̰��� �ٷ� ����� �ٶ�� ���̶� ���� �˰� �� ���̴�. �̰��� �ڿ��� ��ȸ��Ű�� ���, �ٷ� Ż��� ������! - �����콺 �屺",
            "Ÿ���ϰ� �׾�� ������ �տ� ����� ��ī�̸��� �ѱ��� �ʰڴ�. ��ī�̸��� ���� ���� �ƴ���. ������ ���� ��ī�̸��� ���̴�. - ������ ����Ŭ��",
            "�츮�� �� �˾Ƶ����� ���ϴ±�. �׷��鼭 ���� ���� �̸��� ���ٴ�, �����ϱ� ¦�� ������! - �˵���, ������ ������"
        };
    }
    void SetAllQandISlots()
    {
        allQandISlots = new List<BaseSlot>();
        BaseSlot[] temp = equipUIObject.GetComponentsInChildren<BaseSlot>();
        foreach (BaseSlot baseSlot in temp)
        {
            if(baseSlot.itemSlotObject.GetComponent<ItemSlot>().equipSlotType == EquipSlotType.None)
                //��񽽷��� �Ȱ�����
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
        clockText.text = DateTime.Now.ToString("MM/dd HH��:mm��:ss��");//�����ð� ǥ��
    }
    void SetUI()//�����̵��� ���� active true ���·� �ν����Ϳ� ���ƾ���
    {
        // SetActive�������� ��ü ū ������ ������Ʈ�� ó���� �����ְ� false�� UIManager���� ���ָ�
        // ���ӽ����Ҷ� ����Ƽ�� �����̵��� ��ũ��Ʈ�� ���Ѱ� �˰� ����
        // ������ ó������ false�� �Ǿ������� ����Ƽ�� ���߿� ������ ��ũ��Ʈ�� ������ �׾ ��
        // �ڽĲ��� ���޵Ǿ� ����Ƽ�� ��ũ��Ʈ ���� �˰����� �߰��� ������ ���� �� ���ְ� ���ֱ⸦ �ڽĿ���
        // ���ص���
        invenUIObject.SetActive(false);// ����Ƽ�� ó�����縦 �˷������� ��ȣ�ۿ��� ���������⶧���� �ѳ���·� ���۵ɶ� ���ֱ⸸�Ѵ�
        //�̰����� �Ȱ�ġ�� �ٸ���ü�� ��ȣ�ۿ����ϸ� ����Ƽ�� ���縦 ���� ������ �����
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
                popupItemInfo.gameObject.SetActive(false);//�˾� �������� ����
            }
            else
            {
                SoundManager.instance.soundObjectPool.Pop(SoundManager.instance.uiPopSound, transform);

                invenUIObject.SetActive(true);
                invenGoldText.text = GameManager.instance.player.Gold.ToString();//������ �ѹ� �־���� �ʱⰪ �ֽ�ȭ, �������� ������Ƽ������
                isInvenActive = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.U))//������ �ʼ��� �ٲ���� ���������� �˾�
        {
            if (shopUIObject.activeSelf)
            {
                shopUIObject.SetActive(false);
                isShopActive = false;
            }
            else
            {
                shopUIObject.SetActive(true);
                shopPlayerMoneyText.text = "�����ݾ� : " + GameManager.instance.player.Gold;//������ �ѹ� �־���� �ʱⰪ �ֽ�ȭ, �������� ������Ƽ������
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

    void DayNightControll()//���� ���� ���� ��ũ��Ʈ�� ��ȣ�ۿ� �������
    {
        if (isNight)//�㿡�� ��
        {
            worldLight.intensity += Time.deltaTime / 100;//�㳷 ����
            if (worldLight.intensity >= 1)
            {
                isNight = false;
                RenderSettings.skybox = daySkybox;
            }
        }
        else//������ ��
        {
            worldLight.intensity -= Time.deltaTime / 100;//�㳷 ����
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
            timer += Time.deltaTime;//�ε��ð� ���� ������� ����
            loadingProgressText.text = "�ε���.. " + (loadSlider.value * 100) + "%";
            if (asyncLoad.progress < 0.9f)
            {
                loadSlider.value = Mathf.Lerp(0, 1f, timer);
                if(loadSlider.value >= asyncLoad.progress)
                {
                    timer = 0;
                }
            }
            else//�ε����γ����� ���̵��ϴºκ�
            {
                loadSlider.value = Mathf.Lerp(loadSlider.value, 1f, timer);
                if(loadSlider.value >=0.999f)
                {
                    loadingProgressText.text = "�ε���.. " + 100 + "%";
                    asyncLoad.allowSceneActivation = true;
                    break;
                }
            }
        }
        yield return new WaitForSeconds(2f);//�ε������� ���̵��Ǵ� �������ڵ� ���⼭  ��Ȱ��ȭ�ɾ�ߴ�
        loadingSceneUIObject.SetActive(false);
        mainmanuUIObject.SetActive(false);
    }
    public void PurchaseButton()//������ ���� �߰�
    {
        if (Shop.pickItem != null)
        {
            if (player.Gold >= Shop.pickItem.price)//������ ��¹�ư ��尡 ������
            {
                SoundManager.instance.soundObjectPool.Pop(SoundManager.instance.coindropSound, GameManager.instance.player.transform);
                if (Shop.pickItem.itemType == ItemType.consumable && shop.CheckInvenConsum())//�Ҹ����̰� �κ��� �̹� �����ÿ� ������ �߰�
                {
                    player.Gold -= Shop.pickItem.price;

                }
                else if (Shop.pickItem.itemType == ItemType.consumable) //�Ҹ�ǰ�� ù ���Ÿ�
                {
                    shop.clone = Instantiate(Shop.pickItem, UIManager.instance.player.invenObj.transform);//�ν���Ʈ�� �ؼ� ������ �����ϰ� Ŭ���� ���� �κ����� ����, �κ��� �����Ͱ� �ٲ� ����Ʋ�� ����
                    ((ConsumableItem)shop.clone).ItemCount++;//���⸸ �ٸ� ������ ù���ſ� ī��Ʈ ������ ���⼭ ���ټ� �ۿ� ����
                    player.inventory.AddItem(shop.clone);
                    player.Gold -= Shop.pickItem.price;
                }
                else//���� 
                {
                    //���� �κ� ������Ʈ�� Ŭ�и�������� �־��༭ �������ش�
                    shop.clone = Instantiate(Shop.pickItem, UIManager.instance.player.invenObj.transform);//�ν���Ʈ�� �ؼ� ������ �����ϰ� Ŭ���� ���� �κ����� ����, �κ��� �����Ͱ� �ٲ� ����Ʋ�� ����
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
        if (isOkbutton)//����
        {
            if(curConverState == ConverState.shop)
            {
                shopUIObject.SetActive(true);
            }
            if(curConverState == ConverState.quest)
            {
                //����Ʈ Ŭ����� ������ Ŭ����� �ڵ� ��������Ʈ ����
                QuestManager.instance.PlayerCurQuest.State = QuestState.IsClear;
            }
            curConverState = ConverState.None;
        }
        else if(!isOkbutton)//����
        {
            //�ƹ��͵����� ����
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
                    "�̸� : " + item.itemName + "\n" +
                    "Ÿ�� : " + "�Ҹ�ǰ" + "\n" +
                    "���� : " + item.itemInfo + "\n";
                case ItemType.armor:
                    return
                    "�̸� : " + item.itemName + "\n" +
                    "Ÿ�� : " + "��" + "\n" +
                    "ü�� ������ : " + ((Armor)item).additionalHp + "\n" +
                    "���� : " + item.itemInfo + "\n";
                case ItemType.weapon:
                    return
                    "�̸� : " + item.itemName + "\n" +
                    "Ÿ�� : " + "����" + "\n" +
                    "���ݷ� ������ : " + ((Weapon)item).additionalAtk + "\n" +
                    "���� : " + item.itemInfo + "\n";
                default:
                    return null;
            }
        }
        return null;
    }
}
