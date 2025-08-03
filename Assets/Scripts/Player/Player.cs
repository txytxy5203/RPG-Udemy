using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D;


public class Player : Entity
{
    private static Player instance;
    public static Player Instance => instance;

    [Header("攻击相关细节")]
    public Vector2[] attackMovement;
    public float counterAttackDuration;

    [SerializeField] float moveSpeed;
    [SerializeField] float jumpSpeed;

    #region Dash
    [SerializeField] float dashDurationTime;
    //[SerializeField] float dashCoolDownTime;
    [SerializeField] float dashSpeed;

    public int DashDir { get; private set; }
    float dashTimer;
    bool IsStartDashTimer;
    #endregion

    public float MoveSpeed => moveSpeed;
    public float JumpSpeed => jumpSpeed;
    public float DashSpeed => dashSpeed;


    public bool IsBusy { get; private set; }

    #region 组件
    public PlayerInput playerInput { get; private set; }
    public SkillManager skillManager { get; private set; }
    #endregion
    #region 状态机相关
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerPrimaryAttackState primaryAttackState { get; private set; }
    public PlayerCounterAttackState counterAttackState { get; private set; }
    public PlayerAimSwordState aimSwordState { get; private set; }
    public PlayerCatchSwordState catchSwordState { get; private set; }
    #endregion

    public GameObject currentSword;
    protected override void Awake()
    {
        instance = this;
        base.Awake();
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "Jump");
        primaryAttackState = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttackState = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
        
        aimSwordState = new PlayerAimSwordState(this, stateMachine, "AimSword");
        catchSwordState = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
    }
    protected override void Start()
    {
        base.Start();


        //输入控制相关
        playerInput = GetComponent<PlayerInput>();
        playerInput.actions.Enable();

        skillManager = SkillManager.instance;

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();
        Dash();

        
    }
    void Dash()
    {
        //这里把Dash的状态切换写到了Player中
        if (playerInput.actions["Dash"].ReadValue<float>() == 1f 
            && SkillManager.instance.dash.CanUseSkill())
        {
            float inputTemp = playerInput.actions["Move"].ReadValue<float>();
            
            if (inputTemp > 0)
            { 
                DashDir = 1;
            }
            else if(inputTemp < 0)
            {
                 DashDir = -1;
            }
            else
            {
                DashDir = FaceDir;
            }
            stateMachine.ChangeState(this.dashState);
        }
    }
    //开始Dash计时
    public void StartDashTimer()
    {
        StartCoroutine(DashTimer());
    }
    IEnumerator DashTimer()
    {
        yield return new WaitForSeconds(dashDurationTime);
        stateMachine.ChangeState(this.idleState);
    }
    public void StartWallJumpTimer()
    { 
        StartCoroutine(WallJumpTimer());
    }
    IEnumerator WallJumpTimer()
    {
        yield return new WaitForSeconds(.2f);
        stateMachine.ChangeState(this.airState);
    }

    public void AnimatorFinish() => stateMachine.currentState.AnimFinishTrigger();
    public IEnumerator BusyFor(float time)
    { 
        IsBusy = true;
        yield return new WaitForSeconds(time);
        IsBusy = false;
    }

    public void CatchSword()
    {
        if (currentSword != null)
        {
            stateMachine.ChangeState(catchSwordState);
            Destroy(currentSword);
        }
    }
}
