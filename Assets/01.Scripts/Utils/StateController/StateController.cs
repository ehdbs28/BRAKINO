using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController
{
    private readonly Dictionary<Type, State> _states;
    
    public Entity Owner { get; private set; }
    public State CurrentState { get; private set; }

    public StateController(Entity owner)
    {
        Owner = owner;
        _states = new Dictionary<Type, State>();
    }

    public void UpdateState()
    {
        if (CurrentState is not null)
        {
            CurrentState.UpdateState();
        }
    }

    public void ChangeState(Type next)
    {
        if (CurrentState is not null)
        {
            CurrentState.ExitState();
        }

        CurrentState = _states[next];
        CurrentState.EnterState();
    }

    public void RegisterState(State state)
    {
        _states.Add(state.GetType(), state);
    }
}
