using System;
using System.Collections.Generic;
using UnityEngine;

namespace StateControlSystem
{
    public class StateMachine
    {
        public State CurrentState { get; private set; }
        private readonly Dictionary<System.Enum, State> _stateDictionary;

        public StateMachine()
        {
            _stateDictionary = new Dictionary<Enum, State>();
        }

        public void Initialize(Entity owner, Enum startState)
        {
            CurrentState = _stateDictionary[startState];
            CurrentState.EnterState();
        }

        public void ChangeState(Enum stateType)
        {
            CurrentState?.ExitState();
            CurrentState = _stateDictionary[stateType];
            CurrentState.EnterState();
        }

        public void RegisterState(Enum stateType, State state)
        {
            if (_stateDictionary.ContainsKey(stateType))
            {
                Debug.Log($"{stateType.ToString()} is already register in stateMachine.");
                return;
            }
            
            _stateDictionary.Add(stateType, state);
        }
    }
}
