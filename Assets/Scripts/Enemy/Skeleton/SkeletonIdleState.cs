using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : EnemyState
{
    Enemy_Skeleton enemy;
    public SkeletonIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : 
        base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemyBase.SetVelocity(Vector2.zero);

        // ����Idle״̬�� ͣ�� һ��ʱ��
        stateTimer = enemy.idleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0f)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
        if (enemy.isDetectedPlayer)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
