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
        //�ǲ���Ҫ��һ�� �л���״̬һ����return
        currentState.Exit();
        currentState = _playerState;
        currentState.Enter();
    }
}
