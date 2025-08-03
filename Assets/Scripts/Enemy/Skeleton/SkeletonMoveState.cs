using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMoveState : EnemyState
{
    Enemy_Skeleton enemy;
    public SkeletonMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, 
        string _animBoolName, Enemy_Skeleton _enemy) :
        base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
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

        enemyBase.SetVelocity(enemyBase.moveSpeed * enemyBase.FaceDir, enemyBase.rb.velocity.y);


        if (!enemyBase.IsGrounded || enemyBase.IsWalled)
        {
            //Flip µÄÊ±ºòÍ£Ò»Ãë
            enemyBase.Flip();
            stateMachine.ChangeState(enemy.idleState);
        }
        if (enemy.isDetectedPlayer)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
