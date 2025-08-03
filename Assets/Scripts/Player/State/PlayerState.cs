using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PlayerState 
{
    protected PlayerStateMachine stateMachine;
    protected Player player;

    

    private string animBoolName;

    /// <summary>
    /// 方便一些计时使用
    /// </summary>
    protected float stateTimer;
    /// <summary>
    /// 是否攻击完成
    /// </summary>
    protected bool triggerFinish = false;
    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    { 
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }
    public virtual void Enter()
    {
        //Debug.Log("I enter " + animBoolName);
        triggerFinish = false;
        player.animator.SetBool(animBoolName, true);
    }
    public virtual void Update()
    { 
        //Debug.Log("I am in " + animBoolName);
        player.animator.SetFloat("yVelocity", player.rb.velocity.y);
        stateTimer -= Time.deltaTime;
    }
    public virtual void Exit()
    {
        //Debug.Log("I am out " + animBoolName);
        player.animator.SetBool(animBoolName, false);
    }
    public void AnimFinishTrigger()
    {
        triggerFinish = true;
    }
}
