using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected StateController Controller { get; private set; }
    protected Entity Owner { get; private set; }
    
    public State(StateController controller)
    {
        Controller = controller;
        Owner = Controller.Owner;
    }
    
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
}
