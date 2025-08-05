using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackHoleState : PlayerState
{
    float flyTime = .4f;
    bool skillUsed;
    public PlayerBlackHoleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        skillUsed = false;
        stateTimer = flyTime;
        player.rb.gravityScale = 0f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer > 0)
        {
            player.rb.velocity = new Vector2(0, 15);
        }
        if(stateTimer < 0)
        {
            player.rb.velocity = new Vector2(0, -.1f);
            if(!skillUsed)
            {
                if(player.skillManager.blackHole.CanUseSkill())
                    skillUsed = true;
            }
        }
    }
}
