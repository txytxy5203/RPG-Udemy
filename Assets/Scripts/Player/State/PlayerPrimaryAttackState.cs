using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    private int comboCounter;
    private float comboIntervalTime = .3f;
    private float lastAttackTime;
    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //施加一个类似于惯性的效果
        stateTimer = .1f;

        //连击处理
        if (comboCounter > 2 || Time.time > lastAttackTime + comboIntervalTime)
        {
            comboCounter = 0;
        }

        //增加一个攻击后的小位移
        float attackDir = player.FaceDir;
        if (player.playerInput.actions["Move"].ReadValue<float>() != 0)
        {
            attackDir = player.playerInput.actions["Move"].ReadValue<float>() > 0 ? 1 : -1;
        }
        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, 
                           player.attackMovement[comboCounter].y);

        player.animator.SetInteger("ComboCounter", comboCounter);
    }

    public override void Exit()
    {
        base.Exit();
        comboCounter++;
        lastAttackTime = Time.time;
        //攻击完之后进入Busy状态
        player.StartCoroutine(player.BusyFor(.15f));
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0f)
        {
            player.SetVelocity(0, 0);
        }
        if (triggerFinish)
        { 
            stateMachine.ChangeState(player.idleState);
        }
    }
}
