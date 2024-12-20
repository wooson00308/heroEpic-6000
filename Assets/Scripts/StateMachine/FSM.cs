using Scripts.StateMachine;
using UnityEngine;

namespace Scripts
{
    public class FSM
    {
        public enum Step
        {
            Enter,
            Update,
            Exit
        }

        public delegate void State(FSM fsm, Step step, State state);

        State _currentState;

        public void Start(State startState)
        {
            TransitionTo(startState);
        }

        public void OnUpdate()
        {
            _currentState.Invoke(this, Step.Update, null);
        }

        public bool IsRunningState(State state)
        {
            return state == _currentState;
        }

        public void TransitionTo(State state)
        {
            if (IsRunningState(state)) return;

            _currentState?.Invoke(this, Step.Exit, state);
            var oldState = _currentState;
            _currentState = state;
            _currentState.Invoke(this, Step.Enter, oldState);
        }
    }
}

