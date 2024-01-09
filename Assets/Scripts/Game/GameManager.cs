using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public Player player;
    public MonsterObjectPool monsterObjectPool;
    public TextObjectPool textObjectPool;
    public bool isSceneInGame;
    public bool isFirstEnterGame;
    public GameObject mainCamera;
    private int monsterSpawnCount;
    Coroutine coroutine;
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += LoadedsceneEvent;
    }

    // Update is called once per frame
    void Update()
    {
        if(isSceneInGame)
        {
            SetCursor();
        }
    }

    private void LoadedsceneEvent(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "MainMenu")
        {
            isSceneInGame = false;
            StopCoroutine(coroutine);
            Cursor.lockState = CursorLockMode.None;
        }
        if (scene.name == "InGame" && isFirstEnterGame == false)//ó�� �ΰ������� ���� �̺�Ʈ�Լ� �������ָ��
        {
            SetInGameSceneStart();
            isFirstEnterGame = true;
        }
        if (scene.name == "InGame")
        {
            isSceneInGame = true;
            coroutine = StartCoroutine(MonsterSpawn());
        }
    }

    void SetInGameSceneStart()
    {
        isSceneInGame = true;
        UIManager.instance.mainUIObject.SetActive(true);
        monsterObjectPool.InitPool();
        textObjectPool.InitPool();
    }

    void SetCursor()
    {
        if (!UIManager.instance.IsUIActive)
        {
            Cursor.lockState = CursorLockMode.Locked;//�߾Ӱ���
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
        //Cursor.lockState = CursorLockMode.Confined;//������ �� ������
    }
    IEnumerator MonsterSpawn()
    {
        monsterSpawnCount = 10;
        while (true)
        {
            if (Monster.monsterTotalCount < monsterSpawnCount)//�ʵ� ��ȯ����
            {
                yield return new WaitForSeconds(2f);//2�ʴ� �ٸ�������Ʈ�� �Ѱ��༭ ��ȯ���� 2�ʵ��� ��
                switch (Random.Range(1, 3))
                {
                    case 1:
                        if(Bear.bearTypeCount <= monsterSpawnCount/2)
                        {
                            monsterObjectPool.PopObj(MonsterType.Bear);
                        }
                        break;
                    case 2:
                        if(Skeleton.skeletonTypeCount <= monsterSpawnCount/2)
                        {
                            monsterObjectPool.PopObj(MonsterType.Skeleton);
                        }
                        break;
                    default:
                        break;
                }
            }
            yield return null;
        }
    }


}
