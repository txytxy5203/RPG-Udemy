using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (player.playerInput.actions["Counter"].ReadValue<float>() == 1f)
        {
            stateMachine.ChangeState(player.counterAttackState);
        }
        if (player.playerInput.actions["Jump"].ReadValue<float>() == 1f && player.IsGrounded)       
        {
            stateMachine.ChangeState(player.jumpState);
        }

        if (!player.IsGrounded)
        {
            stateMachine.ChangeState(player.airState);
        }

        if (player.playerInput.actions["Attack"].ReadValue<float>() == 1f)
        {
            stateMachine.ChangeState(player.primaryAttackState);
        }

        if (player.playerInput.actions["Aim"].ReadValue<float>() == 1f)
        {
            if(player.currentSword == null)
            {
                stateMachine.ChangeState(player.aimSwordState);
            }
            else
            {
                player.currentSword.GetComponent<SwordSkillController>().ReturnSword();
            }
        }
    }
}
    