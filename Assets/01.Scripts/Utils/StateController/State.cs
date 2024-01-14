using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected StateController _controller;
    
    public State(StateController controller)
    {
        _controller = controller;
    }
    
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
}
