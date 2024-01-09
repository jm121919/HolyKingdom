using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveController : MonoBehaviour
{
    private Player player;

    [SerializeField]//인스펙터에서만 참조 가능하게
    private float smoothRotationTime;//target 각도로 회전하는데 걸리는 시간
    [SerializeField]
    private float smoothMoveTime;//target 속도로 바뀌는데 걸리는 시간
    private float rotationVelocity;//The current velocity, this value is modified by the function every time you call it.
    private float speedVelocity;//The current velocity, this value is modified by the function every time you call it.
    private float currentSpeed;
    private float targetSpeed;

    public Vector2 moveDirection;
    public float moveForce;
    public float moveSpeed;
    public float walkSpeed;
    public float runSpeed;
    public float jumpSpeed;
    public float dodgeSpeed;
    private Animator animator;
    private Transform maincameraTransform;
    public LayerMask groundLayer;
    private const float groundLayerLength = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        maincameraTransform = Camera.main.transform;
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();

        moveSpeed = walkSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical")).normalized;
        moveForce = (Math.Abs(moveDirection.x) + Math.Abs(moveDirection.y)) * moveSpeed;//절대값 두개 더하고 스피드를 곱한값 애니메이션과 호환
        if ((player.IsAction == false || player.curPlayerState == Player_State.Hit) && UIManager.instance.IsUIActive == false)//움직임 조건부
        {
            Move();
            Run();
            Jump();
            Dodge();
        }    
        UpdateAnimeState();
    }

    private void CheckMoveState()
    {
        if (0 < moveForce && moveForce < 9)//걸을때 스피드와 연계된 무브포스 수치값
        {
            player.ChangeState(Player_State.Walk);
        }
        if (moveForce >= 9)
        {
            player.ChangeState(Player_State.Run);
        }
        if (moveForce <= 0 && (player.IsAction == false))
        {
            player.ChangeState(Player_State.Idle);
        }
    }
    private void UpdateAnimeState()
    {
        CheckAnimeState("Dodge");
        CheckAnimeState("Jump");
    }
    void Move()
    {
        if (moveDirection != Vector2.zero)//움직임을 멈췄을 때 다시 처음 각도로 돌아가는걸 막기위함
        {
            float rotation = Mathf.Atan2(moveDirection.x, moveDirection.y) * Mathf.Rad2Deg + maincameraTransform.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, rotation, ref rotationVelocity, smoothRotationTime);
        }

        //각도를 구해주는 코드, 플레이어가 오른쪽 위 대각선으로 움직일시 그 방향을 바라보게 해준다
        //Mathf.Atan2는 라디안을 return하기에 다시 각도로 바꿔주는 Mathf.Rad2Deg를 곱해준다
        //Vector.up은 y axis를 의미한다
        //SmoothDampAngle을 이용해서 부드럽게 플레이어의 각도를 바꿔준다.

        targetSpeed = moveSpeed * moveDirection.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedVelocity, smoothMoveTime);
        //현재스피드에서 타겟스피드까지 smoothMoveTime 동안 변한다
        transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);

        CheckMoveState();
    }
    void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = runSpeed;
        }
        else
        {
            moveSpeed = walkSpeed;
        }
    }
    void Jump()
    {
        Debug.DrawRay(transform.position,Vector3.down * groundLayerLength, Color.red);
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, groundLayerLength, groundLayer))
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                player.ChangeState(Player_State.Jump);
                player.GetComponent<Rigidbody>().AddRelativeForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
            }
        }
    }
    void Dodge()
    {
        if(Input.GetKeyDown (KeyCode.Space) && player.curPlayerState != Player_State.Dodge)
        {
            player.ChangeState(Player_State.Dodge);
            player.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * dodgeSpeed, ForceMode.Impulse);
        }
    }

    void CheckAnimeState(string name)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(name) == true)
        {
            float animTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            //if (animTime > 0 && animTime < 0.9f)//중간까지는 이동가능하다가 고점으로 갔으면 이동불가
            //{

            //}
            if (animTime >= 0.9f)
            {
                // 애니메이션 종료
                player.ChangeState(Player_State.Idle);
            }
        }
    }
}
