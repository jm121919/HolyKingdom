using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveController : MonoBehaviour
{
    private Player player;

    [SerializeField]//�ν����Ϳ����� ���� �����ϰ�
    private float smoothRotationTime;//target ������ ȸ���ϴµ� �ɸ��� �ð�
    [SerializeField]
    private float smoothMoveTime;//target �ӵ��� �ٲ�µ� �ɸ��� �ð�
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
        moveForce = (Math.Abs(moveDirection.x) + Math.Abs(moveDirection.y)) * moveSpeed;//���밪 �ΰ� ���ϰ� ���ǵ带 ���Ѱ� �ִϸ��̼ǰ� ȣȯ
        if ((player.IsAction == false || player.curPlayerState == Player_State.Hit) && UIManager.instance.IsUIActive == false)//������ ���Ǻ�
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
        if (0 < moveForce && moveForce < 9)//������ ���ǵ�� ����� �������� ��ġ��
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
        if (moveDirection != Vector2.zero)//�������� ������ �� �ٽ� ó�� ������ ���ư��°� ��������
        {
            float rotation = Mathf.Atan2(moveDirection.x, moveDirection.y) * Mathf.Rad2Deg + maincameraTransform.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, rotation, ref rotationVelocity, smoothRotationTime);
        }

        //������ �����ִ� �ڵ�, �÷��̾ ������ �� �밢������ �����Ͻ� �� ������ �ٶ󺸰� ���ش�
        //Mathf.Atan2�� ������ return�ϱ⿡ �ٽ� ������ �ٲ��ִ� Mathf.Rad2Deg�� �����ش�
        //Vector.up�� y axis�� �ǹ��Ѵ�
        //SmoothDampAngle�� �̿��ؼ� �ε巴�� �÷��̾��� ������ �ٲ��ش�.

        targetSpeed = moveSpeed * moveDirection.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedVelocity, smoothMoveTime);
        //���罺�ǵ忡�� Ÿ�ٽ��ǵ���� smoothMoveTime ���� ���Ѵ�
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
            //if (animTime > 0 && animTime < 0.9f)//�߰������� �̵������ϴٰ� �������� ������ �̵��Ұ�
            //{

            //}
            if (animTime >= 0.9f)
            {
                // �ִϸ��̼� ����
                player.ChangeState(Player_State.Idle);
            }
        }
    }
}
