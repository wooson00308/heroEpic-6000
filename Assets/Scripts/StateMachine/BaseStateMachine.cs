using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.StateMachine
{
    public abstract class BaseStateMachine : MonoBehaviour
    {
        protected Unit _unit;
        protected Animator _animator;
        protected FSM _fsm;

        protected FSM.State _idleState;
        protected FSM.State _runState;
        protected FSM.State _hitState;
        protected FSM.State _deathState;

        protected readonly int _idleHash = Animator.StringToHash("Idle");
        protected readonly int _runHash = Animator.StringToHash("Run");
        protected readonly int _hitHash = Animator.StringToHash("Hit");
        protected readonly int _deathHash = Animator.StringToHash("Death");

        protected float _currentDeathTime;

        public float deathDurationTime;

        public Unit Unit => _unit;
        public Animator Animator => _animator;
        protected virtual void Awake()
        {
            _unit = GetComponent<Unit>();
            _animator = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            SetState();

            _fsm = new FSM();
            _fsm.Start(_idleState);
        }

        protected virtual void SetState()
        {
            _idleState = IdleState;
            _runState = RunState;
            _hitState = HitState;
            _deathState = DeathState;
        }

        public virtual void OnUpdate()
        {
            _fsm.OnUpdate();
        }

        protected abstract void IdleState(FSM fsm, FSM.Step step, FSM.State state);
        protected abstract void RunState(FSM fsm, FSM.Step step, FSM.State state);
        protected abstract void HitState(FSM fsm, FSM.Step step, FSM.State state);

        protected virtual IEnumerator DelayFrameHit()
        {
            yield return new WaitForEndOfFrame();

            if (_animator.GetBool("Hit")) yield break;

            Hit();
        }

        protected virtual void Hit()
        {

        }

        protected abstract void DeathState(FSM fsm, FSM.Step step, FSM.State state);

        public virtual void OnHit()
        {
            _fsm.TransitionTo(_hitState);
        }

        public virtual void OnDeath()
        {
            _unit.IsDeath = true;
            _fsm.TransitionTo(_deathState);
        }

        protected virtual void TryTransitionTo(FSM.State state)
        {
            _fsm.TransitionTo(state);
        }
    }
}

