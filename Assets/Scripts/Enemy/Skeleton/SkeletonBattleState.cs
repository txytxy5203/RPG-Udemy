using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private Transform playerTrans;
    Enemy_Skeleton enemy;
    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, 
        string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        playerTrans = PlayerManager.instance.player.transform; 
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        #region Flip
        if (playerTrans.position.x > enemy.transform.position.x && !enemy.IsFaceRight)
        {
            enemy.Flip();
        }
        else if (playerTrans.position.x < enemy.transform.position.x && enemy.IsFaceRight)
        {
            enemy.Flip();
        }
        #endregion


        enemy.SetVelocity(enemy.moveToPlayerSpeed * enemy.FaceDir,enemy.rb.velocity.y);
        if (enemy.isDetectedPlayer)
        {
            //ֻҪ��⵽��Ҿ����ü�ʱ
            stateTimer = enemy.battleTime;
            if (enemy.DetectPlayer().distance < enemy.attackDis
            && Time.time > enemy.lastAttackTime + enemy.attackCoolDown)
            { 
                stateMachine.ChangeState(enemy.attackState);
            }
        }

        //����Ҳ���������һ�� �����Զ������
        if (stateTimer < 0)
        { 
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
