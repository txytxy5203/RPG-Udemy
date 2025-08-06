using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    #region 检测玩家
    [SerializeField] protected LayerMask playerMask;
    /// <summary>
    /// 前方检测距离
    /// </summary>
    [SerializeField] protected float forwardDetectDis;
    /// <summary>
    /// 后方检测距离
    /// </summary>
    [SerializeField] protected float behindDetectDis;
    public bool isDetectedPlayer { get; private set; } = false;
    #endregion

    [Header("Stunned Info")]
    public float stunDuration;
    public Vector2 stunDirection;
    protected bool canBeStunned;
    //弹反时机提示
    [SerializeField] GameObject counterImage;

    [Header("Move Info")]
    public float moveSpeed;
    public float moveToPlayerSpeed;
    public float idleTime;
    float deafaultSpeed;

    [Header("Attack Info")]
    public float attackDis;
    public float attackCoolDown;
    /// <summary>
    /// 处于战斗中的最大时间
    /// </summary>
    public float battleTime;
    [HideInInspector] public float lastAttackTime;
    public EnemyStateMachine stateMachine { get; private set; }


    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
        deafaultSpeed = moveSpeed;
    }
    
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();

        isDetectedPlayer = DetectPlayer();
    }

    public virtual void FreezeTime(bool _timeFrozen)
    {
        if(_timeFrozen)
        {
            moveSpeed = 0;
            animator.speed = 0;
            //rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            moveSpeed = deafaultSpeed;
            animator.speed = 1;
        }
    }
    protected virtual IEnumerator FreezeTimeFor(float _seconds)
    {
        FreezeTime(true);
        yield return new WaitForSeconds(_seconds);
        FreezeTime(false);
    }
    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;
        counterImage.SetActive(true);
    }
    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        counterImage.SetActive(false);
    }
    public virtual bool CanBeStunned()
    {
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }
        return false;
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(wallCheck.position, 
            new Vector3(wallCheck.position.x, wallCheck.position.y) 
            + wallCheck.transform.right * forwardDetectDis);
        Gizmos.DrawLine(wallCheck.position,
            new Vector3(wallCheck.position.x, wallCheck.position.y)
            - wallCheck.transform.right * behindDetectDis);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position,
            new Vector3(transform.position.x + attackDis * transform.right.x, 
                        transform.position.y ));
    }
    /// <summary>
    /// 检测旁边是否有玩家
    /// </summary>
    public virtual RaycastHit2D DetectPlayer()
    {
        RaycastHit2D forward = Physics2D.Raycast(wallCheck.position, 
                                Vector2.right * FaceDir, forwardDetectDis, playerMask);
        RaycastHit2D behind = Physics2D.Raycast(wallCheck.position,
                        Vector2.right * -FaceDir, behindDetectDis, playerMask);
        //这里检测Player的方式有很多种 可以自己添加 范围检测等等
        if (forward)
        {
            //这里是前后都检测 前后检测的距离不一样
            return forward;
        }
        else if (behind)
        { 
            return behind;
        }
        //找不到传默认值
        return default(RaycastHit2D);
    }

    public void AnimatorFinish() => stateMachine.currentState.AnimFinishTrigger();
}
