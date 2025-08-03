using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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

        if (player.playerInput.actions["UpDown"].ReadValue<float>() < 0)
        {
            player.SetVelocity(0, -6);
        }
        else
        {
            player.SetVelocity(0, -2);
        }

        if (player.IsGrounded)
        {
            stateMachine.ChangeState(player.idleState);
        }
        if (player.playerInput.actions["Jump"].ReadValue<float>() == 1f)
        { 
            stateMachine.ChangeState(player.wallJumpState);
        }
    }
}
    