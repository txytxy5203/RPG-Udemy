using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    private Transform swordTrans;
    public PlayerCatchSwordState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        swordTrans = player.currentSword.transform;

        float mouseDirX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        if (mouseDirX > player.transform.position.x && !player.IsFaceRight)
        {
            player.Flip();
        }
        else if (mouseDirX < player.transform.position.x && player.IsFaceRight)
        {
            player.Flip();
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (triggerFinish)
        { 
            stateMachine.ChangeState(player.idleState);
        }
    }
}
