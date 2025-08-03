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
            //只要检测到玩家就重置计时
            stateTimer = enemy.battleTime;
            if (enemy.DetectPlayer().distance < enemy.attackDis
            && Time.time > enemy.lastAttackTime + enemy.attackCoolDown)
            { 
                stateMachine.ChangeState(enemy.attackState);
            }
        }

        //这里也可以再添加一下 距离过远的条件
        if (stateTimer < 0)
        { 
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
