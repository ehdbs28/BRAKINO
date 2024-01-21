using System;
using UnityEngine;

public class InputReader : ScriptableObject
{
    public delegate void InputEventListener();
    public delegate void InputEventListener<in T>(T value);
    
    protected InputControl InputControl { get; private set; }

    protected virtual void OnEnable()
    {
        if (InputControl is null)
        {
            CreateNewInputAsset();
        }
    }

    protected virtual void CreateNewInputAsset()
    {
        InputControl = new InputControl();
    }
}
