using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState curState;

    public PlayerStateMachine(PlayerState State)
    {
        this.curState = State;
        ChangeState(curState);
    }

    public void UpdateState()
    {
        if(curState != null)
        {
            curState.onStateUpdate();
        }
    }
    public void ChangeState(PlayerState nextState)
    {
        if (curState == nextState)//같으면 그냥 나감
        {
            if (curState.player.curPlayerState != Player_State.Attack)//공격상태만 예외적으로 재진입 할때 콤보 올려주기 코드 넘겨주기
            {
                return;
            }
        }    
        if (curState != null)//널이 아니면 스테이트가 종료이므로 나가는 함수실행
            curState.onStateExit();

        curState = nextState;
        curState.onStateEnter();
    }
}

public abstract class PlayerState
{
    protected PlayerStateMachine playerStateMachine;
    public Player player;
    public PlayerState(Player player)
    {
        this.player = player;
    }

    public abstract void onStateEnter();
    public abstract void onStateUpdate();
    public abstract void onStateExit();
}
public class IdleState : PlayerState
{
    public IdleState(Player player) : base(player)
    {

    }
    public override void onStateEnter()
    {
        Debug.Log("IdleState :" + "onStateEnter ");
    }

    public override void onStateUpdate()
    {
        Debug.Log("IdleState :" + "onStateUpdate ");
    }

    public override void onStateExit()
    {
        Debug.Log("IdleState :" + "onStateExit ");
    }
}

public class WalkState : PlayerState
{
    public WalkState(Player player) : base(player)
    {

    }
    public override void onStateEnter()
    {
        Debug.Log("WalkState :" + "onStateEnter ");
    }

    public override void onStateUpdate()
    {
        Debug.Log("WalkState :" + "onStateUpdate ");
    }

    public override void onStateExit()
    {
        Debug.Log("WalkState :" + "onStateExit ");
    }
}

public class RunState : PlayerState
{
    public RunState(Player player) : base(player)
    {

    }
    public override void onStateEnter()
    {
        Debug.Log("RunState :" + "onStateEnter ");
    }

    public override void onStateUpdate()
    {
        Debug.Log("RunState :" + "onStateUpdate ");
    }

    public override void onStateExit()
    {
        Debug.Log("RunState :" + "onStateExit ");
    }
}

public class AttackState : PlayerState
{
    public static int attackComboIndex = 1;
    public AttackState(Player player) : base(player)
    {

    }
    public override void onStateEnter()
    {
        Debug.Log("AttackState : " + "onStateEnter ");
    }

    public override void onStateUpdate()
    {
        Debug.Log("AttackState : " + "onStateUpdate ");
    }

    public override void onStateExit()
    {
        Debug.Log("AttackState : " + "onStateExit ");
    }
}

public class DodgeState : PlayerState
{
    public DodgeState(Player player) : base(player)
    {

    }
    public override void onStateEnter()
    {
        Debug.Log("DodgeState :" + "onStateEnter ");
    }

    public override void onStateUpdate()
    {
        Debug.Log("DodgeState :" + "onStateUpdate ");
    }

    public override void onStateExit()
    {
        Debug.Log("DodgeState :" + "onStateExit ");
    }
}

public class JumpState : PlayerState
{
    public JumpState(Player player) : base(player)
    {

    }
    public override void onStateEnter()
    {
        Debug.Log("JumpState :" + "onStateEnter ");
    }

    public override void onStateUpdate()
    {
        Debug.Log("JumpState :" + "onStateUpdate ");
    }

    public override void onStateExit()
    {
        Debug.Log("JumpState :" + "onStateExit ");
    }
}

public class HitState : PlayerState
{
    public HitState(Player player) : base(player)
    {

    }
    public override void onStateEnter()
    {
        Debug.Log("HitState :" + "onStateEnter ");
    }

    public override void onStateUpdate()
    {
        Debug.Log("HitState :" + "onStateUpdate ");
    }

    public override void onStateExit()
    {
        Debug.Log("HitState :" + "onStateExit ");
    }
}

public class SkillState : PlayerState
{
    public SkillState(Player player) : base(player)
    {

    }
    public override void onStateEnter()
    {
        Debug.Log("SkillState :" + "onStateEnter ");
    }

    public override void onStateUpdate()
    {
        Debug.Log("SkillState :" + "onStateUpdate ");
    }

    public override void onStateExit()
    {
        Debug.Log("SkillState :" + "onStateExit ");
    }
}