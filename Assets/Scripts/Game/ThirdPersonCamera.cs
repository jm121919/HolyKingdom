using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public float Xaxis;
    public float Yaxis;
    private float dis = 5f;//카메라와 플레이어사이의 거리
    private float rotSensitive = 3f;//카메라 회전 감도
    private float xRotationMin = -20f;//X축 기준 카메라 회전각도 최소
    private float xRotationMax = 50f;//X축 기준 카메라 회전각도 최대
    private float smoothTime = 0.12f;//카메라가 회전하는데 걸리는 시간
    // 5개의 값은 취향대로 조정할 값이다
    private Vector3 targetRotation;
    private Vector3 currentVel;
    // 2개는 smoth함수에만 쓰는 단순 변수들

    [SerializeField]
    private float m_ZoomSpeed = 3f; //마우스 줌 스피드
    private Transform playerTransform;
    public RaycastHit rayhit;
    private bool onHit;
    public LayerMask ignoreLayerMask;

    private void Awake()
    {
        SoundManager.instance.soundObjectPool.Pop(SoundManager.instance.bgmSound, transform,true);
        GameManager.instance.mainCamera = gameObject;
    }
    public void LateUpdate()
    {
        if (UIManager.instance.IsUIActive == false)//유아이 열려있음 카메라막음 
        {
            CameraMove();
            Zoom();
        }
    }
    private void Start()
    {
        playerTransform = FindAnyObjectByType<Player>().transform;
    }
    void CameraMove()
    {
        // 이슈 벡터를 업으로 해준만큼, 똑같은수치로 계속 검사를 해야 하는데 움직이는 벡터는 2더하고 검사백터는 1을 더해줘서 오류가낫었다
        Xaxis += Input.GetAxis("Mouse Y") * rotSensitive;//x축기준 회전값
        Yaxis += Input.GetAxis("Mouse X") * rotSensitive;//y축기준 회전값

        Xaxis = Mathf.Clamp(Xaxis, xRotationMin, xRotationMax);//X축 기준 회전 y이동 막기
        targetRotation = Vector3.SmoothDamp(targetRotation, new Vector3(Xaxis, Yaxis), ref currentVel, smoothTime);
        //타겟 로테이션은 처음에 비어있고, 내 마우스 값과, 현제 속력과 스무스이동시간을 더해줘 좀더 부드럽게 마우스 셋으로 로테이션이 돌아가게 세팅
        this.transform.localEulerAngles = targetRotation;
        //카메라 오일러 각을 부드럽게 마우스 값을 씌운 회전으로 대입해줌
        Debug.DrawRay(playerTransform.position + Vector3.up, (transform.position + Vector3.down - playerTransform.position).normalized * dis, Color.red);
        onHit = Physics.Raycast(playerTransform.position + Vector3.up, (transform.position + Vector3.down - playerTransform.position).normalized, out rayhit, dis, ~(ignoreLayerMask));//플레이어,몬스터는 예외
        //벡터 업다운 해준건 중앙을 맞춰주기 위하여
        if (onHit)//먼저 긴 카메라 거리레이
        {
            float disTemp = Mathf.Abs(Vector3.Distance(rayhit.point,playerTransform.position));//두 사이의 거리를 절대값으로 씌움
            this.transform.position = rayhit.point;//그 위치로 이동
            this.transform.position = playerTransform.position - (transform.forward * disTemp) + (Vector3.up);//벡터 더해준이유는 트랜스폼이 발밑이라
        }
        else
        {
            this.transform.position = playerTransform.position - (transform.forward * dis) + (Vector3.up);//벡터 더해준이유는 트랜스폼이 발밑이라
            //플레이어를 기준으로 앞을 세팅해야하니 방향벡터 하듯이 플레이어에서 나의 앞을 빼는식으로 진행, dis거리값을 조정하여 플레이어와 카메라의 거리설정
        }
    }


    void Zoom()
    {
        float m_scroll = Input.GetAxis("Mouse ScrollWheel");
        if (m_scroll != 0)
        {
            dis -= m_scroll * m_ZoomSpeed; //카메라 확대 축소 dis의 값만 스크롤로 조정
        }
    }

}
