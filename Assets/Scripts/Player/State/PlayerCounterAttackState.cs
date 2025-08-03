using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    public PlayerCounterAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = player.counterAttackDuration;
        player.animator.SetBool("SuccessfulCounterAttack", false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(Vector2.zero);


        Collider2D[] colliders = Physics2D.OverlapCircleAll(
                player.attackTrans.position, player.attackCheckRadius,
                1 << LayerMask.NameToLayer("Enemy"));
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.GetComponent<Enemy>() != null
                && collider.gameObject.GetComponent<Enemy>().CanBeStunned())
            {
                stateTimer = 10f;  //any value bigger than 1
                player.animator.SetBool("SuccessfulCounterAttack", true);
            }
        }

        if(stateTimer < 0 || triggerFinish)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
