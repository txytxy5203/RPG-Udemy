using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) 
        : base(_player, _stateMachine, _animBoolName)
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
        player.SetVelocity(player.playerInput.actions["Move"].ReadValue<float>() * player.MoveSpeed
                    , player.rb.velocity.y);

        if (player.playerInput.actions["Move"].ReadValue<float>() == 0f)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
