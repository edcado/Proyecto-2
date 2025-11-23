using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerStateMachine 
{
    public PlayerState currentState {  get; private set; }

    public void Initialize(PlayerState _Startstate)
    {
        currentState = _Startstate;
        currentState.Enter();
    }
    public void ChangeState(PlayerState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        //Debug.Log("I change to " + _newState);
        currentState.Enter();
    }

}
