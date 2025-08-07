using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    //�̳��а��л���Dashд����Player���� ��Ϊ�κ�״̬�¶����л���Dash
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.StartDashTimer();
        SkillManager.instance.clone.CreateCloneOnDashStart();
    }

    public override void Exit()
    {
        base.Exit();
        SkillManager.instance.clone.CreateCloneOnDashOver();
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(player.DashSpeed* player.DashDir, 0);
        if (player.IsWalled && !player.IsGrounded)
        {
            stateMachine.ChangeState(player.wallSlideState);
        }
    }    
}
