using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    public PlayerAimSwordState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocity(0, 0);
    }

    public override void Exit()
    {
        base.Exit();
        SkillManager.instance.sword.ClearAimVisualObj();
    }

    public override void Update()
    {
        base.Update();

        //可视化瞄准
        SkillManager.instance.sword.AimVisual();

        //Flip
        float mouseDirX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        if (mouseDirX > player.transform.position.x && !player.IsFaceRight)
        { 
            player.Flip();
        }
        else if(mouseDirX < player.transform.position.x && player.IsFaceRight)
        {
            player.Flip();
        }


        if (player.playerInput.actions["Aim"].ReadValue<float>() == 0f)
        {              
            stateMachine.ChangeState(player.idleState);
        }
    }
}
