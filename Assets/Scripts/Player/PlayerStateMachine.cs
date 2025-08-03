using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine 
{
    public PlayerState currentState { get; private set; }

    public void Initialize(PlayerState _startState)
    { 
        currentState = _startState;
        currentState.Enter();
    }
    public void ChangeState(PlayerState _playerState)
    { 
        //是不是要加一个 切换的状态一样就return
        currentState.Exit();
        currentState = _playerState;
        currentState.Enter();
    }
}
